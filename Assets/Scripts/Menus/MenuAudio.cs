using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudio : MonoBehaviour
{
    private AudioSource _audioSource;
    public AudioClip _clickSound, _menuMusic;

    private void Awake()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void ClickSound()
    {
        _audioSource.PlayOneShot(_clickSound);
    }
}
