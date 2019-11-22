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

    public void OpenSettingsMenu()
    {
        _settingsMenu.SetActive(true);
        _saveSelect.SetActive(false);
        _quitMenu.SetActive(false);
        _characterSelect.SetActive(false);
    }

    public void OpenQuitMenu()
    {
        _settingsMenu.SetActive(false);
        _saveSelect.SetActive(false);
        _quitMenu.SetActive(true);
        _characterSelect.SetActive(false);
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
    }

    public void OpenSaveScreen()
    {
        _saveSelect.SetActive(true);
        _settingsMenu.SetActive(false);
        _quitMenu.SetActive(false);
        gameObject.SetActive(false);
        _characterSelect.SetActive(false);
    }
}
