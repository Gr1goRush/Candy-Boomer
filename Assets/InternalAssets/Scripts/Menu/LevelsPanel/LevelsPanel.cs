using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsPanel : MonoBehaviour
{
    [SerializeField] private LevelPanelItem[] items;

    void Start()
    {
        int levelsCount = LevelsManager.Instance.LevelsCount;
        for (int i = 0; i < levelsCount; i++)
        {
            LevelState levelState = LevelsManager.Instance.GetLevelState(i);

            LevelPanelItem levelPanelItem = items[i];
            levelPanelItem.Set(levelState);

            int levelIndex = i;
            levelPanelItem.SetClickListener(() => SelectLevel(levelIndex));
        }
    }

    private void SelectLevel(int levelIndex)
    {
        LevelsManager.Instance.SelectLevel(levelIndex);
        MenuController.Instance.LoadGame();
    }
}
