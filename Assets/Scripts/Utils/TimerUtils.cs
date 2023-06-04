using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerUtils
{
    public static string FormatTime(int totalSeconds) 
    {
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        return string.Format("{0}:{1}", minutes.ToString("00"), seconds.ToString("00"));
    }
}
