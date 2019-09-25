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

    [SerializeField]
    private int _totalSeatsCount = 4;
    private int _freeSeatsCount;
    public GameObject[] _chairs;

    private void Awake()
    {
        _freeSeatsCount = _totalSeatsCount;
        _chairs = new GameObject[_totalSeatsCount];
    }
    public override void Use()
    {
        if (_freeSeatsCount > 0)
        {
            _freeSeatsCount--;
        }
    }

}
