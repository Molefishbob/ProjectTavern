using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameInput
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _speed = 10;
        [SerializeField] private Vector2 _direction = new Vector2();
        public int DeviceID = 1;
        private Controls controls;

        private void Awake()
        {
            controls = ControlsManager.Instance.GetControls();
            controls.Enable();
            controls.Player.Move.performed += ctx => ReadMovementInput(ctx);
            controls.Player.Pause.performed += ctx => DeviceID = ctx.control.device.deviceId;

            InputSystem.onDeviceChange += (device, change) =>
            {
                if (change == InputDeviceChange.Reconnected)
                    DeviceID = device.deviceId;
            };
        }

        private void OnDestroy()
        {
            controls.Player.Move.performed -= ctx => ReadMovementInput(ctx);
            controls.Player.Pause.performed -= ctx => DeviceID = ctx.control.device.deviceId;
        }

        private void Update()
        {
            Move();
        }

        public void Move()
        {
            transform.Translate(_direction * _speed * Time.deltaTime);
            Vector2 posInCamera = Camera.main.WorldToViewportPoint(transform.position);
            if (posInCamera.x < 0 || posInCamera.x > 1 || posInCamera.y < 0 || posInCamera.y > 1)
                transform.position = new Vector3();
        }

        private void ReadMovementInput(InputAction.CallbackContext context)
        {
            // Debug.Log(context.control.device.deviceId);
            if (DeviceID == context.control.device.deviceId)
            {
                // Debug.Log("Device ID:" + context.control.device.deviceId + " moved " + gameObject.name);
                _direction = context.ReadValue<Vector2>();
            }
        }
    }
}
