using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

public class CleanableMess : PlayerUseable
{

    protected override void Awake()
    {
        base.Awake();
        _timer.OnTimerCompleted += Cleaned;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _timer.OnTimerCompleted -= Cleaned;
    }

    private void Cleaned()
    {
        Debug.Log("You cleaned some rancid puke");
        LevelManager.Instance.PukeAmount--;
        gameObject.SetActive(false);
    }

}
