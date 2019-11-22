using UnityEngine;
using PolyNav;
using Managers;
using static Managers.AIManager;
using static Managers.BeverageManager;
using static PlayerState;

public class Customer : MonoBehaviour
{
    #region Members
    protected State _currentState;
    protected State _afterMoveState;
    [SerializeField]
    protected AIBehaviour _behaviour;
    [SerializeField, Range(0, 100), Tooltip("The percentage chance to take the preferred drink")]
    protected int _preferredDrinkChance = 85;
    protected float _minDrinkFrequency = 5f;
    protected float _maxDrinkFrequency = 10f;
    protected Drink _currentDrink;
    protected Holdables _currentHoldable;
    protected int _sipsCount = 0;
    protected int _drunknessPercentage;
    protected Race _race;
    protected BaseActions _act;
    protected Action _specialAct;
    protected PolyNavAgent _polyNav;
    protected int _beverageAmount;
    protected Vector3 _movePos;
    protected Vector3 _correctStartPos;
    protected ScaledOneShotTimer _drinkTimer;
    protected ScaledOneShotTimer _positionCorrectiontimer;
    protected MyOrder _order;
    protected Customer _fightOpponent;
    protected float _passOutDist;
    [SerializeField]
    private TMPro.TextMeshProUGUI _orderText = null;
    #endregion

    #region Properties
    public State CurrentState { get => _currentState; }
    public State NextState { get => _afterMoveState; }
    public AIBehaviour AIBehaviour { get => _behaviour; }
    public MyOrder AIOrder { get => _order; }
    public Customer FightOpponent { get => _fightOpponent; }
    public int Drunkness { get => _drunknessPercentage; }
    public float DrinkTimerElapsed { get => _drinkTimer.TimeLeft; }
    public bool DrinkTimerRunning { get => _drinkTimer.IsRunning; }
    #endregion

    #region Unity Methods
    private void Awake()
    {
        _beverageAmount = System.Enum.GetNames(typeof(Beverage)).Length;
        _polyNav = GetComponent<PolyNavAgent>();
        _drinkTimer = gameObject.AddComponent<ScaledOneShotTimer>();
        _positionCorrectiontimer = gameObject.AddComponent<ScaledOneShotTimer>();
        _drinkTimer.OnTimerCompleted += TimeToDrink;
        _polyNav.OnDestinationReached += CorrectPosition;
        _polyNav.OnDestinationReached += AfterMoveActions;

        _race = _behaviour._race;
        if (typeof(BaseActions) == _behaviour._actions[0].GetType())
        {
            _act = (BaseActions)_behaviour._actions[0];
            if (_behaviour._actions.Length > 1)
                _specialAct = _behaviour._actions[1];
        }
        else if (_behaviour._actions.Length == 1)
        {
            Debug.LogError("Character is missing BaseActions!");
        }
        else
        {
            _act = (BaseActions)_behaviour._actions[1];
            _specialAct = _behaviour._actions[0];
        }

        if (_orderText == null)
        {
            Debug.LogError("Order text has not been set in!");
        }

        _orderText.text = "";
    }

    private void Update()
    {
        // Fix the position more smoothly
        if (_positionCorrectiontimer.IsRunning && CurrentState != State.PassedOut)
        {
            // Easing out for nice stopping
            float t = _positionCorrectiontimer.NormalizedTimeElapsed;
            transform.position = Vector3.Lerp(_correctStartPos, _movePos, (--t) * t * t + 1);
        } else if (CurrentState == State.PassedOut)
        {
            _positionCorrectiontimer.StopTimer();
        }
    }

    private void OnDestroy()
    {
        _polyNav.OnDestinationReached -= CorrectPosition;
        _polyNav.OnDestinationReached -= AfterMoveActions;
        _drinkTimer.OnTimerCompleted -= TimeToDrink;
        _polyNav.OnAlertDistance -= TimeToPassOut;
    }
    #endregion

    #region Struct
    public struct MyOrder
    {
        public Holdables _order;
        public Beverage _drinkOrder;

        public MyOrder(Holdables foodOrder = Holdables.Nothing, Beverage drinkOrder = Beverage.None)
        {
            _drinkOrder = drinkOrder;

            if (_drinkOrder != Beverage.None)
                _order = Holdables.Drink;
            else
                _order = foodOrder;
        }
    }
    #endregion

    #region Base Actions
    /// <summary>
    /// Sets the AIs state to Moving and
    /// uses base actions to move.
    /// </summary>
    /// <param name="pos"></param>
    public void Move(Vector3 pos)
    {
        _movePos = pos;
        _currentState = State.Moving;
        _act.Move(_polyNav, pos);
        _orderText.text = "Moving";
    }


    /// <summary>
    /// Decides on a drink to order and informs it
    /// visually to the player.
    /// </summary>
    /// <returns>The ordered beverage</returns>
    public void Order()
    {
        Beverage drinkOrder = Beverage.None;
        Holdables foodOrder = Holdables.Nothing;
        int random1 = Random.Range(1, 101);

        if (random1 >= 75)
        {
            foodOrder = Holdables.Food;
            _orderText.text = "FUD";
        }
        else
        {
            int random = Random.Range(1, 101);
            if (random <= _preferredDrinkChance && LevelManager.Instance.BeverageAvailable(_behaviour._race._preferredBeverage))
            {
                drinkOrder = _behaviour._race._preferredBeverage;
            }
            else
            {
                drinkOrder = LevelManager.Instance.RandomPossibleDrink()._drink;
            }
            _orderText.text = "D\\" + drinkOrder.ToString()[0] + drinkOrder.ToString()[1];
        }
        _currentState = State.Ordered;
        _order = new MyOrder(foodOrder, drinkOrder);
    }

    /// <summary>
    /// Fixes the AIs position to be exact
    /// </summary>
    private void CorrectPosition()
    {
        if (CurrentState == State.PassedOut) return;
        _correctStartPos = transform.position;
        _positionCorrectiontimer.StartTimer(0.5f);
    }

    /// <summary>
    /// Used when another action interrupts an action.
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(State newState)
    {
        _currentState = newState;
        if (_drinkTimer.IsRunning)
            _drinkTimer.StopTimer();

        if (_currentState == State.Fighting)
            _orderText.text = "Bullied!";
    }

    /// <summary>
    /// This method is called after completing a task, once a drink has been ordered
    /// 
    /// Decides what the character is going to do depending on how drunk they are
    /// and according to their other traits
    /// </summary>
    public void DecideDrunkAction()
    {
        int fightRoll = Mathf.RoundToInt(Random.Range(0f, 20f) + _race._agressiveness);
        int orderRoll = Mathf.RoundToInt(Random.Range(0f, 20f) + LevelManager.Instance.Happiness / 10f);
        int passOutRoll = Mathf.RoundToInt(Random.Range(0f, 20f) + _drunknessPercentage / 10f);
        int leaveRoll = 0;

        //if customer has a glass, do things
        Glass glass = gameObject.GetComponentInChildren<Glass>();
        if (glass != null)
        {
            glass.transform.parent = null;
            glass.transform.position = transform.position + new Vector3(0, 0.3f, 0);
            glass.GetComponent<CircleCollider2D>().enabled = true;
            glass._isDirty = true;

        }

        if (_drunknessPercentage > 20 && LevelManager.Instance.Happiness > 20)
        {
            leaveRoll = Mathf.RoundToInt(Random.Range(0f, 20f) + _drunknessPercentage / 10f);
        }

        if (fightRoll > orderRoll && fightRoll > passOutRoll && fightRoll > leaveRoll)
        {
            Customer opp = LevelManager.Instance.GetTable(this).GetOpponent(this);
            Fight(opp);
        }
        else if (orderRoll > fightRoll && orderRoll > passOutRoll && orderRoll > leaveRoll)
            Order();
        else if (passOutRoll > fightRoll && passOutRoll > orderRoll && passOutRoll > leaveRoll)
            PassOut();
        else
            Leave(LevelManager.Instance.Exit);

    }

    /// <summary>
    /// Called when the players serve a drink to the customer
    /// </summary>
    /// <param name="drink">The drink being served</param>
    /// <returns>true if the correct drink, otherwise false</returns>
    public bool Served(Drink drink)
    {
        if (drink == null || drink._drink != _order._drinkOrder) return false;

        LevelManager.Instance.ItemSold(drink);
        _currentDrink = drink;
        _currentHoldable = Holdables.Drink;
        _currentState = State.Served;
        Consume();
        _order._drinkOrder = Beverage.None;

        return true;
    }

    /// <summary>
    /// Called when the player serves food to the custmers
    /// </summary>
    /// <param name="food">The food</param>
    /// <returns>True if customer wanted food, otherwise false</returns>
    public bool Served(Holdables food)
    {
        if (_order._order != Holdables.Food || food != Holdables.Food) return false;

        LevelManager.Instance.ItemSold(food);
        _currentHoldable = Holdables.Food;
        _currentState = State.Served;
        Consume();
        _order._order = Holdables.Nothing;
        return true;
    }

    /// <summary>
    /// The AI calls this method shortly after receiving the drink
    /// Different drinks take longer to drink
    /// </summary>
    public void Consume()
    {
        _drinkTimer.StartTimer(Random.Range(_minDrinkFrequency, _maxDrinkFrequency));
        _orderText.text = "Consuming!";
    }

    protected void TimeToDrink()
    {
        if (_currentDrink != null)
        {
            _sipsCount++;
            float alcoholContent = _currentDrink._alcoholContent / _currentDrink._amountOfUses;
            int temp = Mathf.RoundToInt(alcoholContent - (alcoholContent * _race._alcoholTolerance / 10));

            _drunknessPercentage += temp;
        }

        if (_currentDrink == null || _sipsCount >= _currentDrink._amountOfUses)
        {
            StopDrinking();
        }
        else
        {
            Consume();
        }
    }

    protected void StopDrinking()
    {
        _currentDrink = null;
        _currentHoldable = Holdables.Nothing;
        _drinkTimer.StopTimer();
        _sipsCount = 0;
        DecideDrunkAction();
    }

    /// <summary>
    /// Start a fight against an opponent
    /// </summary>
    /// <param name="opponent">the opponent the ai is fighting against</param>
    public void Fight(Customer opponent)
    {
        _currentState = State.Fighting;
        _fightOpponent = opponent;
        opponent._fightOpponent = this;
        _orderText.text = "Fight!";
        _act.Fight(opponent);
    }

    /// <summary>
    /// Prepare to pass out
    /// </summary>
    public void PassOut()
    {
        _passOutDist = Vector2.Distance(LevelManager.Instance.Entrance.transform.position, transform.position) * (float)(Random.Range(0.1f, 0.9f));
        Leave(LevelManager.Instance.Exit);
        _afterMoveState = State.PassedOut;
        _polyNav.alertDistance = _passOutDist;
        _polyNav.OnAlertDistance += TimeToPassOut;
    }

    private void TimeToPassOut()
    {
        CleanableMess puke = LevelManager.Instance.GetPuke();
        puke.transform.position = transform.position + new Vector3(Random.Range(0f, 0.5f), Random.Range(0f, 0.5f));

        _polyNav.Stop();
        _polyNav.OnAlertDistance -= TimeToPassOut;
        _orderText.text = "Passed Out!";
    }

    /// <summary>
    /// Called when the ai is given a seat to sit at
    /// </summary>
    /// <param name="trans">the position of the seat</param>
    public void Sit(Transform trans)
    {
        _afterMoveState = State.Ordered;
        Move(trans.position);
    }

    public void GetInLine(Transform trans)
    {
        _afterMoveState = State.Waiting;
        Move(trans.position);
    }

    public void Leave(Transform trans)
    {
        if (_currentState != State.PassedOut)
            LevelManager.Instance.GetTable(this).RemoveCustomer(this);
        _afterMoveState = State.None;
        Move(trans.position);
    }

    private void AfterMoveActions()
    {
        switch (_afterMoveState)
        {
            case State.None:
                AIManager.Instance.RemoveCustomer(this);
                break;
            case State.Moving:
                _currentState = State.Moving;
                break;
            case State.Waiting:
                _currentState = State.Waiting;
                break;
            case State.Served:
                _currentState = State.Served;
                break;
            case State.PassedOut:
                _currentState = State.PassedOut;
                break;
            case State.Fighting:
                _currentState = State.Fighting;
                break;
            case State.Ordered:
                _currentState = State.Ordered;
                Order();
                break;
            default:
                break;
        }
        _afterMoveState = State.None;
    }

    #endregion

    #region DEBUGACTIONS

    public void OrderOverride()
    {
        Order();
    }

    #endregion
}
