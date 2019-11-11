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
            case Beverage.Ale:
                {
                    Debug.Log("Poured Ale");
                }
                break;
            case Beverage.Whisky:
                {
                    Debug.Log("Poured Whisky");
                }
                break;
            case Beverage.Vodka:
                {
                    Debug.Log("Poured Vodka");
                }
                break;
            case Beverage.Wine:
                {
                    Debug.Log("Poured Wine");
                }
                break;
            case Beverage.LongDrink:
                {
                    Debug.Log("Poured LongDrink");
                }
                break;
            case Beverage.Liquor:
                {
                    Debug.Log("Poured Liquor");
                }
                break;
            case Beverage.Cider:
                {
                    Debug.Log("Poured Cider");
                }
                break;
            case Beverage.Martini:
                {
                    Debug.Log("Poured Martini");
                }
                break;
            case Beverage.Grog:
                {
                    Debug.Log("Poured Grog");
                }
                break;
            case Beverage.FruitDrink:
                {
                    Debug.Log("Poured FruitDrink");
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