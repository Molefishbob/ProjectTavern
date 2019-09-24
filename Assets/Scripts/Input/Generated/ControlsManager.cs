using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                    _instance = new GameObject().AddComponent<ControlsManager>();
                    DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }

        private static ControlsManager _instance;
        private Controls _controls;

        public Controls GetControls()
        {
            if (_controls == null)
                _controls = new Controls();
            return _controls;
        }
    }
}