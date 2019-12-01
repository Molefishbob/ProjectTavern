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
        [HideInInspector]
        public ScaledOneShotTimer _timer;
        private float _duration;
        private float _offset;
        #endregion

        #region Properties
        public List<Customer> ActiveAgents { get => _activeAgents; }
        #endregion

        #region Unity Methods
        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(gameObject);
            else Instance = this;

            _timer = gameObject.AddComponent<ScaledOneShotTimer>();
            _timer.OnTimerCompleted += SpawnAI;
            _activeAgents = new List<Customer>();
            _customers = Resources.LoadAll<Customer>("Customers");
        }

        private void OnDestroy()
        {
            _timer.OnTimerCompleted -= SpawnAI;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Starts spawning AI when called
        /// </summary>
        /// <param name="interval">The interval in which the AI is spawned</param>
        /// <param name="offset">The offset of the interval</param>
        public void StartSpawning(float interval, float offset)
        {
            _offset = offset;
            _duration = interval;
            _timer.StartTimer(_duration + Random.Range(-_offset, _offset + 1));
        }

        /// <summary>
        /// Stops the spawning of the AI
        /// </summary>
        public void StopSpawning()
        {
            _timer.StopTimer();
        }

        /// <summary>
        /// Despawns a specific customer
        /// </summary>
        /// <param name="ai">The customer to be despawned</param>
        public void RemoveCustomer(Customer ai)
        {
            _activeAgents.Remove(ai);
            ai.gameObject.SetActive(false);
        }

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
            Customer ai = LevelManager.Instance.GetCustomer();
            ai.transform.position = LevelManager.Instance.Entrance.position;
            _activeAgents.Add(ai);
            LevelManager.Instance.GetSeat(ai);
        }

        /// <summary>
        /// Checks the activeAgents for fighting customers
        /// </summary>
        /// <returns>The amount of fights ongoing</returns>
        public int SearchForFighters()
        {
            int amount = 0;
            foreach (Customer cus in _activeAgents)
            {
                if (cus.CurrentState == State.Fighting)
                {
                    amount++;
                }
            }
            return amount / 2;
        }
        #endregion
    }
}
