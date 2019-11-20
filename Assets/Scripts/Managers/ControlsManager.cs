using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameInput
{
    public class ControlsManager : MonoBehaviour
    {
        public static ControlsManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject("ControlsManager").AddComponent<ControlsManager>();
                    _instance.Init();
                }
                return _instance;
            }
        }

        public bool Initialized { get; private set; }
        public bool SpawnDebugShip = false;
        private static ControlsManager _instance;
        private List<PlayerMovement> _activePlayers = new List<PlayerMovement>();
        private PlayerMovement _playerPrefab = null;

        public List<int> InUseControllers = new List<int>();

        private void Start()
        {
            if (_instance == null)
            {
                _instance = this;
                gameObject.name = "ControlsManager";
                Init();
            }
        }

        /// <summary>
        /// Initializes the control manager
        /// </summary>
        private void Init()
        {
            DontDestroyOnLoad(gameObject);

            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;

            if (_playerPrefab == null)
            {
                _playerPrefab = Resources.Load<PlayerMovement>("Test_Player");
            }

            InputSystem.onDeviceChange += (device, reason) => DeviceChanged(device, reason);

            if (SpawnDebugShip)
            {
                int tmp = 1;
                foreach (InputDevice device in InputSystem.devices)
                {
                    if (device.path.Contains("Keyboard"))
                        tmp = device.deviceId;
                }
                AddPlayer(tmp);
            }

            Initialized = true;
            Debug.Log("Control Manager Initialized");
        }

        private void OnDestroy()
        {
            if (_instance == this)
                UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(UnityEngine.SceneManagement.Scene Scene, UnityEngine.SceneManagement.LoadSceneMode Loadmode)
        {
            if (Scene.name.ToLower().Contains("level"))
            {
                foreach (int active in InUseControllers)
                {
                    AddPlayer(active);
                }
            }
        }

        /// <summary>
        /// Add an active player with some device
        /// prefers device over deviceID
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="device"></param>
        public void AddPlayer(int deviceID = -1, InputDevice device = null)
        {
            int id = deviceID;

            if (device != null)
                id = device.deviceId;

            foreach (PlayerMovement activePlayer in _activePlayers)
            {
                if (activePlayer.DeviceID == id)
                {
                    Debug.LogError("There is already a player on DeviceID " + id);
                    return;
                }
            }

            PlayerMovement tmp = Instantiate(_playerPrefab, Vector3.zero, Quaternion.identity);
            _activePlayers.Add(tmp);

            tmp.SetDevice(deviceID, device);
        }

        public void RemoveInActivePlayers()
        {
            for (int i = 0; i < _activePlayers.Count; i++)
            {
                if (_activePlayers[i].DeviceID == 0)
                {
                    Destroy(_activePlayers[i].gameObject);
                    _activePlayers[i] = null;
                }
            }

            InUseControllers.RemoveAll(t => t == 0);
            _activePlayers.RemoveAll(t => t == null);
        }

        /// <summary>
        /// Is the specific player active?
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool DebugCheck(PlayerMovement player)
        {
            return _activePlayers.Contains(player);
        }

        /// <summary>
        /// Disconnect handling.
        /// Currently if the game is running, when Device is added
        /// gives the first 0 controlled player control with the added device
        /// TODO:   Pause the game when disconnect happens, if that device was in use.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="reason"></param>
        private void DeviceChanged(InputDevice device, InputDeviceChange reason)
        {
            if (reason != InputDeviceChange.Added && reason != InputDeviceChange.Removed)
                Debug.Log("Device " + reason.ToString() + ". ID: " + device.deviceId);

            switch (reason)
            {
                // Im going to Use Added instead of reconnected, because device might
                // get disconnected and be recognised as something else.
                case InputDeviceChange.Added:
                    foreach (var item in _activePlayers)
                    {
                        if (item.DeviceID == 0)
                        {
                            item.SetDevice(device.deviceId);
                            break;
                        }
                    }

                    for (int i = 0; i < InUseControllers.Count; i++)
                    {
                        if (InUseControllers[i] == 0)
                        {
                            InUseControllers[i] = device.deviceId;
                            break;
                        }
                    }

                    if (!InUseControllers.Contains(0) && Managers.GameManager.Instance.GamePaused)
                    {
                        Debug.Log("UnPausing the game for now this way...");
                        Managers.GameManager.Instance.UnPauseGame();
                    }
                    break;

                // Going to use removed, if the device is for some reason removed in runtime
                // Remove != Disconnected
                case InputDeviceChange.Removed:
                    foreach (var item in _activePlayers)
                    {
                        if (item.DeviceID == device.deviceId)
                        {
                            item.SetDevice(0);
                            item.StopMovement();
                        }
                    }

                    // ToDo: Add someway to gamemanager to know that the pause was called because of
                    //       Controller disconnect.
                    for (int i = 0; i < InUseControllers.Count; i++)
                    {
                        if (InUseControllers[i] == device.deviceId)
                        {
                            InUseControllers[i] = 0;
                            if (!Managers.GameManager.Instance.GamePaused)
                            {
                                Managers.GameManager.Instance.PauseGame();
                            }
                        }
                    }
                    break;

                case InputDeviceChange.Disconnected:
                    break;
                case InputDeviceChange.Reconnected:
                    break;
            }

            if (DisconnectHandler.Instance != null)
            {
                DisconnectHandler.Instance.UpdatePlayers();
            }
        }
    }
}