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
        public int DeviceID { get; private set; }
        private Controls controls;
        [HideInInspector]
        public GameObject _useableObject;

        private void Awake()
        {
            controls = new Controls();
            controls.Player.Move.Enable();
            controls.Player.Pause.Enable();
            controls.Player.Use.Enable();
            controls.Player.Move.performed += ctx => ReadMovementInput(ctx);
            controls.Player.Pause.performed += ctx => DeviceID = ctx.control.device.deviceId;
            controls.Player.Use.performed += ctx => Use(ctx);
        }

        public void SetDevice(int deviceID = -1, InputDevice device = null)
        {
            if (device == null && deviceID == -1)
            {
                Debug.LogError("Invalid parameters!" + gameObject.name + " now uses inputDevice 1 as input!");
                DeviceID = 1;
            }
            if (device != null)
            {
                DeviceID = device.deviceId;
                return;
            }
            DeviceID = deviceID;
        }

        private void OnDestroy()
        {
            controls.Player.Move.performed -= ctx => ReadMovementInput(ctx);
            controls.Player.Pause.performed -= ctx => DeviceID = ctx.control.device.deviceId;
            controls.Player.Use.performed -= ctx => Use(ctx);
            controls.Player.Move.Disable();
            controls.Player.Pause.Disable();
            controls.Player.Use.Disable();
            controls = null;
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
                transform.position = Vector3.zero;
        }

        private void ReadMovementInput(InputAction.CallbackContext context)
        {
            if (DeviceID == context.control.device.deviceId)
                _direction = context.ReadValue<Vector2>();
        }

        private void Use(InputAction.CallbackContext context)
        {
            if (DeviceID == context.control.device.deviceId && _useableObject != null)
            {
                _useableObject.GetComponentInParent<PlayerUseable>().Use(gameObject);
                Debug.Log("ss");
            }
        }       

    }
}