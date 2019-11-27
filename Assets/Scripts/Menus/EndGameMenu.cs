using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Managers;

public class EndGameMenu : MonoBehaviour
{
    [SerializeField]
    protected TMP_Text _statusText;
    public AudioClip _clip;
    public GameObject _quitMenu;
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

    public void Continue()
    {
        GameManager.Instance.NextLevel();
    }
}
