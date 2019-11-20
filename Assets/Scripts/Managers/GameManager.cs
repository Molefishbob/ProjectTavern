using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

#region Events
public delegate void GenericEvent();
public delegate void ValueChangedInt(int amount);
public delegate void ValueChangedFloat(float amount);
public delegate void ValueChangedBool(bool value);
#endregion


namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Parameters
        private static GameManager _instance;
        public event ValueChangedBool OnGamePauseChanged;
        public AudioClip _levelMusic;
        public AudioClip _menuMusic;
        [SerializeField]
        private string MainMenu = "MainMenu";
        [SerializeField]
        private GameObject _loadingScreen = null;
        [SerializeField]
        private GameObject _player1 = null;
        [SerializeField]
        private GameObject _player2 = null;
        [SerializeField]
        private GameObject _player3 = null;
        [SerializeField]
        private GameObject _player4 = null;
        protected string[] _saveFiles = { "save1", "save2", "save3" };
        private LevelManager _levelManager = null;
        private PauseMenu _pauseMenu;
        private AudioManager _audio;
        private int _mainMenuID = 0;
        private int _currentSceneID = 0;
        private string _currentSave;
        private float _timeScaleBeforePause = 1.0f;
        private AudioSource _musicSource;

        #endregion

        #region Properties
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Instantiate(Resources.Load<GameManager>("GameManager"));
                    _instance.Init();
                }

                return _instance;
            }
            private set
            {
                Instance = value;
            }
        }

        /// <summary>
        /// Is the game currently paused?
        /// </summary>
        public bool GamePaused { get; private set; }
        public bool MenuActive { get; private set; }
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
        public LevelManager LevelManager
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
        public PauseMenu PauseMenu
        {
            get
            {
                if (_pauseMenu == null)
                    Debug.LogWarning("Pause menu not assigned!");

                return _pauseMenu;

            }
            set
            {
                _pauseMenu = value;
            }
        }

        public GameObject Player1
        {
            get
            {
                if (_player1 == null)
                {
                    Debug.LogError("Player 1 not assigned!");
                }
                return _player1;
            }
        }
        public GameObject Player2
        {
            get
            {
                if (_player2 == null)
                {
                    Debug.LogError("Player 2 not assigned!");
                }
                return _player2;
            }
        }
        public GameObject Player3
        {
            get
            {
                if (_player3 == null)
                {
                    Debug.LogError("Player 3 not assigned!");
                }
                return _player3;
            }
        }
        public GameObject Player4
        {
            get
            {
                if (_player4 == null)
                {
                    Debug.LogError("Player 4 not assigned!");
                }
                return _player4;
            }
        }
        #endregion

        #region Unity Methods
        private void Start()
        {

            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else if (_instance != this)
            {
                _instance = this;
                Init();
            }
            
            SerializationManager.SaveData save = SerializationManager.LoadedSave;
        }

        private void Init()
        {
            _musicSource = GetComponent<AudioSource>();
            _audio = GetComponent<AudioManager>();
            _currentSceneID = SceneManager.GetActiveScene().buildIndex;

            int count = SceneManager.sceneCountInBuildSettings;
            bool found = false;
            for (int a = 0; a < count; a++)
            {
                string path = SceneUtility.GetScenePathByBuildIndex(a);
                string sceneName = path.Substring(0, path.Length - 6).Substring(path.LastIndexOf('/') + 1);
                if (sceneName == MainMenu)
                {
                    _mainMenuID = a;
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                Debug.LogError("MainMenu not in build or it is named incorrectly");
            }

            DontDestroyOnLoad(this);

            if (_currentSceneID != _mainMenuID)
            {
                MenuActive = false;
                ActivateGame(true);
            } else
            {
                MenuActive = true;
            }
        }

        private void OnDestroy()
        {
            if (_currentSceneID != _mainMenuID)
                SaveData(_currentSave);
        }
        #endregion

        /// <summary>
        /// Pauses all in-game objects and sets timescale to 0.
        /// </summary>
        public void PauseGame()
        {
            if (!GamePaused)
            {
                GamePaused = true;
                _timeScaleBeforePause = Time.timeScale;
                Time.timeScale = 0;
                if (OnGamePauseChanged != null)
                {
                    OnGamePauseChanged(true);
                }
            }
            else
            {
                Debug.LogWarning("Game is already paused.");
            }
        }

        /// <summary>
        /// Unpauses all in-game objects and sets timescale back to what it was before pausing.
        /// </summary>
        public void UnPauseGame()
        {
            if (GamePaused)
            {
                GamePaused = false;
                Time.timeScale = _timeScaleBeforePause;
                if (OnGamePauseChanged != null)
                {
                    OnGamePauseChanged(false);
                }
            }
            else
            {
                Debug.LogWarning("Game is already unpaused.");
            }
        }

        /// <summary>
        /// Selects a save to load from memory
        /// </summary>
        /// <param name="save">the name of the savefile</param>
        public void SelectSave(string save)
        {
            for (int a = 0; a < _saveFiles.Length; a++)
            {
                if (_saveFiles[a] == save)
                {
                    _currentSave = save;
                    SerializationManager.LoadSave(_currentSave);
                }
                ChangeScene(SerializationManager.LoadedSave.LastLevelCleared + 1, true);
            }
        }

        /// <summary>
        /// Activates the level music and unpauses the game
        /// </summary>
        /// <param name="active">Start music</param>
        public void ActivateGame(bool active)
        {
            // TODO: Ask for player amount and activate required amount
            if (active) PlayLevelMusic();
            if (GamePaused) UnPauseGame();
        }

        public void ChangeScene(int id, bool useLoadingScreen)
        {
            // TODO: Change scene, deploy loading screen, etc.
            _musicSource?.Stop();
            PauseMenu?.gameObject.SetActive(false);
            if (id >= SceneManager.sceneCountInBuildSettings)
            {
                Debug.LogWarning("No scene with ID: " + id);
                SerializationManager.DeleteAndMakeDefaultSave(_currentSave);
                ChangeToMainMenu();
            }
            else
            {
                if (useLoadingScreen)
                {
                    // TODO: LoadingScreen.BeginLoading();
                    ActivateGame(true);
                }
                ActivateGame(false);
                _currentSceneID = id;
                SceneManager.LoadScene(id);
                if (GamePaused) UnPauseGame();
            }
        }

        public void NextLevel()
        {

            SerializationManager.LoadedSave.LastLevelCleared++;
            SerializationManager.SaveSave(_currentSave);
            ChangeScene(SerializationManager.LoadedSave.LastLevelCleared + 1, true);
            ActivateGame(true);
        }

        public void ChangeToMainMenu()
        {
            ActivateGame(false);
            SceneManager.LoadScene(_mainMenuID);
            PlayMenuMusic();
        }

        public void SaveData(string levelSave = "")
        {
            SerializationManager.SaveSettings("Settings");
            if (levelSave != "")
                SerializationManager.SaveSave(levelSave);
        }

        /// <summary>
        /// Quits the game to desktop.
        /// </summary>
        public void QuitGame()
        {
            SaveData();
            Application.Quit();
        }

        /// <summary>
        /// Loads the first level.
        /// </summary>
        public void StartNewGame()
        {
            SerializationManager.DeleteAndMakeDefaultSave(_currentSave);
            ChangeScene(SerializationManager.LoadedSave.LastLevelCleared + 1, true);
        }

        /// <summary>
        /// Continue game from a saved checkpoint.
        /// </summary>
        public void ContinueGame()
        {
            ChangeScene(SerializationManager.LoadedSave.LastLevelCleared + 1, true);
        }

        /// <summary>
        /// Reloads the active scene.
        /// </summary>
        public void ReloadScene(bool useLoadingScreen)
        {
            ActivateGame(false);
            //if (useLoadingScreen) LoadingScreen.BeginLoading();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void PlayLevelMusic()
        {
            _audio.PlayMusic(_levelMusic);
        }

        public void PlayMenuMusic()
        {
            _audio.PlayMusic(_menuMusic);
        }

        public void StopAllMusic()
        {
            _audio.StopMusic();
        }

    }
}
