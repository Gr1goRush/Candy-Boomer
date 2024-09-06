using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarsFrame : MonoBehaviour
{
    [SerializeField] private Sprite fillSprite, emptySprite;

    [SerializeField] private Image[] items;

    public void Set(int count)
    {
        for (int i = 0; i < count; i++)
        {
            items[i].sprite = count > i ? fillSprite : emptySprite;
        }
    }
}
