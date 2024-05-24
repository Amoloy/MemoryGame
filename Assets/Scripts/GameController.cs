using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


public class GameController : MonoBehaviour
{
    public GameObject GameBoardPref;
    public GameObject SettingsPanel;
    public GameObject GamePanel;
    public GameObject ResultsPanel;
    public TextMeshProUGUI ScoreLabel;
    public TextMeshProUGUI MissesLabel;

    public float ShowCardsDelay = 1f;
    
    private int _boardSize = 0;
    private int _score = 0;
    private int _misses = 0;

    private SettingsHint _settingsHint;
    private ScoreHint _scoreHint;
    private GameBoard _gameBoardPref;
    private GameBoard _gameBoardInstance;

    public UnityEvent<float> ShowCards;
    public UnityEvent<int, int> OnResultsFormed;

    private void Awake()
    {
        _settingsHint = SettingsPanel.GetComponentInChildren<SettingsHint>();
        _scoreHint = GamePanel.GetComponentInChildren<ScoreHint>();
        _gameBoardPref = GameBoardPref.GetComponent<GameBoard>();
    }

    private void Start()
    {
        TogglesSubscribe();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void StartGame()
    {
        if (_boardSize == 0)
        {
            _settingsHint.Show("Choose size");
            return;
        }
        
        _settingsHint.HideText();
        SettingsPanel.SetActive(false);
        
        GamePanel.SetActive(true);
        _scoreHint.Show(_score);
        
        CreateGameBoard();

        GameBoardSubscribe();
        StartCoroutine(coroutineShowCards());
    }

    public void EndGame()
    {
        GameBoardUnsubscribe();
        Destroy(_gameBoardInstance);
        
        SettingsPanel.SetActive(true);
        
        _scoreHint.Hide();
        GamePanel.SetActive(false);
        
        OnResultsFormed?.Invoke(_score, _misses);
        _score = 0;
        _misses = 0;
    }

    private void CreateGameBoard()
    {
        _gameBoardPref.cardsCount = _boardSize;

        if (_boardSize <= 10)
        {
            _gameBoardPref.startPos = new Vector3(-7f, 1f, 0);
            _gameBoardPref.CardScale = 1;
            _gameBoardPref.cardsInRow = 5;
            ShowCardsDelay *= 1;
        }
        else if (_boardSize <= 24)
        {
            _gameBoardPref.startPos = new Vector3(-8.5f, 2f, 0);
            _gameBoardPref.CardScale = 0.7f;
            _gameBoardPref.cardsInRow = 8;
            ShowCardsDelay *= 2;
        }
        else if (_boardSize <= 36)
        {
            _gameBoardPref.startPos = new Vector3(-8.5f, 2.5f, 0);
            _gameBoardPref.CardScale = 0.6f;
            _gameBoardPref.cardsInRow = 9;
            ShowCardsDelay *= 3;
        }
        else if (_boardSize <= 50)
        {
            _gameBoardPref.startPos = new Vector3(-7.875f, 3f, 0);
            _gameBoardPref.CardScale = 0.5f;
            _gameBoardPref.cardsInRow = 10;
            ShowCardsDelay *= 4;
        }
        
        _gameBoardInstance = Instantiate(GameBoardPref, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<GameBoard>();
    }

    private void IncreaseScore()
    {
        _score++;
        _scoreHint.Show(_score);

        if (_score * 2 == _boardSize)
        {
            ResultsPanel.SetActive(true);
            ScoreLabel.text = $"Score: {_score}";
            MissesLabel.text = $"Misses: {_misses}";
            EndGame();
        }
    }

    private void IncreaseMisses()
    {
        _misses++;
    }

    private IEnumerator coroutineShowCards()
    {
        yield return new WaitForSeconds(0.3f);
        ShowCards?.Invoke(ShowCardsDelay);
    }

    private void GameBoardSubscribe()
    {
        _gameBoardInstance.OnMatchedCards.AddListener(IncreaseScore);
        _gameBoardInstance.OnNotMatchedCards.AddListener(IncreaseMisses);
    }

    private void GameBoardUnsubscribe()
    {
        _gameBoardInstance.OnMatchedCards.RemoveListener(IncreaseScore);
        _gameBoardInstance.OnNotMatchedCards.RemoveListener(IncreaseMisses);
    }
    private void TogglesSubscribe()
    {
        ToggleColorHandler[] toggles = FindObjectsOfType<ToggleColorHandler>();

        foreach (var toggle in toggles)
        {
            toggle.SizeChoosen.AddListener(UpdateBoardSize);
        }
    }

    public void SubscribeAfterAuthorise()
    {
        TogglesSubscribe();
    }

    private void UpdateBoardSize(int size)
    {
        _boardSize = size;
    }
}
