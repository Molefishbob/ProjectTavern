using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisconnectHandler : MonoBehaviour
{
    public static DisconnectHandler Instance;
    [SerializeField] private DisconnectedPlayer[] _playerSlots = null;
    private GameObject _visualHolder = null;
    private Controls _controls = null;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        _visualHolder = transform.GetChild(0).gameObject;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
        
        if (_controls != null)
        {
            _controls.Assinging.Start.Disable();
            _controls.Assinging.Start.performed -= DropInActives;
            _controls = null;
        }
    }

    public void UpdatePlayers()
    {
        if (GameInput.ControlsManager.Instance.InUseControllers.Contains(0) && !_visualHolder.activeSelf)
        {
            _visualHolder.SetActive(true);
            _controls = new Controls();
            _controls.Assinging.Start.performed += DropInActives;
            _controls.Assinging.Start.Enable();
        }
        else if (!GameInput.ControlsManager.Instance.InUseControllers.Contains(0) && _visualHolder.activeSelf)
        {
            _visualHolder.SetActive(false);
            _controls.Assinging.Start.Disable();
            _controls.Assinging.Start.performed -= DropInActives;
            _controls = null;
        }

        for (int i = 0; i < GameInput.ControlsManager.Instance.InUseControllers.Count; i++)
        {
            _playerSlots[i].gameObject.SetActive(true);
            _playerSlots[i].OverlayEnabled = GameInput.ControlsManager.Instance.InUseControllers[i] == 0;
            _playerSlots[i].ControllerText = GameInput.ControlsManager.Instance.InUseControllers[i] ==
                0 ? "Disconnected" :
                UnityEngine.InputSystem.InputSystem.GetDeviceById(GameInput.ControlsManager.Instance.InUseControllers[i]).displayName.Contains("Keyboard")
                ? "Keyboard" : "GamePad";
        }

        for (int i = GameInput.ControlsManager.Instance.InUseControllers.Count; i < _playerSlots.Length; i++)
        {
            _playerSlots[i].gameObject.SetActive(false);
        }
    }

    private void DropInActives(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        GameInput.ControlsManager.Instance.RemoveInActivePlayers();
        UpdatePlayers();
        Managers.GameManager.Instance.UnPauseGame();
    }
}
