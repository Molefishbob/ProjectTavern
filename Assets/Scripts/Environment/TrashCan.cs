using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : PlayerUseable
{
    protected override void Awake()
    {
        base.Awake();
        _timer.OnTimerCompleted += PutStuff;
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        _timer.OnTimerCompleted -= PutStuff;
    }

    private void PutStuff()
    {
        if (User.CurrentlyHeld == PlayerState.Holdables.Glass)
        {
            User.GetComponentInChildren<Glass>().EmptyGlass();
        }
        else
        {
            User.CurrentlyHeld = PlayerState.Holdables.Nothing;
        }

        User.HeldDrink = Managers.BeverageManager.Beverage.None;
    }
}
