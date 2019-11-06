using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

public class PauseMenu : MonoBehaviour
{
    public GameObject _settingsMenu;
    public GameObject _quitMenu;
    public AudioClip _click;

    public void ButtonClickSound()
    {
        AudioManager.Instance.PlaySound(_click);
    }
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
