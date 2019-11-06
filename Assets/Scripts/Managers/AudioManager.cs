using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioSource _sfxAudioSource, _musicAudioSource;
    public AudioMixerGroup _musicGroup, _sfxGroup;

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
}