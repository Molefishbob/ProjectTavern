using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using Managers;
using UnityEngine.InputSystem.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer _audioMixer;
    public Slider _masterSlider, _sfxSlider, _musicSlider;
    public Toggle _masterMute, _sfxMute, _musicMute;
    private const string FILENAME = "Settings";
    private Controls inputActions;
    private Vector2 _lastPos = Vector2.zero;
    private bool mouseControl = false;
    private bool _sliderMode = false;
    [SerializeField]
    private Button[] _sliderButtons = null;
    private Button _backButton = null;

    private void OnEnable()
    {
        SerializationManager.LoadSettings(FILENAME);
        _masterSlider.value = SerializationManager.LoadedSettings.Volume.Master;
        _sfxSlider.value = SerializationManager.LoadedSettings.Volume.SFX;
        _musicSlider.value = SerializationManager.LoadedSettings.Volume.Music;
        _masterMute.isOn = SerializationManager.LoadedSettings.Volume.MasterMute;
        _sfxMute.isOn = SerializationManager.LoadedSettings.Volume.SFXMute;
        _musicMute.isOn = SerializationManager.LoadedSettings.Volume.MusicMute;

        _backButton = transform.GetChild(0).GetChild(1).gameObject.GetComponent<Button>();
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(_backButton.gameObject);

        inputActions = new Controls();
        inputActions.UI.Point.performed += SwitchToMouse;
        inputActions.UI.Navigate.performed += SwitchToController;
        inputActions.UI.Submit.performed += SliderSwitch;
        inputActions.UI.Point.Enable();
        inputActions.UI.Navigate.Enable();
        inputActions.UI.Submit.Enable();

        mouseControl = false;

        _masterSlider.interactable = false;
        _sfxSlider.interactable = false;
        _musicSlider.interactable = false;

        for (int i = 0; i < _sliderButtons.Length; i++)
        {
            _sliderButtons[i].interactable = true;
        }

    }

    private void OnDisable()
    {
        inputActions.UI.Point.Disable();
        inputActions.UI.Navigate.Disable();
        inputActions.UI.Submit.Disable();
        inputActions.UI.Point.performed -= SwitchToMouse;
        inputActions.UI.Navigate.performed -= SwitchToController;
        inputActions.UI.Submit.performed -= SliderSwitch;
        inputActions = null;
    }

    private void SwitchToMouse(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Vector2 pos = obj.ReadValue<Vector2>();
        if (!mouseControl && Vector2.Distance(pos, _lastPos) > 100)
        {
            Debug.Log("Changing to mouse control.");
            EnableAccordingToCurrentcontrol();
            mouseControl = true;
            _sliderMode = false;
            _lastPos = pos;

            _masterSlider.interactable = true;
            _sfxSlider.interactable = true;
            _musicSlider.interactable = true;

            for (int i = 0; i < _sliderButtons.Length; i++)
            {
                _sliderButtons[i].interactable = false;
            }
        }
    }

    private void SwitchToController(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (mouseControl)
        {
            Debug.Log("Changing to controller control.");
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(_backButton.gameObject);
            mouseControl = false;

            _masterSlider.interactable = false;
            _sfxSlider.interactable = false;
            _musicSlider.interactable = false;

            for (int i = 0; i < _sliderButtons.Length; i++)
            {
                _sliderButtons[i].interactable = true;
            }
        }
    }

    private void SliderSwitch(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (_sliderMode)
        {
            _sliderMode = false;
            SwitchFromSliderToButton();
            Invoke("EnableAccordingToCurrentcontrol", 0.1f);
            // EnableAccordingToCurrentcontrol();
        }
    }

    public void CloseSettingsMenu()
    {
        GameManager.Instance.SaveData();
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0)
            MainMenu.Menu.GotoMainMenu();
        else
        {
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(PauseMenu.Menu.DefaultSelection);
            gameObject.SetActive(false);
        }
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

    public void ActivateSlider(int indexFromUp)
    {
        DisableInteractables();
        _sliderMode = true;
        switch (indexFromUp)
        {
            case 1:
                _masterSlider.interactable = true;
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(_masterSlider.gameObject);
                break;
            case 2:
                _sfxSlider.interactable = true;
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(_sfxSlider.gameObject);
                break;
            case 3:
                _musicSlider.interactable = true;
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(_musicSlider.gameObject);
                break;
            default:
                break;
        }
    }

    private void DisableInteractables()
    {
        _backButton.interactable = false;
        _masterSlider.interactable = false;
        _sfxSlider.interactable = false;
        _musicSlider.interactable = false;
        _masterMute.interactable = false;
        _sfxMute.interactable = false;
        _musicMute.interactable = false;

        for (int i = 0; i < _sliderButtons.Length; i++)
        {
            _sliderButtons[i].interactable = false;
        }
    }

    private void SwitchFromSliderToButton()
    {
        GameObject currentSelected = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

        if (currentSelected == _masterSlider.gameObject)
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(_masterSlider.handleRect.gameObject);
        else if (currentSelected == _sfxSlider.gameObject)
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(_sfxSlider.handleRect.gameObject);
        else if (currentSelected == _musicSlider.gameObject)
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(_musicSlider.handleRect.gameObject);
        else
        {
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(_backButton.gameObject);
        }
    }

    private void EnableAccordingToCurrentcontrol()
    {
        _backButton.interactable = true;
        _masterMute.interactable = true;
        _sfxMute.interactable = true;
        _musicMute.interactable = true;

        if (mouseControl)
        {
            _masterSlider.interactable = true;
            _sfxSlider.interactable = true;
            _musicSlider.interactable = true;

            for (int i = 0; i < _sliderButtons.Length; i++)
            {
                _sliderButtons[i].interactable = false;
            }
        }
        else
        {
            _masterSlider.interactable = false;
            _sfxSlider.interactable = false;
            _musicSlider.interactable = false;

            for (int i = 0; i < _sliderButtons.Length; i++)
            {
                _sliderButtons[i].interactable = true;
            }
        }
    }
}
