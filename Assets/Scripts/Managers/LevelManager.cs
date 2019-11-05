﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField]
        [Range(-50, 50)]
        private int _happiness = 50;
        private int _currentMoney = 0;
        private int _tipsGained = 0;
        public int _moneyToWin = 1000;
        public float _playTime = 120f;
        private List<TableInteractions> _tables = null;
        [SerializeField]
        private Transform _door = null;
        public int _maxQueueLength = 5;
        private Customer[] _customerQueue;
        [SerializeField]
        private CustomerPool _customerPoolPrefab = null;
        private CustomerPool _spawnedCustomerPool;
        [SerializeField]
        private PukePool _pukePoolPrefab = null;
        private PukePool _spawnedPukePool;
        private ScaledOneShotTimer _levelTimer;
        [SerializeField]
        private float _spawnInterval = 5;
        [SerializeField]
        private float _spawnOffset = 5;
        [HideInInspector]
        public Queue _queue;

        public List<TableInteractions> Tables { get { return _tables; } }

        public Customer[] CustomerQueue { get { return _customerQueue; } }

        public Transform Door { get { return _door; } }

        public int CurrentMoney { get { return _currentMoney; } }

        public int Happiness { get { return _happiness; } }

        public static LevelManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                GameManager.Instance.LevelManager = this;
            }
            else
            {
                Destroy(gameObject);
            }
            _customerQueue = new Customer[_maxQueueLength];
            _spawnedCustomerPool = Instantiate(_customerPoolPrefab);
            _spawnedPukePool = Instantiate(_pukePoolPrefab);
            _tables = new List<TableInteractions>();
            _tables.AddRange(FindObjectsOfType<TableInteractions>());
            _queue = FindObjectOfType<Queue>();
            if (GameObject.Find("Door") != null)
            {
                _door = GameObject.Find("Door").transform;
            }
            else
            {
                Debug.LogError("No door found.");
            }
            _levelTimer = gameObject.AddComponent<ScaledOneShotTimer>();
            _levelTimer.OnTimerCompleted += EndLevel;

        }

        private void OnDestroy()
        {
            _levelTimer.OnTimerCompleted -= EndLevel;
        }

        private void Start()
        {
            _levelTimer.StartTimer(_playTime);
        }

        private void Update()
        {
            if (CanSpawnAi() && !AIManager.Instance._timer.IsRunning)
            {
                AIManager.Instance.StartSpawning(_spawnInterval, _spawnOffset);
            }
        }

        private void EndLevel()
        {
            //TODO: end level
        }

        /// <summary>
        /// Returns first inactive customer from the customer pool.
        /// 
        /// </summary>
        /// <returns>Customer from customer pool</returns>
        public Customer GetCustomer()
        {
            return _spawnedCustomerPool.GetPooledObject();
        }

        /// <summary>
        /// If you need some puke from the puke pool, you can get it here.
        /// 
        /// </summary>
        /// <returns>Puke from the puke pool</returns>
        public CleanableMess GetPuke()
        {
            return _spawnedPukePool.GetPooledObject();
        }

        /// <summary>
        /// Calculates the earned money from selling a drink.
        /// 
        /// Earned money is increased or decreased by tip depending on its value.
        /// </summary>
        /// <param name="drink">The sold drink</param>
        public void DrinkBought(Drink drink)
        {
            int price = drink._price;
            int tip = Mathf.RoundToInt(drink._price * (_happiness / 100));
            _tipsGained += tip;
            _currentMoney += price + tip;

            if (tip >= 0)
            {
                Debug.Log("Money gained: " + price + " + " + tip);
            } else
            {
                Debug.Log("Money gained: " + price + " - " + tip);
            }
        }


        /// <summary>
        /// Checks all tables for free seats. If seat is found customer moves to the firs available seat. If no seat is found checks for a free queue spot.
        /// If no free seats or queue spots are found, deactivates customer.
        /// 
        /// </summary>
        /// <param name="ai"></param>
        public void GetSeat(Customer ai)
        {
            for (int a = 0; a < _tables.Count; a++)
            {
                if (_tables[a].Use(ai)) return;
            }
            for (int a = 0; a < _maxQueueLength; a++)
            {
                if (_customerQueue[a] == null)
                {
                    _customerQueue[a] = ai;
                    _queue.GoToQueue(ai);

                    return;
                }
            }

            AIManager.Instance.RemoveCustomer(ai);

        }


        /// <summary>
        /// Checks if there is a free seat for the customer in the queue. 
        /// If free seat is found customer leaves queue and the rest of the queued customers are rearranged.
        /// </summary>
        /// <param name="ai"></param>
        /// <returns>True if a free seat is found. Oterwise false </returns>
        public bool LeaveQueue(Customer ai)
        {
            for (int i = 0; i < _tables.Count; i++)
            {
                if (_tables[i].Use(ai))
                {
                    _queue._queuedCustomers.RemoveAt(0);
                    _customerQueue[0] = null;
                    for (int a = 0; a < _customerQueue.Length; a++)
                    {
                        if (_customerQueue[i] == null)
                        {
                            _customerQueue[i] = _customerQueue[i + 1];
                            _customerQueue[i + 1] = null;
                        }

                    }
                    return true;
                }
            }
            return false;

        }

        /// <summary>
        /// Finds the table a specific customer is sitting in.
        /// 
        /// </summary>
        /// <param name="customer">The customer that is being searched for </param>
        /// <returns>The table the customer is in.</returns>
        public TableInteractions GetTable(Customer customer)
        {
            if (customer == null)
                return null;

            foreach (TableInteractions table in _tables)
            {
                foreach (Customer cust in table.Sitters)
                {
                    if (cust == null) continue;
                    if (cust.GetInstanceID() == customer.GetInstanceID()) return table;
                }
            }

            Debug.LogError("Customer has no table");
            return null;
        }


        /// <summary>
        /// Checks if a new customer can be spawned if there are any seats in the tables. If no seats are found chekc if there are free spots in queue.
        /// </summary>
        /// <returns>True if there are seats in any table or free spots in queue. Otherwise returns false</returns>
        private bool CanSpawnAi()
        {
            for (int i = 0; i < _tables.Count; i++)
            {
                if (_tables[i]._currentState != TableInteractions.TableState.Full)
                {
                    return true;
                }
            }

            for (int i = 0; i < _customerQueue.Length; i++)
            {
                if (CustomerQueue[i] == null)
                {
                    return true;
                }
            }

            return false;
        }
    }

}