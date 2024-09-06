using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class CustomToggle : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _textMesh;

    [SerializeField] private Sprite onSprite, offSprite;
    [SerializeField] private string onText, offText;

    bool isOn;
    public bool IsOn
    {
        get { return isOn; }
        set { isOn = value; SetValueWithoutNotify(isOn); }
    }

    public UnityEvent<bool> onSwitched;

    public void SetValueWithoutNotify(bool v)
    {
        if (v)
        {
            _image.sprite = onSprite;
            _textMesh.text = onText;
        }
        else
        {
            _image.sprite = offSprite;
            _textMesh.text = offText;
        }

    }

    void ChangeValue()
    {
        IsOn = !IsOn;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ChangeValue();
        onSwitched?.Invoke(isOn);
    }

    private void Reset()
    {
        _image = GetComponent<Image>();
    }
}
