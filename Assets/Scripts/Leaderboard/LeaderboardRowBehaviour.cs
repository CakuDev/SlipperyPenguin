using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LeaderboardRowBehaviour : MonoBehaviour
{
    public TMP_Text TMPPosition;
    public TMP_Text TMPName;
    public TMP_Text TMPLevel;
    public TMP_Text TMPScore;
    public TMP_Text TMPTime;

    public void OnCreate(int position, string name, int level, int score, int time)
    {
        TMPPosition.text = position.ToString();
        TMPName.text = name;
        TMPLevel.text = level.ToString();
        TMPScore.text = score.ToString();
        TMPTime.text = TimerUtils.FormatTime(time);
    }

    public void HighlightRow()
    {
        HighlightText(TMPPosition);
        HighlightText(TMPName);
        HighlightText(TMPLevel);
        HighlightText(TMPScore);
        HighlightText(TMPTime);
    }

    private void HighlightText(TMP_Text TMPText)
    {
        Color black = new(0.1490196f, 0.1960784f, 0.1490196f);
        TMPText.fontSize += 10;
        TMPText.color = black;
    }
}
