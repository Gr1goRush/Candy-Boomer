using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelPanelItem : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private StarsFrame starsFrame;

    private UnityAction clickAction;

    private void Awake()
    {
        _button.onClick.AddListener(ClickButton);
    }

    public void Set(LevelState levelState)
    {
        _button.interactable = levelState.unlocked;
        starsFrame.Set(levelState.starsCount);
    }

    public void SetClickListener(UnityAction unityAction) 
    {
        clickAction = unityAction;
    }

    private void ClickButton()
    {
        clickAction?.Invoke();
    }
}
