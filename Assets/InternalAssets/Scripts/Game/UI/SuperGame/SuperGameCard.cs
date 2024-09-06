using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SuperGameCard : MonoBehaviour
{
    [SerializeField] private Image candyImage;
    [SerializeField] private Button _button;

    public void SetCandySprite(Sprite sprite)
    {
        candyImage.sprite = sprite;
    }

    public void AddClickListener(UnityAction action)
    {
        _button.onClick.AddListener(action);
    }

    public void SetInteractable(bool interactable)
    {
        _button.interactable = interactable;
    }

    public void SetDefault()
    {
        candyImage.gameObject.SetActive(false);
    }

    public void ShowCandy()
    {
        candyImage.gameObject.SetActive(true);
    }
}
