using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class LocalLeaderboardRepository : MonoBehaviour
{
    public int maxElements = 50;

    BinaryFormatter formatter;
    string path;
    List<RowInfo> rows;

    // Start is called before the first frame update
    void Start()
    {
        formatter = new();
        rows = new();
        path = Application.persistentDataPath + "/leaderboard.pen";
        if(!File.Exists(path))
        {
            File.Create(path).Dispose();
        } else
        {
            FileStream stream = new(path, FileMode.Open);
            if(stream.Length != 0)
            {
                rows = formatter.Deserialize(stream) as List<RowInfo>;
            }
            stream.Close();
        }
    }

    public void SaveLeaderboardRow(RowInfo row)
    {
        AddNewRow(row);
    }

    public List<RowInfo> LoadLeaderboardData()
    {
        FileStream stream = new(path, FileMode.Open);
        formatter.Serialize(stream, rows);
        stream.Close();
        return rows;
    }

    private void AddNewRow(RowInfo row)
    {
        int position = -1;
        for(int i = 0; i < rows.Count; i++)
        {
            RowInfo iRow = rows[i];
            if(row.Compare(iRow))
            {
                position = i;
                break;
            }
        }

        if (position != -1)
        {
            rows.Insert(position, row);
            if (rows.Count > maxElements) rows.RemoveAt(maxElements);

        }
        else if (rows.Count < maxElements) rows.Add(row);
    }
}
