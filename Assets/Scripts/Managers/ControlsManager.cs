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

        private void Start()
        {
            if (_instance == null)
            {
                _instance = this;
                gameObject.name = "ControlsManager";
                Init();
            }
        }

        private void Init()
        {
            DontDestroyOnLoad(gameObject);
            if (_playerPrefab == null)
            {
                _playerPrefab = Resources.Load<PlayerMovement>("Test_Player");
            }
            Initialized = true;
            Debug.Log("Control Manager Initialized");

            if (SpawnDebugShip)
            {
                int tmp = 1;
                foreach (InputDevice device in InputSystem.devices)
                {
                    if (device.path.Contains("Keyboard"))
                        tmp = device.deviceId;
                }
                AddPlayer(1);
            }
        }

        public void AddPlayer(int deviceID = -1, InputDevice device = null)
        {
            foreach (PlayerMovement activePlayer in _activePlayers)
            {
                int id = deviceID;

                if (device != null)
                    id = device.deviceId;

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

        public bool DebugCheck(PlayerMovement player)
        {
            return _activePlayers.Contains(player);
        }
    }
}