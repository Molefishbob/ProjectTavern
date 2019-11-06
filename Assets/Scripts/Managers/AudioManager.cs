using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioSource _sfxAudioSource, _musicAudioSource;
    public AudioMixerGroup _musicGroup, _sfxGroup;
    public AudioMixer _mixer;


    private void Start()
    {
        SerializationManager.LoadSettings("Settings");
        SerializationManager.SettingsData settings = SerializationManager.LoadedSettings;

        _mixer.SetFloat("masterVol", settings.Volume.Master);
        _mixer.SetFloat("musicVol", settings.Volume.Music);
        _mixer.SetFloat("sfxVol", settings.Volume.SFX);
        if (settings.Volume.MasterMute)
            _mixer.SetFloat("masterVol", -80f);
        if (settings.Volume.MusicMute)
            _mixer.SetFloat("musicVol", -80f);
        if (settings.Volume.SFXMute)
            _mixer.SetFloat("sfxVol", -80f);
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

    public void StopMusic()
    {
        _musicAudioSource.Stop();
    }
}