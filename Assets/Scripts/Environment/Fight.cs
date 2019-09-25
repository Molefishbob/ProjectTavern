using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight : PlayerUseable
{
    protected override void Awake()
    {
        base.Awake();
        _timer.OnTimerCompleted += FightStopped;
    }

    private void FightStopped()
    {
        Debug.Log("FINISH HIM!");
    }
}