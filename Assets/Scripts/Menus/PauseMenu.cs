using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject _settingsMenu;
    public GameObject _quitMenu;
    public void OpenSettingsMenu()
    {
        _settingsMenu.SetActive(true);
        _quitMenu.SetActive(false);
    }

    public void OpenQuitMenu()
    {
        _settingsMenu.SetActive(false);
        _quitMenu.SetActive(true);
    }

    public void Continue()
    {
        gameObject.SetActive(false);
    }
}
