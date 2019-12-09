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

    public TableState _currentState;
    protected int _totalSeatsCount;
    private int _freeSeatsCount;
    protected Transform[] _chairs;
    protected Customer[] _sitters;
    private Transform[] _placesForGlasses;
    private Glass[] _glassesOnTable;
    #endregion

    #region Properties
    public Transform[] Chairs { get => _chairs; }
    public Customer[] Sitters { get => _sitters; }
    public Transform[] GlassPlaces { get { return _placesForGlasses; } }
    public Glass[] GlassesOnTable { get { return _glassesOnTable; } }
    #endregion

    #region Unity Methods
    private void Awake()
    {
        _currentState = TableState.Empty;
        _totalSeatsCount = transform.childCount - 1;
        _chairs = new Transform[_totalSeatsCount];
        for (int a = 0; a < _totalSeatsCount; a++)
        {
            _chairs[a] = transform.GetChild(a);
        }
        _freeSeatsCount = _totalSeatsCount;
        _sitters = new Customer[_totalSeatsCount];

        GameObject temp = gameObject.transform.GetChild(transform.childCount - 1).gameObject;
        _placesForGlasses = new Transform[temp.transform.childCount];
        _glassesOnTable = new Glass[temp.transform.childCount];
        for (int i = 0; i < temp.transform.childCount; i++)
        {
            _placesForGlasses[i] = temp.transform.GetChild(i);
        }

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

        return true;
    }

    /// <summary>
    /// Adds the ai to the table
    /// </summary>
    /// <param name="ai">the customer</param>
    public void AddAi(Customer ai)
    {
        int val = -1;
        for (int a = 0; a < _sitters.Length; a++)
        {
            val = Random.Range(0, _sitters.Length);
            if (_sitters[val] == null)
                break;

        }

        if (_sitters[val] != null)
        {
            print("kys");
            int b = 0;
            foreach (Customer cus in _sitters)
            {
                if (cus == null)
                {
                    val = b;
                    break;
                }
                b++;
            }
        }

        ai.Sit(_chairs[val].transform);
        _sitters[val] = ai;

        _freeSeatsCount--; 
        if (_freeSeatsCount == 0)
        {
            _currentState = TableState.Full;
        }
        else
        {
            _currentState = TableState.Occupied;
        }
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
            if (_sitters[a] != notme && _sitters[a].CurrentState != Managers.AIManager.State.Fighting)
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
    /// Rearranges the sitters array after (Commented out)
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
            if (_sitters[a] == null) continue;
            if (me == _sitters[a].GetInstanceID()) _sitters[a] = null;
        }
    }
    #endregion

}
