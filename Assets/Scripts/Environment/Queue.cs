using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

public class Queue : AIUseable
{
    private int _queueLength;
    private int _freeSpots;
    public Transform[] _queueSpots;

    private void Awake()
    {
        _queueLength = transform.childCount;
        _queueSpots = new Transform[_queueLength];
        _freeSpots = _queueLength;
        for (int a = 0; a < _queueLength; a++)
        {
            _queueSpots[a] = transform.GetChild(a);
        }

    }

    public void GoToQueue(Customer ai)
    {
        ai.GetInLine(_queueSpots[_queueLength - _freeSpots].transform);
        _freeSpots--;
    }

}
