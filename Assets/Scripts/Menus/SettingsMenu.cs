using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer _audioMixer;
    public Slider _masterSlider, _sfxSlider, _musicSlider;
    public Toggle _masterMute, _sfxMute, _musicMute;
    private const string FILENAME = "Settings.json";

    private void Awake()
    {
        SerializationManager.LoadSettings(FILENAME);
        _masterSlider.value = SerializationManager.LoadedSettings.Volume.Master;
    }

    //TODO: serialize current settings data
    public void CloseSettingsMenu()
    {
        gameObject.SetActive(false);
        //TODO: Move to gamemanager or make events
        SerializationManager.SaveSettings(FILENAME);
    }

    public void MasterVolumeChange()
    {
        float sliderValue = _masterSlider.value;
        _audioMixer.SetFloat("masterVol", Mathf.Log10(sliderValue) * 20);
        SerializationManager.LoadedSettings.Volume.Master = _masterSlider.value;
    }

    public void SFXVolumeChange()
    {
        float sliderValue = _sfxSlider.value;
        _audioMixer.SetFloat("sfxVol", Mathf.Log10(sliderValue) * 20);
    }

    public void MusicVolumeChange()
    {
        float sliderValue = _musicSlider.value;
        _audioMixer.SetFloat("musicVol", Mathf.Log10(sliderValue) * 20);
    }

    public void MuteMusic()
    {
        bool toggleValue = _musicMute.isOn;
        _audioMixer.SetFloat("musicVol", -80f);
    }

    public void MuteMaster()
    {
        bool toggleValue = _masterMute.isOn;
        _audioMixer.SetFloat("masterVol", -80f);
    }

    public void MuteSFX()
    {
        bool toggleValue = _sfxMute.isOn;
        _audioMixer.SetFloat("sfxVol", -80f);
    }

}
