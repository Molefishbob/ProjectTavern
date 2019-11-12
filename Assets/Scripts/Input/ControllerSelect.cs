using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameInput;

public class ControllerSelect : MonoBehaviour
{
    public Controls _controls = null;
    private GameObject[] playerSlots = new GameObject[4];

    void Awake()
    {
        _controls = new Controls();
        _controls.Assinging.Assing.performed += TryAddDevice;
        _controls.Assinging.Start.performed += AllSet;
        _controls.Assinging.Enable();

        for (int i = 0; i < playerSlots.Length; i++)
        {
            playerSlots[i] = transform.GetChild(0).GetChild(i).gameObject;
        }
    }

    private void AllSet(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (ControlsManager.Instance.InUseControllers.Contains(context.control.device.deviceId))
            Managers.GameManager.Instance.NextLevel();
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
        int id = context.control.device.deviceId;

        if (!ControlsManager.Instance.InUseControllers.Contains(id) && ControlsManager.Instance.InUseControllers.Count < playerSlots.Length)
        {
            ControlsManager.Instance.InUseControllers.Add(id);
            Debug.Log("Added: " + id + "|" + context.control.device.name);
        }

        for (int i = 0; i < ControlsManager.Instance.InUseControllers.Count; i++)
        {
            playerSlots[i].SetActive(true);
        }
    }
}
