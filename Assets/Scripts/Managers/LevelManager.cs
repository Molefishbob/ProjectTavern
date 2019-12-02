using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField]
        [Range(-50, 50)]
        private int _happiness = 0;
        private int _currentMoney = 0;
        private int _tipsGained = 0;
        public int _moneyToWin = 1000;
        public float _playTime = 120f;
        public event ValueChangedFloat OnHappinessChanged;
        public event ValueChangedFloat OnMoneyChanged;
        private int _pukeAmount = 0;
        private List<TableInteractions> _glassTables = new List<TableInteractions>();
        private List<Customer> _passedOut = new List<Customer>();
        private List<TableInteractions> _tables = null;
        [SerializeField]
        protected int _cleanlinessHappinessMod = 5, _fighthappinessMod = 5, _difficulty = 2;
        [SerializeField]
        protected GetIngredient[] _barrels;
        private List<Drink> _possibleDrinks = new List<Drink>();
        [SerializeField]
        private Transform _entrance = null, _exit = null;
        private int _maxQueueLength = 0;
        private Customer[] _customerQueue;
        [SerializeField]
        protected InGameUI _ui;
        [SerializeField]
        private CustomerPool _customerPoolPrefab = null;
        private CustomerPool _spawnedCustomerPool;
        [SerializeField]
        private CustomerPool _humanCustomerPoolPrefab = null;
        private CustomerPool _spawnedHumanCustomerPool;
        [SerializeField]
        private CustomerPool _elfCustomerPoolPrefab = null;
        private CustomerPool _spawnedElfCustomerPool;
        [SerializeField]
        private CustomerPool _dwarfCustomerPoolPrefab = null;
        private CustomerPool _spawnedDwarfCustomerPool;
        [SerializeField]
        private PukePool _pukePoolPrefab = null;
        private PukePool _spawnedPukePool;
        [SerializeField]
        private GlassPool _glassPoolPrefab = null;
        private GlassPool _spawnedGlassPool;
        [SerializeField]
        private SfxSoundPool _sfxSoundPoolPrefab = null;
        private SfxSoundPool _spawnedSfxSoundPool;
        [SerializeField]
        protected EndGameMenu _endGameMenu;
        private ScaledOneShotTimer _levelTimer;
        [SerializeField]
        protected float _happInterval = 10f;
        private ScaledRepeatingTimer _conHappinessTimer;
        [SerializeField]
        private float _spawnInterval = 5;
        [SerializeField]
        private float _spawnOffset = 5;
        [HideInInspector]
        public Queue _queue;
        public int _fightUnhappiness = 5;
        public int _correctDrinkHappiness = 3;
        public int _wrongDrinkUnhappiness = 10;
        public int _dirtyGlassUnhappiness = 5;

        public List<TableInteractions> Tables { get { return _tables; } }
        public int PukeAmount { get { return _pukeAmount; } set { _pukeAmount = Mathf.Clamp(value, 0, 255); } }
        public Customer[] CustomerQueue { get { return _customerQueue; } }
        public List<Drink> PossibleDrinks => _possibleDrinks;

        public Transform Entrance { get { return _entrance; } }
        public string LevelEndText { get; set; }
        public Transform Exit { get { return _exit; } }
        public float PlayTime { get { return _playTime; } }
        public ScaledOneShotTimer LevelTime { get { return _levelTimer; } }
        public int CurrentMoney
        {
            get
            {
                return _currentMoney;
            }
            set
            {
                _currentMoney = value;
                OnMoneyChanged?.Invoke(_currentMoney);
            }
        }
        public int Happiness
        {
            get
            {
                return _happiness;
            }
            set
            {
                _happiness = Mathf.Clamp(value, -50, 50);
                OnHappinessChanged?.Invoke(_happiness);
            }
        }

        private void OnValidate()
        {
            GetIngredient[] barr = FindObjectsOfType<GetIngredient>();

            if (barr == null) return;

            bool hasChanged = false;
            for (int a = 0; _barrels != null && a < _barrels.Length && !hasChanged; a++)
            {
                if (_barrels.Length != barr.Length)
                {
                    hasChanged = true;
                    Debug.Log("Barrel list updated");
                    break;
                }
                bool old = false;
                for (int b = 0; b < barr.Length; b++)
                {
                    if (_barrels[a] == barr[b])
                    {
                        old = true;
                    }
                }
                if (!old)
                {
                    Debug.Log("Barrel list updated");
                    break;
                }
            }
            for (int a = 0; _barrels != null && a < barr.Length && !hasChanged; a++)
            {
                bool isss = false;
                for (int b = 0; b < _barrels.Length; b++)
                {
                    if (barr[a] == _barrels[b])
                    {
                        isss = true;
                    }
                }
                if (!isss)
                {
                    Debug.Log("Barrel list updated");
                    break;
                }
            }

            if (_barrels == null) Debug.Log("Barrel list updated");
            _barrels = barr;
        }

        public static LevelManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;

                if (GameManager.Instance != null)
                    GameManager.Instance.LevelManager = this;
                else
                    Debug.LogError("GameManagerException: GameManager is missing");
            }
            else
            {
                Destroy(gameObject);
            }

            _spawnedCustomerPool = Instantiate(_customerPoolPrefab);
            _spawnedHumanCustomerPool = Instantiate(_humanCustomerPoolPrefab);
            _spawnedElfCustomerPool = Instantiate(_elfCustomerPoolPrefab);
            _spawnedDwarfCustomerPool = Instantiate(_dwarfCustomerPoolPrefab);
            _spawnedPukePool = Instantiate(_pukePoolPrefab);
            _spawnedGlassPool = Instantiate(_glassPoolPrefab);
            _spawnedSfxSoundPool = Instantiate(_sfxSoundPoolPrefab);
            _tables = new List<TableInteractions>();
            _tables.AddRange(FindObjectsOfType<TableInteractions>());
            _queue = FindObjectOfType<Queue>();
            GameObject entrance = GameObject.Find("Entrance");
            if (entrance != null)
            {
                _entrance = GameObject.Find("Entrance").transform;
            }
            else
            {
                Debug.LogError("No entrance found.");
            }
            GameObject exit = GameObject.Find("Exit");
            if (exit != null)
            {
                _exit = GameObject.Find("Exit").transform;
            }
            else
            {
                Debug.LogError("No Exit found.");
            }
            _levelTimer = gameObject.AddComponent<ScaledOneShotTimer>();
            _conHappinessTimer = gameObject.AddComponent<ScaledRepeatingTimer>();

        }

        private void OnDestroy()
        {
            _levelTimer.OnTimerCompleted -= EndLevel;
            _conHappinessTimer.OnTimerCompleted -= CheckHappinessModifier;
        }

        private void Start()
        {
            _maxQueueLength = _queue.QueueLength;
            _customerQueue = new Customer[_maxQueueLength];
            _conHappinessTimer.OnTimerCompleted += CheckHappinessModifier;
            _levelTimer.OnTimerCompleted += EndLevel;
            GetAvailableDrinks();
            StartLevel();
        }

        private void CheckHappinessModifier()
        {
            int fightMod = AIManager.Instance.SearchForFighters() * _fighthappinessMod;
            if (fightMod == 0) fightMod = -(_fighthappinessMod / _difficulty);
            int cleanliness = _passedOut.Count + _pukeAmount + _glassTables.Count * _cleanlinessHappinessMod;
            if (cleanliness == 0) cleanliness = -(_cleanlinessHappinessMod / _difficulty);
            int temp = fightMod + cleanliness;
            Debug.Log("happiness changed! " + -temp);
            Happiness -= temp;
        }

        private void Update()
        {
            if (CanSpawnAi() && !AIManager.Instance._timer.IsRunning)
            {
                AIManager.Instance.StartSpawning(_spawnInterval, _spawnOffset);
            }
        }

        private void GetAvailableDrinks()
        {
            List<IngredientManager.DrinkIngredient> ingredients = new List<IngredientManager.DrinkIngredient>();
            List<Drink> drinks = new List<Drink>();
            foreach (GetIngredient barrel in _barrels)
            {
                ingredients.Add(barrel._ingredient);
            }

            foreach (Drink drink in Resources.LoadAll<Drink>("Drinks"))
            {
                drinks.Add(drink);
            }

            for (int a = 0; a < drinks.Count; a++)
            {
                int count = 0;
                for (int b = 0; b < drinks[a]._ingredients.Count; b++)
                {
                    for (int c = 0; c < ingredients.Count; c++)
                    {
                        if (drinks[a]._ingredients[b] == ingredients[c])
                        {
                            count++;
                        }
                    }
                }
                if (count == drinks[a]._ingredients.Count) _possibleDrinks.Add(drinks[a]);
            }
        }

        /// <summary>
        /// Checks if the given beverage is possible to make in the level.
        /// </summary>
        /// <param name="bev">The given beverage</param>
        /// <returns>True if possible to make, false if not.</returns>
        public bool BeverageAvailable(BeverageManager.Beverage bev)
        {
            foreach (Drink drink in _possibleDrinks)
            {
                if (drink._drink == bev) return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if the given Drink is possible to make in the level.
        /// </summary>
        /// <param name="bev">The given drink</param>
        /// <returns>True if possible to make, false if not.</returns>
        public bool BeverageAvailable(Drink bev)
        {
            foreach (Drink drink in _possibleDrinks)
            {
                if (drink._drink == bev._drink) return true;
            }
            return false;
        }

        /// <summary>
        /// Gives a random drink that is possible to make in the level.
        /// </summary>
        /// <returns></returns>
        public Drink RandomPossibleDrink()
        {
            int ran = Random.Range(0, _possibleDrinks.Count);
            return _possibleDrinks[ran];
        }

        /// <summary>
        /// Adds the passed out customer to affect happiness
        /// </summary>
        /// <param name="cus">The customer</param>
        public void AddPassedOut(Customer cus)
        {
            foreach (Customer ai in _passedOut)
            {
                if (ai == cus)
                {
                    return;
                }
            }
            _passedOut.Add(cus);
        }

        /// <summary>
        /// Removes a passed out customer
        /// </summary>
        /// <param name="cus">The customer</param>
        public void RemovePassedOut(Customer cus)
        {
            foreach (Customer ai in _passedOut)
            {
                if (ai == cus)
                {
                    _passedOut.Remove(ai);
                    return;
                }
            }
        }

        /// <summary>
        /// Adds a table that has no spots for glasses left
        /// </summary>
        /// <param name="table">The table</param>
        public void GlassSpotsFull(TableInteractions table)
        {
            foreach (TableInteractions i in _glassTables)
            {
                if (table == i)
                {
                    return;
                }
            }
            _glassTables.Add(table);
        }

        /// <summary>
        /// Removes a table that has spots for glasses
        /// </summary>
        /// <param name="table">The table</param>
        public void RemoveGlassTable(TableInteractions table)
        {
            foreach (TableInteractions i in _glassTables)
            {
                if (table == i)
                {
                    _glassTables.Remove(i);
                    return;
                }
            }
        }

        /// <summary>
        /// Does the necessary things to get the level going
        /// </summary>
        public void StartLevel()
        {
            _conHappinessTimer.StartTimer(_happInterval);
            _levelTimer.StartTimer(_playTime);
            Debug.Log("Game time started");
        }

        private void EndLevel()
        {
            bool win = false;
            if (_currentMoney >= _moneyToWin)
            {
                LevelEndText = "WIN";
                win = true;
            }
            else
            {
                win = false;
                LevelEndText = "LOS";
            }
            GameManager.Instance.PauseGame();
            _conHappinessTimer.StopTimer();
            _levelTimer.StopTimer();
            _endGameMenu.gameObject.SetActive(true);
            _endGameMenu.ContinueButtonSettings(win);
        }

        /// <summary>
        /// Returns first inactive customer from the customer pool.
        /// 
        /// </summary>
        /// <returns>Customer from customer pool</returns>
        public Customer GetCustomer()
        {
            int customerRandomizer = 0;

            customerRandomizer = Random.Range(0, 4);

            switch (customerRandomizer)
            {
                case 0:
                    {
                        return _spawnedCustomerPool.GetPooledObject();
                    }
                case 1:
                    {
                        return _spawnedHumanCustomerPool.GetPooledObject();
                    }
                case 2:
                    {
                        return _spawnedElfCustomerPool.GetPooledObject();
                    }
                case 3:
                    {
                        return _spawnedDwarfCustomerPool.GetPooledObject();
                    }
                default:
                    Debug.Log("No customer to spawn");
                    return null;
            }

            
        }

        /// <summary>
        /// If you need some puke from the puke pool, you can get it here.
        /// 
        /// </summary>
        /// <returns>Puke from the puke pool</returns>
        public CleanableMess GetPuke()
        {
            _pukeAmount++;
            return _spawnedPukePool.GetPooledObject();
        }

        /// <summary>
        /// Used to play any sfx sound in level
        /// </summary>
        /// <returns>Returns a reference to the sfx soundplayer</returns>
        public SFXSound GetSfxPlayer()
        {
            return _spawnedSfxSoundPool.GetPooledObject();
        }

        /// <summary>
        /// Returns a new glass from glass pool;
        /// 
        /// </summary>
        /// <returns>Glass from glass pool</returns>
        public Glass GetGlass()
        {
            return _spawnedGlassPool.GetPooledObject();
        }

        /// <summary>
        /// Calculates the earned money from selling a drink.
        /// 
        /// Earned money is increased or decreased by tip depending on customer happiness.
        /// </summary>
        /// <param name="drink">The sold drink</param>
        public void ItemSold(Drink drink)
        {
            int price = drink._price;
            int tip = Mathf.RoundToInt((float)drink._price * ((float)_happiness / 100f));
            _tipsGained += tip;
            CurrentMoney += price + tip;

            if (tip >= 0)
            {
                Debug.Log("Money gained: " + price + " + " + tip);
            }
            else
            {
                Debug.Log("Money gained: " + price + " - " + tip);
            }
        }

        /// <summary>
        /// Calculates the earned money from selling food
        /// 
        /// Earned money is increased or decreased by tip depending on customer happiness.
        /// </summary>
        /// <param name="food">The sold food</param>
        public void ItemSold(PlayerState.Holdables food)
        {
            int price = 5; // TODO: Add food price somewhere
            int tip = Mathf.RoundToInt(price * (_happiness / 100));
            _tipsGained += tip;
            CurrentMoney += price + tip;

            if (tip >= 0)
            {
                Debug.Log("Money gained: " + price + " + " + tip);
            }
            else
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
                        if (_customerQueue[a] == null && CustomerQueue.Length > 1 && a != _customerQueue.Length - 1)
                        {
                            _customerQueue[a] = _customerQueue[a + 1];
                            _customerQueue[a + 1] = null;
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