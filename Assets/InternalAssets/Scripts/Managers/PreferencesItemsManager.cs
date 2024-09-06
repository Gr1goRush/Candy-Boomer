using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public enum PreferenceItem
{
    Background, GameMusic, ExtraShootPlatformBoost, BombBoost, ExtraBulletsBoost
}

public enum PreferenceItemType
{
    Constant, OnNextRound
}

[System.Serializable]
public struct PreferenceItemData
{
    public PreferenceItem item;
    public PreferenceItemType type;
    public string fileNamePrefix, resourcesPath;
    public int variantsCount, price;

    [HideInInspector] public int selectedItemIndex;

    [HideInInspector] public bool[] variantsAvailableStates;

    public bool AvailableAny()
    {
        for (int i = 0; i < variantsAvailableStates.Length; i++)
        {
            if (variantsAvailableStates[i])
            {
                return true;
            }
        }

        return false;
    }

    public string GetSaveKey(int index)
    {
        return "PreferenceItem" + item.ToString() + "_" + index.ToString(); 
    }

    public string GetSelectedItemSaveKey()
    {
        return "PreferenceItem" + item.ToString() + "_Selected";
    }
}

public class PreferencesItemsManager : Singleton<PreferencesItemsManager>
{
    [SerializeField] private PreferenceItemData[] itemsData;

    private Dictionary<PreferenceItem, PreferenceItemData> itemsDictionary;

    public event Action<PreferenceItem> ItemChanged, ItemSelectionChanged;

    protected override void Awake()
    {
        base.Awake();

        itemsDictionary = new Dictionary<PreferenceItem, PreferenceItemData>();

        for (int i = 0; i < itemsData.Length; i++)
        {
            PreferenceItemData itemData = itemsData[i];

            bool[] itemsAvailableStates = new bool[itemData.variantsCount];
            for (int _index = 0; _index < itemsAvailableStates.Length; _index++)
            {
                itemsAvailableStates[_index] = (_index == 0 && itemData.type == PreferenceItemType.Constant) || SavesManager.Instance.GetInt(itemData.GetSaveKey(_index), 0) == 1;
            }

            itemData.variantsAvailableStates = itemsAvailableStates;

            if (itemData.type != PreferenceItemType.OnNextRound)
            {
                itemData.selectedItemIndex = SavesManager.Instance.GetInt(itemData.GetSelectedItemSaveKey(), 0);
            }
            else
            {
                itemData.selectedItemIndex = -1;
            }

            itemsDictionary.Add(itemData.item, itemData);
        }
    }

    public bool Buy(PreferenceItem item, int index)
    {
        PreferenceItemData itemData = itemsDictionary[item];

        if(itemData.type == PreferenceItemType.OnNextRound)
        {
            index = 0;
        }

    //    itemData.price = 1;

        if (!CoinsBalanceManager.Instance.HasCoins(itemData.price))
        {
            return false;
        }

        CoinsBalanceManager.Instance.SubtractCoins(itemData.price);

        bool[] itemsAvailableStates = itemData.variantsAvailableStates;
        itemsAvailableStates[index] = true;
        itemData.variantsAvailableStates = itemsAvailableStates;
        itemsDictionary[item] = itemData;

        SavesManager.Instance.SetInt(itemData.GetSaveKey(index), 1);

        ItemChanged?.Invoke(item);

        return true;
    }

    public void Select(PreferenceItem item, int index)
    {
        PreferenceItemData itemData = itemsDictionary[item];

        itemData.selectedItemIndex = index;
        itemsDictionary[item] = itemData;

        SavesManager.Instance.SetInt(itemData.GetSelectedItemSaveKey(), index);

        ItemChanged?.Invoke(item);
        ItemSelectionChanged?.Invoke(item);
    }

    public bool ItemAvailbaleAnyVariant(PreferenceItem item)
    {
        PreferenceItemData itemData = itemsDictionary[item];
        return itemData.AvailableAny();
    }

    public bool CanUseNextRoundItem(PreferenceItem item)
    {
        PreferenceItemData itemData = itemsDictionary[item];
        if (itemData.type != PreferenceItemType.OnNextRound || !itemData.AvailableAny())
        {
            return false;
        }

        return true;
    }

    public bool TryUseNextRoundItem(PreferenceItem item)
    {
        PreferenceItemData itemData = itemsDictionary[item];
        if (itemData.type != PreferenceItemType.OnNextRound || !itemData.AvailableAny())
        {
            return false;
        }

        itemData.variantsAvailableStates[0] = false;
        itemsDictionary[item] = itemData;

        SavesManager.Instance.SetInt(itemData.GetSaveKey(0), 0);

        return true;
    }

    public PreferenceItemData GetData(PreferenceItem item)
    {
        return itemsDictionary[item];
    }

    public T LoadObject<T>(PreferenceItemData itemData, int index) where T : UnityEngine.Object
    {
        string path = Path.Combine(itemData.resourcesPath, itemData.fileNamePrefix + index.ToString());
        return Resources.Load<T>(path);
    }

    public T LoadObject<T>(PreferenceItem item, int index) where T : UnityEngine.Object
    {
        PreferenceItemData itemData = GetData(item);
        return LoadObject<T>(itemData, index);
    }

    public T LoadSelected<T>(PreferenceItem item) where T : UnityEngine.Object
    {
        PreferenceItemData itemData = GetData(item);
        return LoadObject<T>(itemData, itemData.selectedItemIndex);
    }
}
