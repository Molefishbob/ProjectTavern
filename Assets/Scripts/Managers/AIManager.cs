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
            Fighting = 5,
            Ordered = 6
        }
        protected List<Customer> _activeAgents;
        protected Customer[] _customers;
        public static AIManager Instance;
        private int iterations = 0;
        #endregion

        #region Properties
        public List<Customer> ActiveAgents { get => _activeAgents; }
        #endregion

        #region Unity Methods
        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(gameObject);
            else Instance = this;

            _activeAgents = new List<Customer>();
            _customers = Resources.LoadAll<Customer>("Customers");
        }
        private void Start()
        {

        }

        private void Update()
        {
            if (iterations <= 2)
            {
                if (iterations == 2)
                {
                    SpawnAI();
                }
                iterations++;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Spawns an AI to the level.
        /// 
        /// Spawns the AI, adds it to the list of AIs in the scene.
        /// Finds a table to sit for the AI, if any are free.
        /// In case there are no tables free it will look for a line spot.
        /// In case there are no line spots this method should not even be called.
        /// </summary>
        public void SpawnAI()
        {
            // TODO: Check for free tables
            // TODO: If no free tables check for free spots in the row
            // TODO: if no free spots in the row Murder the programmer who called this method
            Customer ai = LevelManager.Instance.GetCustomer();
            ai.transform.position = LevelManager.Instance.Door.position;
            LevelManager.Instance.GetSeat(ai);
            _activeAgents.Add(ai);
        }
        #endregion
    }
}
