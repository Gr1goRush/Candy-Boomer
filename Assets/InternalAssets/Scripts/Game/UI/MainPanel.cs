using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText, coinsText;
    [SerializeField] private Button bombBoostButton;

    private void Start()
    {
        UpdateBombBoostState();
    }

    public void UpdateBombBoostState()
    {
        bombBoostButton.gameObject.SetActive(PreferencesItemsManager.Instance.ItemAvailbaleAnyVariant(PreferenceItem.BombBoost));
    }

    public void SetScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void SetCoins(int coins)
    {
        coinsText.text = coins.ToString();
    }

    public void Pause()
    {
        GameController.Instance.Pause();
    }

    public void BombBoost()
    {
        GameController.Instance.UseBombBoost();
    }
}
