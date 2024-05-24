using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserController : MonoBehaviour
{
    private User _user;
    private ReadWriteSystem _readWriteSystem;
    private GameController _gameController;
    private Window_Graph _graph;
    
    
    private int _boardSize;

    private void Awake()
    {
        _readWriteSystem = GetComponent<ReadWriteSystem>();
        _gameController = FindObjectOfType<GameController>();
        _graph = FindObjectOfType<Window_Graph>();
    }

    private void Start()
    {
        _gameController.OnResultsFormed.AddListener(UpdateUserInfo);
    }

    public void Authorise(string name)
    {
        _user = _readWriteSystem.Load(name);
    }

    public void ShowStatistics()
    {
        _graph = FindObjectOfType<Window_Graph>();
        _graph.SetData(GetByScore());
        _graph.SetHeight(_boardSize);
        _graph.Show();
    }

    private List<int> GetByScore()
    {
        List<int> missesList = new List<int>();

        for (int i = 0; i < _user.results.Count; i++)
        {
            if (_user.results[i].score * 2 == _boardSize)
            {
                missesList.Add(_user.results[i].misses);
            }
        }

        return missesList;
    }

    private void UpdateUserInfo(int score, int misses)
    {
        ResultData data = new ResultData(score, misses);
        _user.results.Add(data);
        _readWriteSystem.Save(_user);
    }

    private void UpdateBoardSize(int size)
    {
        _boardSize = size;
    }

    public void SubscribeAfterAuthorise()
    {
        SubscribeToggles();
    }

    private void SubscribeToggles()
    {
        ToggleColorHandler[] toggles = FindObjectsOfType<ToggleColorHandler>();

        foreach (var toggle in toggles)
        {
            toggle.SizeChoosen.AddListener(UpdateBoardSize);
        }
    }
}
