using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

public class PauseMenu : MonoBehaviour
{
    public GameObject _settingsMenu;
    public GameObject _quitMenu;
    public AudioClip _click;

    public GameObject DefaultSelection;
    public static PauseMenu Menu;
    private GameObject _continueButton = null;

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
        _continueButton = DefaultSelection;
        Menu = this;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(DefaultSelection);
    }

    private void OnDisable()
    {
        _quitMenu.SetActive(false);
        _settingsMenu.SetActive(false);
        DefaultSelection = _continueButton;
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

        for (int i = 0; i < transform.GetChild(2).childCount; i++)
        {
            transform.GetChild(2).GetChild(i).GetComponent<UnityEngine.UI.Button>().interactable = false;
        }

        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(_quitMenu.transform.GetChild(0).GetChild(2).gameObject);
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
