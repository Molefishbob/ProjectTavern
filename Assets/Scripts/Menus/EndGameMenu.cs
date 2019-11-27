using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Managers;
using UnityEngine.UI;

public class EndGameMenu : MonoBehaviour
{
    [SerializeField]
    protected TMP_Text _statusText;
    public AudioClip _clip;
    public GameObject _quitMenu;
    public Button _continueButton;
    private void OnEnable()
    {
        _statusText.text = "YOU ARE " + LevelManager.Instance.LevelEndText;
    }

    public void ButtonClickSound()
    {
        AudioManager.Instance.PlaySound(_clip);
    }

    public void OpenQuitMenu()
    {
        _quitMenu.SetActive(true);
    }

    public void ToMainMenu()
    {
        GameManager.Instance.ChangeToMainMenu();
    }

    public void ContinueButtonSettings(bool win)
    {
        _continueButton.onClick.RemoveAllListeners();
        _continueButton.onClick.AddListener(ButtonClickSound);
        if (win)
        {
            _continueButton.GetComponentInChildren<TMP_Text>().text = "Continue";
            _continueButton.onClick.AddListener(Continue);
        } else
        {
            _continueButton.GetComponentInChildren<TMP_Text>().text = "Retry";
            _continueButton.onClick.AddListener(Retry);
        }
    }

    public void Continue()
    {
        GameManager.Instance.NextLevel();
    }

    public void Retry()
    {
        GameManager.Instance.ReloadScene(true);
    }
}
