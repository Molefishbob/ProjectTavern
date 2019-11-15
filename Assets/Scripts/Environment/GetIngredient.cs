using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Managers.IngredientManager;

public class GetIngredient : PlayerUseable
{

    public DrinkIngredient _ingredient;

    protected override void Awake()
    {
        base.Awake();
        _timer.OnTimerCompleted += TakeIngredient;
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        _timer.OnTimerCompleted -= TakeIngredient;
    }
    private void TakeIngredient()
    {
        if (User.CurrentlyHeld == PlayerState.Holdables.Glass)
        {
            User.GetComponentInChildren<Glass>().AddIngredient(_ingredient);
            switch (_ingredient)
            {
                case DrinkIngredient.Ale:
                    {
                        Debug.Log("Got Ale");
                    }
                    break;
                case DrinkIngredient.Whisky:
                    {
                        Debug.Log("Got Whisky");
                    }
                    break;
                case DrinkIngredient.Vodka:
                    {
                        Debug.Log("Got Vodka");
                    }
                    break;
                case DrinkIngredient.Lemon:
                    {
                        Debug.Log("Got Lemon");
                    }
                    break;
                case DrinkIngredient.Grapes:
                    {
                        Debug.Log("Got Grapes");
                    }
                    break;
                case DrinkIngredient.Sugar:
                    {
                        Debug.Log("Got Sugar");
                    }
                    break;
                case DrinkIngredient.Ice:
                    {
                        Debug.Log("Got Ice");
                    }
                    break;
                default:
                    
                    Debug.LogError("Error!!! No ingredient selected");
                    break;
            }
        }
        else
        {
            Debug.Log("You need a glass");
        }
    }
}
