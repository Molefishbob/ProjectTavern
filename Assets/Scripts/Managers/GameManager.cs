﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#region Events
public delegate void GenericEvent();
public delegate void ValueChangedInt(int amount);
public delegate void ValueChangedFloat(float amount);
public delegate void ValueChangedBool(bool value);
#endregion

public class GameManager : MonoBehaviour
{
    #region Parameters
    public static GameManager Instance;
    [SerializeField]
    private string MainMenu = "MainMenu";
    private int _mainMenuID = 0;
    private int _currentSceneID = 0;
    [SerializeField]
    private GameObject _loadingScreen = null;
    [SerializeField]
    private GameObject _levelManager = null;
    #endregion

    #region Properties
    /// <summary>
    /// Is the game currently paused?
    /// </summary>
    public bool GamePaused { get; private set; }
    public GameObject LoadingScreen
    {
        get
        {
            if (_loadingScreen == null)
                Debug.LogError("Loading Screen not assigned!");

            return _loadingScreen;

        }
        set
        {
            _loadingScreen = value;
        }
    }
    public GameObject LevelManager
    {
        get
        {
            if (_levelManager == null)
                Debug.LogError("Level Manager not assigned!");

            return _levelManager;

        }
        set
        {
            _levelManager = value;
        }
    }
    #endregion

    #region Unity Methods
    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        _currentSceneID = SceneManager.GetActiveScene().buildIndex;
        int count = SceneManager.sceneCountInBuildSettings;

        for (int a = 0; a < count; a++)
        {
            if (SceneManager.GetSceneByBuildIndex(a).name == MainMenu)
            {
                _mainMenuID = a;
                break;
            }
            if (a == count - 1)
            {
                Debug.LogError("MainMenu not in build or it is named incorrectly!");
            }
        }
    }
    #endregion

    public void ChangeScene(int id, bool useLoadingScreen)
    {
        // TODO: Change scene, deploy loading screen, etc.
    }

    public void NextLevel()
    {
        // TODO: Change to next level
    }

    public void ChangeToMainMenu()
    {
        SceneManager.LoadScene(_mainMenuID);
    }


}
