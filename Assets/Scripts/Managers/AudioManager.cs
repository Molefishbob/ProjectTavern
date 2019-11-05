using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource _menuMusicAudioSource, _clickAudioSource, _gameMusic;

    public void ClickSound()
    {
        _clickAudioSource.Play();
    }
}
