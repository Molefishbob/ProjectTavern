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
        private PlayerUseable _useableObject;

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

        /// <summary>
        /// Assing an Input device to this player, if non is given, input will be given to InputDeviceID 1.
        /// Device is used over DeviceID
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="device"></param>
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

        /// <summary>
        /// Stops all movement
        /// </summary>
        public void StopMovement()
        {
            _direction = Vector2.zero;
        }

        /// <summary>
        /// Clear and disable unused stuff
        /// </summary>
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

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            // Something smarter needs to be done
            transform.Translate(_direction * _speed * Time.deltaTime);

            // Move it into center of the level maybe, instead of this
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
            if (DeviceID == context.control.device.deviceId && _useableObject != null && !_useableObject.IsBeingUsed)
            {
                _useableObject.Use(gameObject);
                Debug.Log("Action started on " + _useableObject.GetType().ToString());
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "PlayerUsable")
            {
                _useableObject = collision.GetComponent<PlayerUseable>();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (_useableObject?.gameObject == collision.gameObject)
            {
                if (_useableObject.IsBeingUsed)
                {
                    _useableObject.InterruptAction();
                    Debug.Log(_useableObject.GetType().ToString() + " action interrupted!");
                }
                _useableObject = null;
            }
        }
    }
}