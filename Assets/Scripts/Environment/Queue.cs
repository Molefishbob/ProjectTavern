using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

public class Queue : AIUseable
{
    private int _queueLength;
    private int _freeSpots;
    [HideInInspector]
    public Transform[] _queueSpots;
    private List<Customer> _queuedCustomers = new List<Customer>();

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

    private void Update()
    {
        if(_queuedCustomers.Count > 0) {
            LevelManager.Instance.LeaveQueue(_queuedCustomers[0]);
        }
    }
    public void GoToQueue(Customer ai)
    {
        ai.GetInLine(_queueSpots[_queueLength - _freeSpots].transform);
        _queuedCustomers.Add(ai);
        _freeSpots--;
    }

}
