using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

public class MenuAudioMiddleHand : MonoBehaviour
{
    public AudioClip _audioClip;
   public void ClickSound()
    {
        AudioManager.Instance.PlaySound(_audioClip);
    }
}
