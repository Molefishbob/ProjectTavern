using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableInteractions : AIUseable
{
    public enum State
    {
        None = 0,
        Empty = 1,
        Occupied = 2,
        Full = 3
    }

    protected int _totalSeatsCount;
    private int _freeSeatsCount;
    protected Transform[] _chairs;
    protected Customer[] _sitters;

    private void Awake()
    {
        _totalSeatsCount = transform.childCount;
        _chairs = new Transform[_totalSeatsCount];
        for (int a = 0; a < _totalSeatsCount; a++)
        {
            _chairs[a] = transform.GetChild(a);
        }
        _freeSeatsCount = _totalSeatsCount;
        _sitters = new Customer[_totalSeatsCount];
    }
    public override void Use() { }

    /// <summary>
    /// Adds the ai to sit on the table if there is room
    /// </summary>
    /// <param name="ai">The ai to be added to the list</param>
    /// <returns>True if gains a seat, otherwise false/returns>
    public bool Use(Customer ai)
    {
        if (_freeSeatsCount > 0)
        {
            ai.Sit(_chairs[_totalSeatsCount - _freeSeatsCount]);
            _sitters[_totalSeatsCount - _freeSeatsCount] = ai;
            _freeSeatsCount--;
            return true;
        } else
        {
            return false;
        }
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
        _freeSeatsCount++;
        _sitters = temp;
    }

}
