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
    [HideInInspector]
    public List<Customer> _queuedCustomers = new List<Customer>();

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
        if (_queuedCustomers.Count > 0)
        {
            if (LevelManager.Instance.LeaveQueue(_queuedCustomers[0]))
            {
                _freeSpots++;
                MoveUpInQueue();
            }

        }
    }

    /// <summary>
    /// Moves spawned customer to the first free spot in the queue.
    /// </summary>
    /// <param name="ai"></param>
    public void GoToQueue(Customer ai)
    {
        ai.GetInLine(_queueSpots[_queueLength - _freeSpots].transform);
        _queuedCustomers.Add(ai);
        _freeSpots--;
    }

    /// <summary>
    /// When a customer leaves the queue, moves the rest of the customers up one spot in the queue.
    /// </summary>
    public void MoveUpInQueue()
    {
        for (int i = 0; i < _queuedCustomers.Count; i++)
        {
            _queuedCustomers[i].GetInLine(_queueSpots[i]);
        }
    }

}
