using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    #region Members
    protected AIAction.State _currentState;
    [SerializeField]
    protected Race _race;
    [SerializeField]
    protected float _movementSpeed;
    [SerializeField]
    protected float _turnSpeed;
    #endregion

    #region Parameters
    public AIAction.State CurrentState { get => _currentState; set => _currentState = value; }
    #endregion

    public void Move(Vector3 movementVector, float turnAngle = 0)
    {
        // TODO: Movement
        // TODO: Turning
    }

    public void Order()
    {

    }
}
