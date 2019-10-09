using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Managers.BeverageManager;

public class CounterTop : PlayerUseable
{
    public PlayerState.Holdables StuffOnThis = PlayerState.Holdables.Nothing;
    public Beverage Beverage = Beverage.None;

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
        // Put stuff on this
        if (StuffOnThis == PlayerState.Holdables.Nothing && User.CurrentlyHeld != PlayerState.Holdables.Nothing)
        {
            StuffOnThis = User.CurrentlyHeld;
            Beverage = User.HeldDrink;

            User.CurrentlyHeld = PlayerState.Holdables.Nothing;
            User.HeldDrink = Beverage.None;
        }
        // Take stuff from this
        else if (User.CurrentlyHeld == PlayerState.Holdables.Nothing && StuffOnThis != PlayerState.Holdables.Nothing)
        {
            User.CurrentlyHeld = StuffOnThis;
            User.HeldDrink = Beverage;

            StuffOnThis = PlayerState.Holdables.Nothing;
            Beverage = Beverage.None;
        }
    }
}
