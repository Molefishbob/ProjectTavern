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
    private int _totalSeatsCount;
    private int _freeSeatsCount;
    public GameObject chair;

    private void Awake()
    {
        _freeSeatsCount = _totalSeatsCount;
    }
    public override void Use()
    {
        if (_freeSeatsCount > 0)
        {
            _freeSeatsCount--;
        }
    }

}
