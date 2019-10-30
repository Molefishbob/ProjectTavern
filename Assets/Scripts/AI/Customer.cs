﻿using UnityEngine;
using PolyNav;
using Managers;
using static Managers.AIManager;
using static Managers.BeverageManager;

public class Customer : MonoBehaviour
{
    #region Members
    protected State _currentState;
    protected State _afterMoveState;
    [SerializeField]
    protected AIBehaviour _behaviour;
    [SerializeField, Range(0, 100), Tooltip("The percentage chance to take the preferred drink")]
    protected int _preferredDrinkChance = 85;
    protected float _minDrinkFrequency = 15f;
    protected float _maxDrinkFrequency = 30f;
    protected Drink _currentDrink;
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
    protected Beverage _orderedDrink;
    private bool _hasBeenServed = false;
    [SerializeField]
    private TMPro.TextMeshProUGUI _orderText = null;
    #endregion

    #region Properties
    public State CurrentState { get => _currentState; }
    public State NextState { get => _afterMoveState; }
    public AIBehaviour AIBehaviour { get => _behaviour; }
    public Beverage OrderedDrink { get => _orderedDrink; }
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
        if (_positionCorrectiontimer.IsRunning)
        {
            // Easing out for nice stopping
            float t = _positionCorrectiontimer.NormalizedTimeElapsed;
            transform.position = Vector3.Lerp(_correctStartPos, _movePos, (--t) * t * t + 1);
        }
    }

    private void OnDestroy()
    {
        _polyNav.OnDestinationReached -= CorrectPosition;
        _polyNav.OnDestinationReached -= AfterMoveActions;
        _drinkTimer.OnTimerCompleted -= TimeToDrink;
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
    }


    /// <summary>
    /// Decides on a drink to order and informs it
    /// visually to the player.
    /// </summary>
    /// <returns>The ordered beverage</returns>
    public Beverage Order()
    {
        Beverage order = Beverage.None;
        int random = Random.Range(1, 101);
        if (random <= _preferredDrinkChance)
        {
            order = _behaviour._race._preferredBeverage;
        }
        else
        {
            int ran = Random.Range(1, _beverageAmount + 1);
            order = (Beverage)ran;
        }
        _currentState = State.Ordered;
        _orderedDrink = order;
        _orderText.text = "D\\" + _orderedDrink.ToString()[0];
        return order;
    }

    /// <summary>
    /// Fixes the AIs position to be exact
    /// </summary>
    private void CorrectPosition()
    {
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
            _orderedDrink = Order();
        else if (passOutRoll > fightRoll && passOutRoll > orderRoll && passOutRoll > leaveRoll)
            PassOut();
        else
            Leave(LevelManager.Instance.Door);
 
    }

    /// <summary>
    /// Called when the players serve a drink to the customer
    /// </summary>
    /// <param name="drink">The drink being served</param>
    /// <returns>true if the correct drink, otherwise false</returns>
    public bool Served(Drink drink)
    {
        if (drink._drink != _orderedDrink) return false;

        _currentDrink = drink;
        _currentState = State.Served;
        Drink();
        _orderedDrink = Beverage.None;
        _hasBeenServed = true;

        //DEBUG
        //Leave(LevelManager.Instance.Door);

        return true;
    }

    /// <summary>
    /// The AI calls this method shortly after receiving the drink
    /// Different drinks take longer to drink
    /// </summary>
    public void Drink()
    {
        _drinkTimer.StartTimer(Random.Range(_minDrinkFrequency, _maxDrinkFrequency));
        _orderText.text = "Drinking!";
    }

    protected void TimeToDrink()
    {
        _sipsCount++;
        float alcoholContent = _currentDrink._alcoholContent / _currentDrink._amountOfUses;
        int temp = Mathf.RoundToInt(alcoholContent - (alcoholContent * _race._alcoholTolerance / 10));

        _drunknessPercentage += temp;
        if (_sipsCount >= _currentDrink._amountOfUses)
        {
            StopDrinking();
        }
        else
        {
            Drink();
        }
    }

    protected void StopDrinking()
    {
        _currentDrink = null;
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
        _act.Fight(opponent);
    }

    /// <summary>
    /// Prepare to pass out
    /// </summary>
    public void PassOut()
    {
        _currentState = State.PassedOut;
        // TODO: PASS OUT
        CleanableMess puke = LevelManager.Instance.GetPuke();
        puke.transform.parent = this.transform;
        puke.transform.position = Vector3.zero;

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
        LevelManager.Instance.GetTable(this).RemoveCustomer(this);
        _afterMoveState = State.None;
        Move(trans.position);
    }

    /// <summary>
    /// What to do in addition of the correction manuevers
    /// </summary>
    private void AfterMoveActions()
    {
        switch (_afterMoveState)
        {
            case State.None:
                AIManager.Instance.RemoveCustomer(this);
                break;
            case State.Moving:
                break;
            case State.Waiting:
                break;
            case State.Served:
                break;
            case State.PassedOut:
                break;
            case State.Fighting:
                break;
            case State.Ordered:
                _orderedDrink = Order();
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
        _orderedDrink = Order();
    }

    #endregion
}
