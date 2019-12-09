using System.Collections.Generic;
using UnityEngine;
using static Managers.BeverageManager;
using static Managers.IngredientManager;

[CreateAssetMenu(fileName = "New Drink", menuName = "Drink",order = 4)]
public class Drink : ScriptableObject
{
    public Beverage _drink = Beverage.None;
    public List<DrinkIngredient> _ingredients = new List<DrinkIngredient>();
    public int _alcoholContent;
    public int _price;
    public int _amountOfUses;
    public Sprite _sprite;
}
