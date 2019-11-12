using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Managers.BeverageManager;

public class Glass : MonoBehaviour
{
    public List<Ingredient.DrinkIngredient> _ingredients = new List<Ingredient.DrinkIngredient>();
    private List<Drink> _drinks;
    private Drink[] _possibleDrinks = new Drink[10];
    private Drink _currentDrink;

    private void Start()
    {
        _drinks = new List<Drink>();
        int i = 0;
        foreach (Drink drink in Resources.LoadAll<Drink>("Drinks"))
        {
            _drinks.Add(drink);
            _possibleDrinks[i] = drink;
            i++;
        }
    }

    private void Update()
    {
        CheckCurrentDrink();
    }

    public void AddIngredient(Ingredient.DrinkIngredient ingredient)
    {
        _ingredients.Add(ingredient);

        CheckCurrentDrink();
    }

    public void CheckCurrentDrink()
    {
        for (int i = 0; i < _possibleDrinks.Length; i++)
        {
            if (_possibleDrinks[i]._ingredients.Count > _ingredients.Count)
            {
                for (int a = 0; a < _possibleDrinks[i]._ingredients.Count; a++)
                {
                    _possibleDrinks[i] = null;
                }
            }
            else
            {
                for (int b = 0; b < _ingredients.Count; b++)
                {
                    if (!_possibleDrinks[i]._ingredients.Contains(_ingredients[b]))
                    {
                        _possibleDrinks[i] = null;
                        _currentDrink = null;
                    }
                    else
                    {
                        _currentDrink = _possibleDrinks[i];
                    }
                }
            }
            
        }

        print(_currentDrink);
    }

}