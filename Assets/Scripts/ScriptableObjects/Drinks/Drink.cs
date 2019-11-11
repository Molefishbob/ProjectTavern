using System.Collections.Generic;
using UnityEngine;
using static Managers.BeverageManager;

[CreateAssetMenu(fileName = "New Drink", menuName = "Drink",order = 4)]
public class Drink : ScriptableObject
{
    public Beverage _drink = Beverage.None;
    public List<Ingredient.DrinkIngredient> _ingredients = new List<Ingredient.DrinkIngredient>();
    public int _alcoholContent;
    public int _price;
    public int _amountOfUses;
}
