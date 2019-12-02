using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

public class PauseMenu : MonoBehaviour
{
    public GameObject _settingsMenu;
    public GameObject _quitMenu;
    public AudioClip _click;

    private void Start()
    {
        if (GameManager.Instance.PauseMenu == null)
        {
            GameManager.Instance.PauseMenu = this;
        } else
        {
            Debug.LogWarning("Pause menu already assigned! Removing the new pause menu..");
            Destroy(gameObject);
        }

        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        _quitMenu.SetActive(false);
        _settingsMenu.SetActive(false);
    }

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

    public void ToMainMenu()
    {
        GameManager.Instance.ChangeToMainMenu();
    }

    public void Continue()
    {
        GameManager.Instance.UnPauseGame();
        gameObject.SetActive(false);
    }
}
