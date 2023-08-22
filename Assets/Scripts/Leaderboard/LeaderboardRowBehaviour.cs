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
    public Image background;
    public Color highlightColor;
    public Image rankFish;
    public Color fish1Color;
    public Color fish2Color;
    public Color fish3Color;

    public void OnCreate(int position, string name, int level, int score, int time)
    {
        TMPPosition.text = position.ToString();
        TMPName.text = name;
        TMPLevel.text = level.ToString();
        TMPScore.text = score.ToString();
        TMPTime.text = TimerUtils.FormatTime(time);

        if (position == 1) rankFish.color = fish1Color;
        if (position == 2) rankFish.color = fish2Color;
        if (position == 3) rankFish.color = fish3Color;
        if (position > 3) rankFish.color = new Color(0, 0, 0, 0);
    }

    public void HighlightRow()
    {
        HighlightText(TMPPosition);
        HighlightText(TMPName);
        HighlightText(TMPLevel);
        HighlightText(TMPScore);
        HighlightText(TMPTime);
        background.color = highlightColor;
    }

    private void HighlightText(TMP_Text TMPText)
    {
        Color black = new(0.1490196f, 0.1960784f, 0.1490196f);
        TMPText.fontSize += 10;
        TMPText.color = black;
    }
}
