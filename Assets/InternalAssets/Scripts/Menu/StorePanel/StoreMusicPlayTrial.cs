using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreMusicPlayTrial : MonoBehaviour
{
    [SerializeField] public int variantIndex;

    [SerializeField] public Button _button;

    private AudioClip musicClip;

    void Start()
    {
        musicClip = PreferencesItemsManager.Instance.LoadObject<AudioClip>(PreferenceItem.GameMusic, variantIndex);

        _button.onClick.AddListener(Play);
    }

    private void Play()
    {
        AudioController.Instance.PlayMusicTimer(musicClip, 5f);
    }

    private void Reset()
    {
        _button = GetComponent<Button>();
    }
}
