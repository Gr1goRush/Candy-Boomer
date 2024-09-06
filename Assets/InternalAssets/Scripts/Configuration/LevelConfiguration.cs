using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct LevelRowItem 
{
    public LevelRowItemType type;
}

public enum LevelRowItemType
{
    Candy, Bomb
}

[System.Serializable]
public struct LevelRow
{
    public float appearInterval;
    public LevelRowItem[] items;
}

[CreateAssetMenu(menuName = "Game Data/Level", fileName = "Level", order = 2)]
public class LevelConfiguration : ScriptableObject
{
    public long threeStarsScore = 120;
    public LevelRow[] topRows, bottomRows;

//#if UNITY_EDITOR
//    [Header("Level Generator:")]
//    public int minBombCountInRow = 0;
//    public int maxBombCountInRow = 2;
//    public int minItemsCountInRow = 4, maxItemsCountInRow = 6;
//    public int rowsCount = 5;
//    public float minAppearInterval = 1, maxAppearInterval = 4;

//    [ContextMenu("Generate")]
//    private void GenerateLevel()
//    {
//        topRows = GenerateRows();
//        bottomRows = GenerateRows();

//        threeStarsScore = CalculateThreeScoreInRows(topRows) + CalculateThreeScoreInRows(bottomRows);
//    }

//    private LevelRow[] GenerateRows()
//    {
//        LevelRow[] rows = new LevelRow[rowsCount];
//        for (int rowIndex = 0; rowIndex < rows.Length; rowIndex++)
//        {
//            LevelRow levelRow = new LevelRow();
//            if (rowIndex == 0)
//            {
//                levelRow.appearInterval = 0f;
//            }
//            else
//            {
//                levelRow.appearInterval = GenerateRandom(minAppearInterval, maxAppearInterval);
//            }

//            int bombCount = Random.Range(minBombCountInRow, maxBombCountInRow + 1);
//            int itemsCount = Random.Range(minItemsCountInRow, maxItemsCountInRow + 1);

//            LevelRowItem[] items = new LevelRowItem[itemsCount];
//            for (int itemIndex = 0; itemIndex < itemsCount; itemIndex++)
//            {
//                LevelRowItem item = new LevelRowItem();
//                if(itemIndex < bombCount)
//                {
//                    item.type = LevelRowItemType.Bomb;
//                }
//                else
//                {
//                    item.type = LevelRowItemType.Candy;
//                }

//                items[itemIndex] = item;
//            }

//            System.Random random = new System.Random();
//            items = items.OrderBy(x => random.Next()).ToArray();

//            levelRow.items = items;

//            rows[rowIndex] = levelRow;
//        }

//        return rows;
//    }

//    private static float GenerateRandom(float min, float max)
//    {
//        return System.Convert.ToSingle(System.Math.Round(Random.Range(min, max), 1));
//    }

//    private int CalculateThreeScoreInRows(LevelRow[] rows)
//    {
//        int score = 0;

//        foreach (LevelRow row in rows)
//        {
//            foreach (LevelRowItem item in row.items)
//            {
//                if(item.type == LevelRowItemType.Candy)
//                {
//                    score += 1;
//                }
//                else
//                {
//                    score += 10;
//                }
//            }
//        }

//        return score;
//    }
//#endif
}
