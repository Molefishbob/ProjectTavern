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

    private void Awake()
    {
        _totalSeatsCount = transform.childCount;
        _chairs = new Transform[_totalSeatsCount];
        for (int a = 0; a < _totalSeatsCount; a++)
        {
            _chairs[a] = transform.GetChild(a);
        }
        _freeSeatsCount = _totalSeatsCount;
    }
    public override void Use() { }

    public void Use(Customer ai)
    {
        if (_freeSeatsCount > 0)
        {
            ai.Sit(_chairs[_totalSeatsCount - _freeSeatsCount]);
            _freeSeatsCount--;
        }
    }

}
