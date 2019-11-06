using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudio : MonoBehaviour
{
    public AudioSource _musicAudioSource, _clickAudioSource;
    //TODO: make unified audiomanager

    public void ClickSound()
    {
        _clickAudioSource.Play();
    }
}
