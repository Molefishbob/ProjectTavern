using UnityEngine;
using PolyNav;
using static Managers.AIManager;
using static Managers.BeverageManager;

public class Customer : MonoBehaviour
{
    #region Members
    protected State _currentState;
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
    protected ScaledOneShotTimer _drinkTimer;
    #endregion

    #region Properties
    public State CurrentState { get => _currentState; }
    public AIBehaviour AIBehaviour { get => _behaviour; }
    #endregion

    #region Unity Methods
    private void Awake()
    {
        _beverageAmount = System.Enum.GetNames(typeof(Beverage)).Length;
        _polyNav = GetComponent<PolyNavAgent>();
        _drinkTimer = gameObject.AddComponent<ScaledOneShotTimer>();
        _drinkTimer.OnTimerCompleted += TimeToDrink;

        _polyNav.OnDestinationReached += CorrectPosition;
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
    }

    private void OnDestroy()
    {
        _polyNav.OnDestinationReached -= CorrectPosition;
        _drinkTimer.OnTimerCompleted -= TimeToDrink;
    }
    #endregion

    #region Base Actions
    /// <summary>
    /// Sets the AIs state to Moving and
    /// Uses base actions to move.
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
    /// <returns></returns>
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
        return order;
    }

    /// <summary>
    /// Fixes the AIs position to be exact
    /// </summary>
    private void CorrectPosition()
    {
        transform.position = _movePos;
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
        int orderRoll = Mathf.RoundToInt(Random.Range(0f, 20f)/*+ overallhappiness multiplier*/);
        int passOutRoll =Mathf.RoundToInt(Random.Range(0f, 20f) + _drunknessPercentage / 100f);

        if (fightRoll > orderRoll && fightRoll > passOutRoll)
            Fight(opponent: null /*TODO: GET opponent*/);
        else if (orderRoll > fightRoll && orderRoll > passOutRoll)
            Order();
        else
            PassOut();
    }

    /// <summary>
    /// Called when the players serve the correct drink to the AI
    /// </summary>
    /// <param name="drink">The ordered drink</param>
    public void Served(Drink drink)
    {
        _currentDrink = drink;
        _currentState = State.Served;
        Drink();
    }

    /// <summary>
    /// The AI calls this method shortly after receiving the drink
    /// Different drinks take longer to drink
    /// </summary>
    public void Drink()
    {
        _drinkTimer.StartTimer(Random.Range(_minDrinkFrequency, _maxDrinkFrequency));
    }

    protected void TimeToDrink()
    {
        _sipsCount++;
        float alcoholContent = _currentDrink._alcoholContent;
        int temp = Mathf.RoundToInt(alcoholContent * (_race._alcoholTolerance / 10));
        if ((float)temp < (float)alcoholContent / 2)
        {
            temp += Mathf.RoundToInt(alcoholContent + alcoholContent * 0.2f);
        }

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
        // TODO: remove drink from hand
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
        // Also add excrement reflex
    }

    /// <summary>
    /// Called when the ai is given a seat to sit at
    /// </summary>
    /// <param name="trans">the position of the seat</param>
    public void Sit(Transform trans)
    {
        _currentState = State.Waiting;
        Move(trans.position);
    }
    #endregion
}
