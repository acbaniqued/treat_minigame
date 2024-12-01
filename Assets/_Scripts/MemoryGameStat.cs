using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryGameStat : MinigameStat
{
    public string UserName;
    public int Difficulty;
    public float SessionTime;
    public int TotalClicks;
    public bool IsSessionWin;

    public MemoryGameStat(string username, int difficulty, float sessionTime, int totalClicks, bool isWin)
    {
        UserName = username;
        Difficulty = difficulty;
        SessionTime = sessionTime;
        TotalClicks = totalClicks;
        IsSessionWin = isWin;
    }
}
