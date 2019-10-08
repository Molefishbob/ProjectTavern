using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableInteractions : AIUseable
{
    #region Parameters
    public enum TableState
    {
        None = 0,
        Empty = 1,
        Occupied = 2,
        Full = 3
    }

    protected TableState _currentState;
    protected int _totalSeatsCount;
    private int _freeSeatsCount;
    protected Transform[] _chairs;
    protected Customer[] _sitters;
    #endregion

    #region Properties
    public Transform[] Chairs { get => _chairs; }
    public Customer[] Sitters { get => _sitters; }
    #endregion

    #region Unity Methods
    private void Awake()
    {
        _currentState = TableState.Empty;
        _totalSeatsCount = transform.childCount;
        _chairs = new Transform[_totalSeatsCount];
        for (int a = 0; a < _totalSeatsCount; a++)
        {
            _chairs[a] = transform.GetChild(a);
        }
        _freeSeatsCount = _totalSeatsCount;
        _sitters = new Customer[_totalSeatsCount];
    }
    #endregion

    #region Methods
    public override void Use() { }

    /// <summary>
    /// Adds the ai to sit on the table if there is room
    /// </summary>
    /// <param name="ai">The ai to be added to the list</param>
    /// <returns>True if gains a seat, otherwise false/returns>
    public bool Use(Customer ai)
    {
        if (_currentState == TableState.Full) return false;

        ai.Sit(_chairs[_totalSeatsCount - _freeSeatsCount]);
        _sitters[_totalSeatsCount - _freeSeatsCount] = ai;
        _freeSeatsCount--;
        if (_freeSeatsCount == 0)
        {
            _currentState = TableState.Full;
        }
        else
        {
            _currentState = TableState.Occupied;
        }
        return true;
    }

    /// <summary>
    /// Finds an opponent from the table that is not the given customer
    /// </summary>
    /// <param name="notme">The customer that wants to fight</param>
    /// <returns>An opponent</returns>
    public Customer GetOpponent(Customer notme)
    {
        for (int a = 0; a < _totalSeatsCount - _freeSeatsCount; a++)
        {
            if (_sitters[a] != notme)
            {
                return _sitters[a];
            }
        }
        Debug.LogError("No other customers on the table! Cannot start a fight");
        return null;
    }

    /// <summary>
    /// Removes a customer from the table
    /// 
    /// Removes the specified customer from the sitters array
    /// Rearranges the sitters array after
    /// </summary>
    /// <param name="ai">The ai to be removed from the table</param>
    public void RemoveCustomer(Customer ai)
    {
        _freeSeatsCount++;

        if (_freeSeatsCount != _totalSeatsCount)
            _currentState = TableState.Occupied;
        else
            _currentState = TableState.Empty;

        int me = ai.GetInstanceID();

        for (int a = 0; a < _sitters.Length; a++)
        {
            if (me == _sitters[a].GetInstanceID()) _sitters[a] = null;
        }

        Customer[] temp = new Customer[_sitters.Length];
        int i = 0;
        for (int a = 0; a < _sitters.Length; a++)
        {
            if (_sitters[a] != null)
            {
                temp[i] = _sitters[a];
                i++;
            }
        }
        _sitters = temp;
    }
    #endregion

}
