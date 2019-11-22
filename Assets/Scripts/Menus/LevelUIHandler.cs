using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

public class LevelUIHandler : MonoBehaviour
{
    [SerializeField]
    protected PauseMenu _pauseMenu;

    private void Awake()
    {
        if (GameManager.Instance.PauseMenu != null && GameManager.Instance.PauseMenu != _pauseMenu)
        {
            Destroy(_pauseMenu);
        } else
        {
            print("yes");
            GameManager.Instance.PauseMenu = _pauseMenu;
        }
    }
}
