using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct LeaderboardsRecord
{
    public string name;
    public int score;
}

public class LeaderboardsManager : Singleton<LeaderboardsManager>
{

    public void AddRecord(string playerName, int score)
    {
        XmlData leaderboardsData = GetLeaderboardsData();
        XmlData itemData = leaderboardsData.Create("item");
        itemData.SetString("name", playerName);
        itemData.SetInt("score", score);

        SavesManager.Instance.Save();
    }

    public LeaderboardsRecord[] GetRecords()
    {
        XmlData leaderboardsData = GetLeaderboardsData();
        XmlData[] items = leaderboardsData.GetArray();

        LeaderboardsRecord[] records = new LeaderboardsRecord[items.Length];
        for (int i = 0; i < records.Length; i++)
        {
            XmlData xmlData = items[i];
            records[i] = new LeaderboardsRecord
            {
                name = xmlData.GetString("name"),
                score = xmlData.GetInt("score")
            };
        }

        return records.OrderByDescending(x => x.score).ToArray();
    }

    XmlData GetLeaderboardsData()
    {
        return SavesManager.Instance.FindOrCreate("leaderboards");
    }
}