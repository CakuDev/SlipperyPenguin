using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RowInfo
{
    public string name;
    public int level;
    public int score;
    public int time;

    public RowInfo(string name, int level, int score, int time)
    {
        this.name = name;
        this.level = level;
        this.score = score;
        this.time = time;
    }

    public bool Compare(RowInfo rowInfo)
    {
        if (this.score > rowInfo.score) return true;
        else if (this.score < rowInfo.score) return false;

        if (this.time < rowInfo.time) return true;
        else if (this.time > rowInfo.time)  return false;

        return true;
    }
}
