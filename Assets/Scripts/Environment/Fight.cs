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
    protected override void OnDestroy()
    {
        base.OnDestroy();
        _timer.OnTimerCompleted -= FightStopped;
    }
    private void FightStopped()
    {
        Debug.Log("Reima älä räjähdä, me tiedetään miltä susta tuntuu");
    }
}