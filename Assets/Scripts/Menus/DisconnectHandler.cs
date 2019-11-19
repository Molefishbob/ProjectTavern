using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisconnectHandler : MonoBehaviour
{
    public static DisconnectHandler Instance;
    [SerializeField] private DisconnectedPlayer[] _playerSlots = null;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void UpdatePlayers()
    {
        for (int i = 0; i < GameInput.ControlsManager.Instance.InUseControllers.Count; i++)
        {
            _playerSlots[i].OverlayEnabled = GameInput.ControlsManager.Instance.InUseControllers[i] == 0;
            _playerSlots[i].ControllerText = GameInput.ControlsManager.Instance.InUseControllers[i] ==
                0 ? "Disconnected" :
                UnityEngine.InputSystem.InputSystem.GetDeviceById(GameInput.ControlsManager.Instance.InUseControllers[i]).displayName.Contains("Keyboard")
                ? "Keyboard" : "GamePad";
        }
    }
}
