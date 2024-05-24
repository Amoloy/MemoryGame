using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class ReadWriteSystem : MonoBehaviour
{
    private string _path;
    private List<User> _users;

    private void Awake()
    {
        _path = Path.Combine(Application.dataPath + "/UserDataFile.json");
    }

    public void Save(User user)
    {
        int index = FindUser(user.name);

        if (index == -1)
        {
            _users.Add(user);
        }
        else
        {
            _users[index] = user;
        }
        
        SaveJson();
    }

    public User Load(string name)
    {
        if (_users == null)
        {
            LoadJson();
        }
        
        int index = FindUser(name);

        if (index == -1)
        {
            return new User(name);
        }
        else return _users[index];
    }

    private int FindUser(string name)
    {
        for (int i = 0; i < _users.Count; i++)
        {
            if (_users[i].name == name)
            {
                return i;
            }
        }

        return -1;
    }

    private void SaveJson()
    {
        string json = JsonConvert.SerializeObject(new UserLIst(_users), Formatting.Indented);
        File.WriteAllText(_path, json);

    }

    private void LoadJson()
    {
        if (File.Exists(_path))
        {
            string json = File.ReadAllText(_path);
            _users = JsonConvert.DeserializeObject<UserLIst>(json).users;
        }

        if (_users == null)
        {
            _users = new List<User>();
        }
    }
}
