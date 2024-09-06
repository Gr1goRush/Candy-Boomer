using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinPanel : MonoBehaviour
{
    [SerializeField] private StarsFrame starsFrame;
    [SerializeField] private TextMeshProUGUI scoreText, coinsText;
    [SerializeField] private TMP_InputField playerNameField;

    public void Show(int starsCount, int score, int coins)
    {
        starsFrame.Set(starsCount);
        scoreText.text = score.ToString();
        coinsText.text = coins.ToString();

        gameObject.SetActive(true);
    }

    public void Restart()
    {
        AddScoreRecord();

        GameScenesManager.Instance.LoadGame();
    }

    public void Levels()
    {
        AddScoreRecord();

        GameScenesManager.Instance.LoadMenu("levels");
    }

    private void AddScoreRecord()
    {
        string playerName = playerNameField.text;
        if(!string.IsNullOrWhiteSpace(playerName))
        {
            GameController.Instance.AddScoreRecord(playerName);
        }
    }
}
