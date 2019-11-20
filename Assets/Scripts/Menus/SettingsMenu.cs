using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using Managers;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer _audioMixer;
    public Slider _masterSlider, _sfxSlider, _musicSlider;
    public Toggle _masterMute, _sfxMute, _musicMute;
    private const string FILENAME = "Settings";

    private void OnEnable()
    {
        SerializationManager.LoadSettings(FILENAME);
        _masterSlider.value = SerializationManager.LoadedSettings.Volume.Master;
        _sfxSlider.value = SerializationManager.LoadedSettings.Volume.SFX;
        _musicSlider.value = SerializationManager.LoadedSettings.Volume.Music;
    }

    //TODO: serialize current settings data
    public void CloseSettingsMenu()
    {
        gameObject.SetActive(false);
        GameManager.Instance.SaveData();
    }

    public void MasterVolumeChange()
    {
        float sliderValue = _masterSlider.value;
        if (!_masterMute.isOn)
            _audioMixer.SetFloat("masterVol", Mathf.Log10(sliderValue) * 20);
        SerializationManager.LoadedSettings.Volume.Master = _masterSlider.value;
    }

    public void SFXVolumeChange()
    {
        float sliderValue = _sfxSlider.value;
        if (!_sfxMute.isOn)
            _audioMixer.SetFloat("sfxVol", Mathf.Log10(sliderValue) * 20);
        SerializationManager.LoadedSettings.Volume.SFX = _sfxSlider.value;
    }

    public void MusicVolumeChange()
    {
        float sliderValue = _musicSlider.value;
        if (!_musicMute.isOn)
            _audioMixer.SetFloat("musicVol", Mathf.Log10(sliderValue) * 20);

        SerializationManager.LoadedSettings.Volume.Music = _musicSlider.value;
    }

    public void MuteMusic()
    {
        bool toggleValue = _musicMute.isOn;
        SerializationManager.LoadedSettings.Volume.MusicMute = toggleValue;
        if (toggleValue == true)
        {
            _audioMixer.SetFloat("musicVol", -80f);
        }
        else
        {
            _audioMixer.SetFloat("musicVol", Mathf.Log10(SerializationManager.LoadedSettings.Volume.Music) * 20);
        }
    }

    public void MuteMaster()
    {
        bool toggleValue = _masterMute.isOn;
        SerializationManager.LoadedSettings.Volume.MasterMute = toggleValue;
        if (toggleValue == true)
        {
            _audioMixer.SetFloat("masterVol", -80f);
        }
        else
        {
            _audioMixer.SetFloat("masterVol", Mathf.Log10(SerializationManager.LoadedSettings.Volume.Master) * 20);
        }
    }

    public void MuteSFX()
    {
        bool toggleValue = _sfxMute.isOn;
        SerializationManager.LoadedSettings.Volume.SFXMute = toggleValue;
        if (toggleValue == true)
        {
            _audioMixer.SetFloat("sfxVol", -80f);
        }
        else
        {
            _audioMixer.SetFloat("sfxVol", Mathf.Log10(SerializationManager.LoadedSettings.Volume.SFX) * 20);
        }
    }

}
