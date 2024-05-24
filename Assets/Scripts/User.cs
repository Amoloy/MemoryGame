using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class User
{
    public string name;
    public List<ResultData> results;

    public User(string name)
    {
        this.name = name;
        results = new List<ResultData>();
    }
}
