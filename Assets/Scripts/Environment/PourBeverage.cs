using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Managers.BeverageManager;

public class PourBeverage : PlayerUseable
{

    public Beverage _drink;

    protected override void Awake()
    {
        base.Awake();
        _timer.OnTimerCompleted += DrinkPoured;
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        _timer.OnTimerCompleted -= DrinkPoured;
    }
    private void DrinkPoured()
    {
        User.CurrentlyHeld = PlayerState.Holdables.Drink;
        User.HeldDrink = _drink;
        switch (_drink)
        {
            case Beverage.PaleLager:
                {
                    Debug.Log("Poured pale lager");
                }
                break;
            case Beverage.StoutBeer:
                {
                    Debug.Log("Poured stout beer");
                }
                break;
            case Beverage.WhiteWine:
                {
                    Debug.Log("Poured white wine");
                }
                break;
            case Beverage.RedWine:
                {
                    Debug.Log("Poured red wine");
                }
                break;
            case Beverage.FruitCocktail:
                {
                    Debug.Log("Poured fruit cock");
                }
                break;
            case Beverage.Spirit:
                {
                    Debug.Log("Poured spirit");
                }
                break;
            case Beverage.Water:
                {
                    Debug.Log("Poured water");
                }
                break;
            case Beverage.Grog:
                {
                    Debug.Log("Poured Grog");
                }
                break;
            default:
                User.CurrentlyHeld = PlayerState.Holdables.Nothing;
                User.HeldDrink = Beverage.None;
                Debug.LogError("Error!!! No beverage selected");
                break;
        }
    }
}