using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Managers;

public class MainMenu : MonoBehaviour
{
    public GameObject _settingsMenu;
    public GameObject _quitMenu;
    public GameObject _saveSelect;
    public GameObject _characterSelect;

    public GameObject _settingsMenuDefaultBind;
    public GameObject _mainMenuDefaultBind;
    public GameObject _quitMenuDefaultBind;
    public GameObject _saveMenuDefaultBind;

    private GameObject _nextSelection = null;

    public static MainMenu Menu;

    private void OnEnable()
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(_mainMenuDefaultBind);
        Menu = this;
    }

    public void OpenSettingsMenu()
    {
        _settingsMenu.SetActive(true);
        _saveSelect.SetActive(false);
        _quitMenu.SetActive(false);
        _characterSelect.SetActive(false);
        gameObject.SetActive(false);
        _nextSelection = _settingsMenuDefaultBind;

        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(_nextSelection);
    }

    public void OpenQuitMenu()
    {
        _settingsMenu.SetActive(false);
        _saveSelect.SetActive(false);
        _quitMenu.SetActive(true);
        _characterSelect.SetActive(false);
        _nextSelection = _quitMenuDefaultBind;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<UnityEngine.UI.Button>().interactable = false;
        }

        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(_nextSelection);
    }

    public void StartGame()
    {
        _characterSelect.SetActive(true);
        _saveSelect.SetActive(false);
        _settingsMenu.SetActive(false);
        _quitMenu.SetActive(false);
        gameObject.SetActive(false);
    }

    public void GotoMainMenu()
    {
        _saveSelect.SetActive(false);
        _settingsMenu.SetActive(false);
        _quitMenu.SetActive(false);
        gameObject.SetActive(true);
        _characterSelect.SetActive(false);
        _nextSelection = _mainMenuDefaultBind;

        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(_nextSelection);
    }

    public void OpenSaveScreen()
    {
        _saveSelect.SetActive(true);
        _settingsMenu.SetActive(false);
        _quitMenu.SetActive(false);
        gameObject.SetActive(false);
        _characterSelect.SetActive(false);
        _nextSelection = _saveMenuDefaultBind;

        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(_nextSelection);
    }
}
