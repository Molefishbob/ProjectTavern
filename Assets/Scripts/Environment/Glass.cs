using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Managers.BeverageManager;

public class Glass : MonoBehaviour
{
    private List<Ingredient.DrinkIngredient>_ingredients = new List<Ingredient.DrinkIngredient>();
    private Drink _currentDrink;

    public void AddIngredient(Ingredient.DrinkIngredient ingredient)
    {
        _ingredients.Add(ingredient);
        CheckCurrentDrink();
    }

    public void CheckCurrentDrink()
    {

        foreach(Beverage beverage in System.Enum.GetValues(typeof(Beverage)))
        {
            
        }
    }

}
