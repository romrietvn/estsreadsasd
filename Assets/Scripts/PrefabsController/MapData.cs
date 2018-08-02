using UnityEngine;
using System.Collections;

public class MapData
{
    public int Level { set; get; }
    public int Star     { set; get; }
    public int Score { set; get; }
    public bool IsLock { set; get; }

    public MapData(int _level,int _isLock, int _star, int _score)
    {
        Level = _level;
        Star = _star;
        Score = _score;
        IsLock = _isLock == 0;
    }
}
