using System.Collections;
using System.Collections.Generic;
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
    [SerializeField]
    protected float _rotationSpeed = 15f;
    protected Race _race;
    protected BaseActions _act;
    protected Action _specialAct;
    #endregion

    #region Parameters
    public State CurrentState { get => _currentState; set => _currentState = value; }
    #endregion

    #region Unity Methods
    private void Start()
    {
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
        GetComponent<PolyNavAgent>().SetDestination(new Vector2(-4, -3));
    }
    #endregion

    #region Base Actions
    public void Move(Vector2 movementVector, Vector2 turnDirection)
    {
        _act.Move(movementVector, turnDirection);
    }

    public Beverage Order()
    {
        return _act.Order();
    }

    public void Drink(/* add drink */)
    {
        _act.Drink(/* add drink */);
    }

    public void Fight(/*add enemy */)
    {
        _act.Fight(/* add enemy */);
    }

    public void PassOut()
    {
        _act.PassOut();
    }
    #endregion
}
