using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Managers;

namespace GameInput
{
    [RequireComponent(typeof(PlayerState))]
    /// <summary>
    /// Handles the binding of a device to  a singular player
    /// Also handles the progressbars for actions.
    /// Action bars might be moved elsewhere.
    /// </summary>
    public class PlayerMovement : MonoBehaviour
    {
        #region MemberVariables

        [SerializeField] private float _maxSpeed = 0.1f;
        [SerializeField] private float _acceleration = 10;
        [SerializeField] private float _deaccleration = 5;
        [SerializeField] private Vector2 _inputDirection = new Vector2();
        private Vector2 _movementVector = new Vector2();

        private Controls controls;
        private PlayerState _myState = null;

        #endregion

        /// <summary>
        /// Which device is set to this player
        /// </summary>
        public int DeviceID { get; private set; }

        #region UnityMethods

        private void Awake()
        {
            controls = new Controls();
            controls.Player.Move.Enable();
            controls.Player.Pause.Enable();
            controls.Player.Use.Enable();
            controls.Player.Move.performed += ctx => ReadMovementInput(ctx);
            controls.Player.Pause.performed += ctx => ReadPauseInput(ctx);
            controls.Player.Use.performed += ctx => Use(ctx);

            _myState = GetComponent<PlayerState>();
        }

        /// <summary>
        /// Clear and disable unused stuff
        /// </summary>
        private void OnDestroy()
        {
            controls.Player.Move.performed -= ctx => ReadMovementInput(ctx);
            controls.Player.Pause.performed -= ctx => ReadPauseInput(ctx);
            controls.Player.Use.performed -= ctx => Use(ctx);
            controls.Player.Move.Disable();
            controls.Player.Pause.Disable();
            controls.Player.Use.Disable();
            controls = null;

            if (ControlsManager.DoesExist)
                ControlsManager.Instance.RemovePlayer(DeviceID);
        }

        private void FixedUpdate()
        {
            Move();
        }

        #endregion

        #region Methods

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
            _inputDirection = Vector2.zero;
            _movementVector = Vector2.zero;
        }

        /// <summary>
        /// Uses direction gotten from the controller and moves accordingly
        /// Also checks if out of screen and resets player
        /// </summary>
        private void Move()
        {
            _movementVector += _inputDirection * _inputDirection.magnitude * _acceleration * Time.deltaTime;

            _movementVector -= _movementVector * _deaccleration * Time.deltaTime;

            _movementVector = Vector3.ClampMagnitude(_movementVector, _maxSpeed);

            // Something smarter needs to be done
            transform.Translate(_movementVector);

            // Move it into center of the level maybe, instead of this
            Vector2 posInCamera = Camera.main.WorldToViewportPoint(transform.position);
            if (posInCamera.x < 0 || posInCamera.x > 1 || posInCamera.y < 0 || posInCamera.y > 1)
                transform.position = Vector3.zero;
        }

        /// <summary>
        /// Read direction from given context if it is assinged one
        /// </summary>
        /// <param name="context"></param>
        private void ReadMovementInput(InputAction.CallbackContext context)
        {
            if (DeviceID == context.control.device.deviceId)
                _inputDirection = context.ReadValue<Vector2>();
        }

        private void ReadPauseInput(InputAction.CallbackContext context)
        {
            if (DeviceID == context.control.device.deviceId)
            {
                bool active = GameManager.Instance.PauseMenu.gameObject.activeSelf;
                GameManager.Instance.PauseMenu.gameObject.SetActive(!active);
                if (!active)
                    GameManager.Instance.PauseGame();
                else
                    GameManager.Instance.UnPauseGame();
            }
        }

        /// <summary>
        /// Tell the other object that it is being used, Used object is reported from triggers
        /// </summary>
        /// <param name="context">Which device did this</param>
        private void Use(InputAction.CallbackContext context)
        {
            if (DeviceID == context.control.device.deviceId)
            {
                _myState.UseUseable();
            }
        }

        #endregion


        #region DebugStuff

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + new Vector3(_inputDirection.x, _inputDirection.y));

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + new Vector3(_movementVector.x * 10, _movementVector.y * 10));
        }

        #endregion
    }
}