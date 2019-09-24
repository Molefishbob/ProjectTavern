using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Race", menuName = "Race", order = 2)]
public class Race : ScriptableObject
{
    // TODO: Move elsewhere
    public enum Beverage
    {
        None = 0,
        PaleLager = 1,
        StoutBeer = 2,
        WhiteWine = 3,
        RedWine = 4,
        FruitCocktail = 5,
        Spirit = 6,
        Water = 7
        // TODO: add more beverages
    }
    // End

    public enum Type
    {
        None = 0,
        Human = 1,
        Dwarf = 2,
        Elf = 3,
        Orc = 4
    }

    public int MaxAlcoholTolerance = 10;
    public int MinAlcoholTolerance = 0;
    public Type _race = Type.None;
    public int _alcoholTolerance = 0;
    public int _agressiveness = 0;
    public Beverage _preferredBeverage = Beverage.None;
}
