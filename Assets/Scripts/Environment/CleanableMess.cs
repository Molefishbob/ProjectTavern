using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanableMess : PlayerUseable
{

    protected override void Awake()
    {
        base.Awake();
        _timer.OnTimerCompleted += Cleaned;
    }

    private void OnDestroy()
    {
        _timer.OnTimerCompleted -= Cleaned;
    }

    private void Cleaned()
    {
        Debug.Log("You cleaned some rancid puke");
        gameObject.SetActive(false);
    }

}
