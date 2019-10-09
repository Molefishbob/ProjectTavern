using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField]
        [Range(0, 100)]
        private int _happiness = 50;
        private int _currentMoney = 0;
        public int _moneyToWin = 1000;
        public float _playTime = 120f;
        private List<TableInteractions> _tables = null;
        [SerializeField]
        private Transform _door = null;
        [SerializeField]
        private int _maxQueueLength = 5;
        private Customer[] _customerQueue;
        [SerializeField]
        private CustomerPool _customerPoolPrefab = null;
        private CustomerPool _spawnedCustomerPool;
        [SerializeField]
        private PukePool _pukePoolPrefab = null;
        private PukePool _spawnedPukePool;
        private ScaledOneShotTimer _levelTimer;

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

        private void EndLevel()
        {
            //TODO: end level
        }

        public Customer GetCustomer()
        {
            return _spawnedCustomerPool.GetPooledObject();
        }

        public CleanableMess GetPuke()
        {
            return _spawnedPukePool.GetPooledObject();
        }

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
                    return;
                }
            }
        }

            /// <summary>
            /// Finds the table a specific customer is sitting in.
            /// 
            /// </summary>
            /// <param name="customer">The customer that is being searched for </param>
            /// <returns>The table the customer is in.</returns>
            public TableInteractions GetTable(Customer customer)
            {

                foreach (TableInteractions table in _tables)
                {
                    foreach (Customer cust in table.Sitters)
                    {
                        if (cust.GetInstanceID() == customer.GetInstanceID()) return table;
                    }
                }

                Debug.LogError("Customer has no table");
                return null;
            }

        }

    }
