using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreferenceObjectChanger<T> : MonoBehaviour where T : UnityEngine.Object
{
    [SerializeField] private PreferenceItem item;

    void Start()
    {
        Set();

        PreferencesItemsManager.Instance.ItemSelectionChanged += ItemSelectionChanged;
    }

    private void ItemSelectionChanged(PreferenceItem _item)
    {
        if (_item == item)
        {
            Set();
        }
    }

    private void Set()
    {
        Set(PreferencesItemsManager.Instance.LoadSelected<T>(item));
    }

    protected virtual void Set(T obj)
    {

    }

    private void OnDestroy()
    {
        if (PreferencesItemsManager.Instance != null)
        {
            PreferencesItemsManager.Instance.ItemSelectionChanged -= ItemSelectionChanged;
        }
    }
}
