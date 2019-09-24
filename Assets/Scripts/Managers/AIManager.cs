using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class AIManager : MonoBehaviour
    {
        #region Members
        public enum State
        {
            None = 0,
            Moving = 1,
            Waiting = 2,
            Served = 3,
            PassedOut = 4,
            Fighting = 5
        }
    protected Customer[] _activeAgents;
        #endregion
        #region Properties
        public Customer[] ActiveAgents { get => _activeAgents; }
        #endregion
        #region Unity Methods
        private void Awake()
        {

        }
        private void Start()
        {

        }
        #endregion
        #region Methods
        #endregion
    }
}
