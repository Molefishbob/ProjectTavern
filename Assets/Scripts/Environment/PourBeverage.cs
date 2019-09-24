using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PourBeverage : PlayerUseable
{
    public enum Beverage
    {
        None = 0,
        PaleLager = 1,
        StoutBeer = 2,
        WhiteWine = 3,
        RedWine = 4,
        FruitCocktail = 5,
        Spirit = 6,
        Water = 7,
        Absinthe = 8
    }

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
            case Beverage.Absinthe:
                {
                    Debug.Log("Poured absinthe");
                }
                break;
            case Beverage.None:
                {

                }
                break;
        }
    }
}