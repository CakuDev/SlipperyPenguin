using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int currentLevel = 1;
    public int score = 0;
    public int timer = 0;

    public void SetGameInfo(int currentLevel, int score, int timer)
    {
        this.currentLevel = currentLevel;
        this.score = score;
        this.timer = timer;
    }

    public string GetTimerFormat()
    {
        int minutes = timer / 60;
        int seconds = timer % 60;
        return string.Format("{0}:{1}", minutes.ToString("00"), seconds.ToString("00"));
    }
}
