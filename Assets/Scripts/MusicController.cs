using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicController : MonoBehaviour
{
    public AudioClip introMusic;
    public AudioClip gameMusic;
    public AudioSource musicSource;
    public AudioMixer audioMixer;

    private float mainVolumeLevel = 1;
    private float musicVolumeLevel = 1;
    private float gameEffectsVolumeLevel = 1;

    // Start is called before the first frame update
    void Start()
    {
        musicSource.ignoreListenerPause = true;
    }

    public void SetGameMusic()
    {
        Debug.Log("CAMBIAR A MÚSICA DE JUEGO");
        musicSource.clip = gameMusic;
        musicSource.Play();
    }

    public void SetIntroMusic()
    {
        musicSource.clip = introMusic;
        musicSource.Play();
    }

    public void SetVolume(string volumeParam, float sliderValue)
    {
        audioMixer.SetFloat(volumeParam, Mathf.Log10(sliderValue) * 20);
        if (volumeParam.Equals(AudioMixerParams.MAIN_VOLUME)) mainVolumeLevel = sliderValue;
        if (volumeParam.Equals(AudioMixerParams.MUSIC_VOLUME)) musicVolumeLevel = sliderValue;
        if (volumeParam.Equals(AudioMixerParams.GAME_EFFECTS_VOLUME)) gameEffectsVolumeLevel = sliderValue;
    }

    public void SetMainVolume(float sliderValue)
    {
        mainVolumeLevel = sliderValue;
        SetVolume(AudioMixerParams.MAIN_VOLUME, mainVolumeLevel);
    }

    public void SetMusicVolume(float sliderValue)
    {
        musicVolumeLevel = sliderValue;
        SetVolume(AudioMixerParams.MUSIC_VOLUME, musicVolumeLevel);
    }

    public void SetGameEffectsVolume(float sliderValue)
    {
        gameEffectsVolumeLevel = sliderValue;
        SetVolume(AudioMixerParams.GAME_EFFECTS_VOLUME, gameEffectsVolumeLevel);
    }

    internal float GetVolumeLevel(string volumeType)
    {
        if (volumeType.Equals(AudioMixerParams.MAIN_VOLUME)) return mainVolumeLevel;
        if (volumeType.Equals(AudioMixerParams.MUSIC_VOLUME)) return musicVolumeLevel;
        if (volumeType.Equals(AudioMixerParams.GAME_EFFECTS_VOLUME)) return gameEffectsVolumeLevel;
        return 1;
    }
}
