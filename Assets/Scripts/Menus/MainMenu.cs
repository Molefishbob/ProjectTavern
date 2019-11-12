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




    public void OpenSettingsMenu()
    {
        _settingsMenu.SetActive(true);
        _saveSelect.SetActive(false);
        _quitMenu.SetActive(false);
    }

    public void OpenQuitMenu()
    {
        _settingsMenu.SetActive(false);
        _saveSelect.SetActive(false);
        _quitMenu.SetActive(true);
    }

    public void StartGame()
    {
        _saveSelect.SetActive(true);
        _settingsMenu.SetActive(false);
        _quitMenu.SetActive(false);
        gameObject.SetActive(false);
    }

    
}
