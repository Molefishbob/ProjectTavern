using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishWasher : PlayerUseable
{

    protected override void Awake()
    {
        base.Awake();
        _timer.OnTimerCompleted += WashDish;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _timer.OnTimerCompleted -= WashDish;
    }
    public void WashDish()
    {
        Glass glass;
        if(User.CurrentlyHeld == PlayerState.Holdables.Glass && User.GetComponentInChildren<Glass>() != null)
        {
            glass = User.GetComponentInChildren<Glass>();
            glass._isDirty = false;
            glass._spriteRend.sprite = glass._cleanEmpty;
        }
        else
        {
            Debug.Log("You need a dirty glass in order to wash a dirty glass");
        }
    }
}
