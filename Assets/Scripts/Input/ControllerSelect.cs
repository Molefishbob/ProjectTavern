using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameInput;
using TMPro;

public class ControllerSelect : MonoBehaviour
{
    public Controls _controls = null;
    private GameObject[] _playerSlots = new GameObject[4];

    /// <summary>
    /// Set from editor
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI[] _controllerTexts = new TextMeshProUGUI[4];

    void Awake()
    {
        _controls = new Controls();
        _controls.Assinging.Assing.performed += TryAddDevice;
        _controls.Assinging.Start.performed += AllSet;
        _controls.Assinging.Enable();

        for (int i = 0; i < _playerSlots.Length; i++)
        {
            _playerSlots[i] = transform.GetChild(0).GetChild(i).gameObject;
        }

        for (int i = 0; i < ControlsManager.Instance.InUseControllers.Count; i++)
        {
            _playerSlots[i].SetActive(true);
        }
    }

    private void AllSet(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (ControlsManager.Instance.InUseControllers.Contains(context.control.device.deviceId))
            Managers.GameManager.Instance.StartCurrentLevel();
    }

    private void OnDestroy()
    {
        _controls.Assinging.Assing.performed -= TryAddDevice;
        _controls.Assinging.Assing.performed -= AllSet;
        _controls.Assinging.Disable();
        _controls = null;
    }

    private void TryAddDevice(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (ControlsManager.Instance.InUseControllers.Count >= 4)
            return;

        int id = context.control.device.deviceId;

        if (!ControlsManager.Instance.InUseControllers.Contains(id) && ControlsManager.Instance.InUseControllers.Count < _playerSlots.Length)
        {
            ControlsManager.Instance.InUseControllers.Add(id);
            Debug.Log("Added: " + id + "|" + context.control.device.name);
        }

        for (int i = 0; i < ControlsManager.Instance.InUseControllers.Count; i++)
        {
            if (!_playerSlots[i].activeSelf)
                _playerSlots[i].SetActive(true);

            if (ControlsManager.Instance.InUseControllers[i] == id)
            {
                if (context.control.device.name != "Keyboard")
                    _controllerTexts[i].text = "Gamepad";
                else
                    _controllerTexts[i].text = context.control.device.name;
            }
        }
    }
}
