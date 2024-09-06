using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StorePanelItem : MonoBehaviour
{
    [SerializeField] private PreferenceItem item;
    [SerializeField] private int variantIndex = -1;

    [SerializeField] private GameObject pricePanel, bottomPanel;
    [SerializeField] private TextMeshProUGUI priceText, stateText;
    [SerializeField] private Button _button;

    private UnityAction clickAction;

    private void Start()
    {
        UpdateState();

        _button.onClick.AddListener(ClickButton);

        PreferencesItemsManager.Instance.ItemChanged += PreferenceItemChanged;
    }

    private void PreferenceItemChanged(PreferenceItem changedItem)
    {
        if(item == changedItem)
        {
            UpdateState();
        }
    }

    private void UpdateState()
    {
        SetClickListener(null);

        PreferenceItemData itemData = PreferencesItemsManager.Instance.GetData(item);
        if(itemData.type == PreferenceItemType.OnNextRound)
        {
            if (itemData.AvailableAny())
            {
                bottomPanel.SetActive(false);
            }
            else
            {
                SetPrice(itemData.price);
                bottomPanel.SetActive(true);

                SetClickListener(Buy);
            }

            return;
        }

        bottomPanel.SetActive(true);

        if (!itemData.variantsAvailableStates[variantIndex])
        {
            SetPrice(itemData.price);
            SetClickListener(Buy);
        }
        else
        {
            if(itemData.selectedItemIndex == variantIndex)
            {
                SetState(true);
                SetClickListener(null);
            }
            else
            {
                SetState(false);
                SetClickListener(Select);
            }
        }
    }

    private void SetPrice(int price)
    {
        priceText.text = price.ToString();
        pricePanel.SetActive(true);
       stateText.gameObject.SetActive(false);
    }

    private void SetState(bool selected)
    {
        stateText.text = selected ? "Selected" : "Select";
        pricePanel.SetActive(false);
        stateText.gameObject.SetActive(true);
    }

    private void SetClickListener(UnityAction action)
    {
        clickAction = action;
        _button.interactable = clickAction != null;
    }

    private void ClickButton()
    {
        clickAction?.Invoke();
    }

    private void Buy()
    {
        if(PreferencesItemsManager.Instance.Buy(item, variantIndex))
        {
            AudioController.Instance.Sounds.PlayOneShot("buy");
        }
    }

    private void Select()
    {
        PreferencesItemsManager.Instance.Select(item, variantIndex);
    }

    private void OnDestroy()
    {
        if (PreferencesItemsManager.Instance != null)
        {
            PreferencesItemsManager.Instance.ItemChanged -= PreferenceItemChanged;
        }
    }
}
