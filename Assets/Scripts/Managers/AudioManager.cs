using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Managers;

public class AudioManager : MonoBehaviour
{
    public AudioSource _sfxAudioSource, _musicAudioSource;
    public AudioMixerGroup _musicGroup, _sfxGroup;
    public AudioMixer _mixer;

    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SerializationManager.LoadSettings("Settings");
        SerializationManager.SettingsData settings = SerializationManager.LoadedSettings;

        SetMixerValue("musicVol", settings.Volume.Music);
        SetMixerValue("masterVol", settings.Volume.Master);
        SetMixerValue("sfxVol", settings.Volume.SFX);

        if (settings.Volume.MasterMute)
            _mixer.SetFloat("masterVol", -80f);
        if (settings.Volume.MusicMute)
            _mixer.SetFloat("musicVol", -80f);
        if (settings.Volume.SFXMute)
            _mixer.SetFloat("sfxVol", -80f);

        float a = 0;
        print(_mixer.GetFloat("musicVol", out a));
        print(a);

        GameManager.Instance.PlayMenuMusic();
    }
    public void PlaySound(AudioClip clip)
    {
        _sfxAudioSource.clip = clip;
        _sfxAudioSource.outputAudioMixerGroup = _sfxGroup;
        _sfxAudioSource.Play();
    }

    public void PlayMusic(AudioClip song)
    {
        _musicAudioSource.clip = song;
        _musicAudioSource.outputAudioMixerGroup = _musicGroup;
        _musicAudioSource.Play();
    }

    public void SetMixerValue(string mixerGroup, float value)
    {
        _mixer.SetFloat(mixerGroup, Mathf.Log10(value) * 20);
    }

    public void StopMusic()
    {
        _musicAudioSource.Stop();
    }
}