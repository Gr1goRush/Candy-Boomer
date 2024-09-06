using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMusicChanger : PreferenceObjectChanger<AudioClip>
{
    [SerializeField] private AudioSource _audioSource;

    protected override void Set(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}

