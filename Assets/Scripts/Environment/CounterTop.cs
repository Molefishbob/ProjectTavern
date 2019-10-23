using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static Managers.BeverageManager;

public class CounterTop : PlayerUseable
{
    public PlayerState.Holdables StuffOnThis = PlayerState.Holdables.Nothing;
    public Beverage Beverage = Beverage.None;
    private TextMeshProUGUI _nameText = null;

    protected override void Awake()
    {
        base.Awake();
        _timer.OnTimerCompleted += PutStuff;

        _nameText = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
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

            _nameText.text = "" + StuffOnThis.ToString()[0];
            if (Beverage != Beverage.None)
            {
                _nameText.text += "\\" + Beverage.ToString()[0];
            }

        }
        // Take stuff from this
        else if (User.CurrentlyHeld == PlayerState.Holdables.Nothing && StuffOnThis != PlayerState.Holdables.Nothing)
        {
            User.CurrentlyHeld = StuffOnThis;
            User.HeldDrink = Beverage;

            StuffOnThis = PlayerState.Holdables.Nothing;
            Beverage = Beverage.None;
            _nameText.text = "";
        }
    }
}
