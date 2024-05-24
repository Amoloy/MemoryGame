using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ResultData
{
    public int score;
    public int misses;

    public ResultData()
    {
        
    }

    public ResultData(int score, int misses)
    {
        this.score = score;
        this.misses = misses;
    }
}
