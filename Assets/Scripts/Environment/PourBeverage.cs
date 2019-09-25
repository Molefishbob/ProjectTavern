using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Managers.BeverageManager;

public class PourBeverage : PlayerUseable
{


    public Beverage _drink;

    public override void Use()
    {
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
                Debug.LogError("Error!!! No beverage selected");
                break;
        }
    }
}