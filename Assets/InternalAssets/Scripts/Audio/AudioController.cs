using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AudioController : Singleton<AudioController>
{
    public SoundsController Sounds => soundsController;
    public bool SoundsMuted { get; private set; }
    public bool MusicMuted { get; private set; }

    [SerializeField] private AudioSource soundsSource, musicSource;
    [SerializeField] private SoundsController soundsController;

    private AudioClip defaultMusicClip;

    private Coroutine playingMusicTimerCoroutine = null;

    protected override void Awake()
    {
        base.Awake();

        SoundsMuted = PlayerPrefs.GetInt("MuteSounds", 0) == 1;
        soundsSource.mute = SoundsMuted;

        MusicMuted = PlayerPrefs.GetInt("MuteMusic", 0) == 1;
        musicSource.mute = MusicMuted;

        defaultMusicClip = musicSource.clip;
    }

    public void SetSoundsMuted(bool v)
    {
        SoundsMuted = v;
        soundsSource.mute = v;
        PlayerPrefs.SetInt("MuteSounds", v ? 1 : 0);
    }

    public void SetMusicMuted(bool v)
    {
        MusicMuted = v;
        musicSource.mute = v;
        PlayerPrefs.SetInt("MuteMusic", v ? 1 : 0);
    }

    public void PlayOneShot(AudioClip clip, float volume)
    {
        soundsSource.PlayOneShot(clip, volume);
    }

    public void PlayMusicTimer(AudioClip clip, float time)
    {
        if (playingMusicTimerCoroutine != null)
        {
            StopCoroutine(playingMusicTimerCoroutine);
            playingMusicTimerCoroutine = null;
        }

        playingMusicTimerCoroutine = StartCoroutine(PlayMusicTimerCoroutine(clip, time));
    }

    private IEnumerator PlayMusicTimerCoroutine(AudioClip clip, float time)
    {
        musicSource.clip = clip;
        musicSource.Play();

        yield return new WaitForSecondsRealtime(time);

        musicSource.clip = defaultMusicClip;
        musicSource.Play();
    }
}