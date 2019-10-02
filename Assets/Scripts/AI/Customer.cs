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
    protected Race _race;
    protected BaseActions _act;
    protected Action _specialAct;
    protected PolyNavAgent _polyNav;
    protected int _beverageAmount;
    protected Vector3 _movePos;
    #endregion

    #region Properties
    public State CurrentState { get => _currentState; }
    public AIBehaviour AIBehaviour { get => _behaviour; }
    public State SetState { set => _currentState = value; }
    #endregion

    #region Unity Methods
    private void Awake()
    {
        _beverageAmount = System.Enum.GetNames(typeof(Beverage)).Length;
        _polyNav = GetComponent<PolyNavAgent>();

        _polyNav.OnDestinationReached += CorrectPosition;
        _race = _behaviour._race;
        if (typeof(BaseActions) == _behaviour._actions[0].GetType())
        {
            _act = (BaseActions)_behaviour._actions[0];
            if (_behaviour._actions.Length > 1)
                _specialAct = _behaviour._actions[1];
        } else if (_behaviour._actions.Length == 1)
        {
            Debug.LogError("Character is missing BaseActions !");
        }
        else
        {
            _act = (BaseActions)_behaviour._actions[1];
            _specialAct = _behaviour._actions[0];
        }
    }

    private void Start()
    {

    }

    private void OnDestroy()
    {
        _polyNav.OnDestinationReached -= CorrectPosition;
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
        SetState = State.Ordered;
        return order;
    }

    /// <summary>
    /// Fixes the AIs position to be exact.
    /// </summary>
    private void CorrectPosition()
    {
        transform.position = _movePos;
    }

    /// <summary>
    /// Called when the players serve the correct drink to the AI
    /// </summary>
    public void Served()
    {
        SetState = State.Served;
        // TODO: Start drinking
    }

    /// <summary>
    /// The AI calls this method shortly after receiving the drink
    /// Different drinks take longer to drink
    /// </summary>
    /// <param name="drink"></param>
    public void Drink(Beverage drink)
    {
        // TODO: Drink the drink
    }

    /// <summary>
    /// Start a fight against an opponent
    /// </summary>
    /// <param name="opponent"></param>
    public void Fight(Customer opponent)
    {
        SetState = State.Fighting;
        _act.Fight(opponent);
    }

    /// <summary>
    /// Prepare to pass out
    /// </summary>
    public void PassOut()
    {
        SetState = State.PassedOut;
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
