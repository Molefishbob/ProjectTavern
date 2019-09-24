using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Managers.AIManager;
using static Managers.BeverageManager;

public class Customer : MonoBehaviour
{
    #region Members
    protected State _currentState;
    [SerializeField]
    protected AIBehaviour _behaviour;
    [SerializeField]
    protected float _movementSpeed;
    [SerializeField]
    protected float _turnSpeed;
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
            _act = (BaseActions) _behaviour._actions[1];
            _specialAct = _behaviour._actions[0];
        }
    }
    #endregion

    public void Move(Vector3 movementVector, float turnAngle)
    {
        // TODO: Use move method from behaviour
    }

    public Beverage Order()
    {
        // TODO: Use Order method from behaviour
        return 0;
    }
}
