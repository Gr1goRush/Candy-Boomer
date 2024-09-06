using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    private int score = 0;
    private int coins = 0;
    public bool Paused { get; private set; }
    public bool GameCompleted { get; private set; }

    public Playground Playground => playground;
    [SerializeField] private Playground playground;

    [SerializeField] private GameUIContainer UIContainer;

    private long threeStarsScore = 0;

    void Start()
    {
        Paused = false;
        GameCompleted = false;

        score = 0;
        UpdateUIScore();

        coins = 0;
        UpdateUICoins();

        LevelConfiguration levelConfiguration = LevelsManager.Instance.LoadSelectedLevelConfiguration();

        Playground.AttackGrid.StartAttack(levelConfiguration.topRows, levelConfiguration.bottomRows);

      //  startTime = DateTimeOffset.Now.ToUnixTimeSeconds();
        threeStarsScore = levelConfiguration.threeStarsScore;
    }

    public void AddScore(int amount = 1)
    {
        score += amount;
        UpdateUIScore();
    }

    private void UpdateUIScore()
    {
        UIContainer.MainPanel.SetScore(score);
    }

    public void AddCoin()
    {
        coins++;
        UpdateUICoins();
    }

    private void UpdateUICoins()
    {
        UIContainer.MainPanel.SetCoins(coins);
    }

    public void UseBombBoost()
    {
        if (!PreferencesItemsManager.Instance.TryUseNextRoundItem(PreferenceItem.BombBoost))
        {
            return;
        }

        AudioController.Instance.Sounds.PlayOneShot("bomb_boost");

        float rate = UnityEngine.Random.Range(0.6f, 0.8f);
        Playground.AttackGrid.ExplodeRandomRate(rate);

        InputController.Instance.SkipNextTouch();

        UIContainer.MainPanel.UpdateBombBoostState();
    }

    private void SetPaused(bool paused)
    {
        Time.timeScale = paused ? 0f : 1f;
        Paused = paused;

        if (!Paused)
        {
            InputController.Instance.SkipNextTouch();
        }
    }

    public void Pause()
    {
        SetPaused(true);
        UIContainer.PausePanel.Show();
    }

    public void UnPause()
    {
        UIContainer.PausePanel.Hide();
        SetPaused(false);

    }

    public void Lose()
    {
        Complete();
        UIContainer.ShowLosePanel();

        AudioController.Instance.Sounds.PlayOneShot("lose");
    }

    public void Win()
    {
        if(GameCompleted)
        {
            return;
        }

      //  long completeTime = DateTimeOffset.Now.ToUnixTimeSeconds();
      //  long gameTime = completeTime - startTime;
        int starsCount = 0;
        if(score > 0)
        {
            if(score >= (threeStarsScore * 0.9d))
            {
                starsCount = 3;
            }
            else if(score >= (threeStarsScore * 0.6d))
            {
                starsCount = 2;
            }
            else if(score >= (threeStarsScore * 0.3d))
            {
                starsCount = 1;
            }
        }

        CoinsBalanceManager.Instance.AddCoins(coins);
        LevelsManager.Instance.CompleteLevel(starsCount);

        Complete();

        UIContainer.ShowWinPanel(starsCount, score, coins);

        AudioController.Instance.Sounds.PlayOneShot("win");

        PreferencesItemsManager.Instance.TryUseNextRoundItem(PreferenceItem.ExtraShootPlatformBoost);
        PreferencesItemsManager.Instance.TryUseNextRoundItem(PreferenceItem.ExtraBulletsBoost);
    }

    private void Complete()
    {
        GameCompleted = true;
        Time.timeScale = 0f;

        Playground.Hide();
    }

    public void SuperGame()
    {
        AudioController.Instance.Sounds.PlayOneShot("super_game");

        SetPaused(true);
        UIContainer.SuperGamePanel.Show();
    }

    public void SuperGameCompleted(bool success)
    {
        SetPaused(false);

        if(success)
        {
            LevelsManager.Instance.UnlockNextLevel();

            AudioController.Instance.Sounds.PlayOneShot("super_game_win");
        }
        else
        {
            GameScenesManager.Instance.LoadGame();
        }
    }

    public void SuperGameCanceled()
    {
        SetPaused(false);
    }

    public void AddScoreRecord(string playerName)
    {
        LeaderboardsManager.Instance.AddRecord(playerName, score);
    }
}
