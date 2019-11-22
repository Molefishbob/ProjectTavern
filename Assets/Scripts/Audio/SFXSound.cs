using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

public class SFXSound : MonoBehaviour
{
    private ScaledOneShotTimer _timer;
    private AudioSource _audio;

    void Awake()
    {
        _timer = gameObject.AddComponent<ScaledOneShotTimer>();
        _audio = GetComponent<AudioSource>();
        _audio.outputAudioMixerGroup = AudioManager.Instance._sfxGroup;
        _timer.OnTimerCompleted += CommitSudoku;
    }

    private void OnDestroy()
    {
        _timer.OnTimerCompleted -= CommitSudoku;
    }

    public void PlaySFXSound(AudioClip clip, bool loop = false)
    {
        _audio.clip = clip;
        _audio.loop = loop;
        _timer.StartTimer(clip.length + 0.1f);
        _audio.Play();
    }

    private void CommitSudoku()
    {
        _audio.clip = null;
        _audio.loop = false;
        if (_audio.isPlaying)
            _audio.Stop();
        gameObject.SetActive(false);
    }
}
