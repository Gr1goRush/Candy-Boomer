using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardsPanelItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText, scoreText;

    public void Set(LeaderboardsRecord leaderboardsRecord)
    {
        nameText.text = leaderboardsRecord.name;
        scoreText.text = leaderboardsRecord.score.ToString();
    }
}
