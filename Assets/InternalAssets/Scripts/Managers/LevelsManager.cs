using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct LevelState
{
    public int starsCount;
    public bool unlocked;
}

public class LevelsManager : Singleton<LevelsManager>
{
    public int LevelsCount => levelsList.levelsCount;

    public int SelectedLevelIndex { get; private set; }

    [SerializeField] private LevelsList levelsList;

    private LevelState[] levelStates;

    protected override void Awake()
    {
        base.Awake();

        SelectedLevelIndex = 0;

        levelStates = new LevelState[levelsList.levelsCount];
        for (int i = 0; i < levelStates.Length; i++)
        {
            LevelState levelState = new();
            levelState.unlocked = i == 0 || SavesManager.Instance.GetInt("LevelUnlocked_" + i.ToString(), 0) == 1;

            if (levelState.unlocked)
            {
                levelState.starsCount = SavesManager.Instance.GetInt("LevelStars_" + i.ToString(), 0);
                SelectedLevelIndex = i;
            }
            else
            {
                levelState.starsCount = 0;
            }

            levelStates[i] = levelState;
        }
    }

    public LevelConfiguration LoadSelectedLevelConfiguration()
    {
        return Resources.Load<LevelConfiguration>("Levels/Level" + SelectedLevelIndex.ToString());
    }

    public LevelState GetLevelState(int levelIndex)
    {
        return levelStates[levelIndex];
    }

    public void CompleteLevel(int starsCount)
    {
        LevelState levelState = levelStates[SelectedLevelIndex];
        if (levelState.starsCount < starsCount)
        {
            levelState.starsCount = starsCount;
            levelStates[SelectedLevelIndex] = levelState;

            SavesManager.Instance.SetInt("LevelStars_" + SelectedLevelIndex.ToString(), 1);
        }
    }

    public void UnlockNextLevel()
    {
        for (int levelIndex = 0; levelIndex < levelStates.Length; levelIndex++)
        {
            LevelState levelState = levelStates[levelIndex];

            if (!levelState.unlocked)
            {
                levelState.unlocked = true;
                levelStates[levelIndex] = levelState;

                SavesManager.Instance.SetInt("LevelUnlocked_" + levelIndex.ToString(), 1);

                return;
            }
        }
    }

    //public bool TryBuyLevel(int levelIndex)
    //{
    //    if (BalanceManager.Instance.HasBalance(LevelPrice))
    //    {
    //        BalanceManager.Instance.SubtractBalance(LevelPrice);
    //        UnlockLevel(levelIndex);

    //        return true;
    //    }

    //    return false;
    //}

    public void SelectLevel(int levelIndex)
    {
        SelectedLevelIndex = levelIndex;
    }
}
