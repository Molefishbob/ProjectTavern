using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;
using static Managers.AIManager;
using static Managers.BeverageManager;
using System;

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
    #endregion

    #region Properties
    public State CurrentState { get => _currentState; }
    public AIBehaviour AIBehaviour { get => _behaviour; }
    public State SetState { set => _currentState = value;}
    #endregion

    #region Unity Methods
    private void Awake()
    {
        _beverageAmount = System.Enum.GetNames(typeof(Beverage)).Length;
        _polyNav = GetComponent<PolyNavAgent>();
        _race = _behaviour._race;
        if (typeof(BaseActions) == _behaviour._actions[0].GetType())
        {
            _act = (BaseActions)_behaviour._actions[0];
            _specialAct = _behaviour._actions[1];
        }
        else
        {
            _act = (BaseActions)_behaviour._actions[1];
            _specialAct = _behaviour._actions[0];
        }
    }
    private void Start()
    {
        _polyNav.SetDestination(new Vector2(5, -3));
    }
    #endregion

    #region Base Actions
    /// <summary>
    /// Sets the AIs state to Moving and
    /// Uses base actions to move.
    /// </summary>
    /// <param name="pos"></param>
    public void Move(Vector2 pos)
    {
        _currentState = State.Moving;
        _act.Move(_polyNav,pos);
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
        return order;
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

    public void Sit(Transform trans)
    {
        _currentState = State.Waiting;
        Move(trans.position);
    }
    #endregion
}
