using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glass : MonoBehaviour
{
    public List<Ingredient.DrinkIngredient> _currentIngredients = new List<Ingredient.DrinkIngredient>();
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

    public void AddIngredient(Ingredient.DrinkIngredient ingredient)
    {
        _currentIngredients.Add(ingredient);

        CheckCurrentDrink();
    }

    public void CheckCurrentDrink()
    {
        for (int i = 0; i < _possibleDrinks.Length; i++)
        {
            if (_possibleDrinks[i] == null) continue;

            if (_possibleDrinks[i]._ingredients.Count > _currentIngredients.Count)
            {
                _possibleDrinks[i] = null;
            }
            else
            {
                bool doesContain = false;
                for (int b = 0; b < _currentIngredients.Count; b++)
                {
                    if (_possibleDrinks[i]._ingredients.Contains(_currentIngredients[b]))
                    {
                        doesContain = true;
                    }
                    else
                    {
                        doesContain = false;
                        break;
                    }
                }
                if (doesContain)
                {
                    _currentDrink = _possibleDrinks[i];
                }
            }

        }
        if(_currentDrink = null)
        {
            Debug.Log("Warning, no valid drink!");
        }
        
    }

    public void EmptyGlass()
    {
        int i = 0;
        foreach (Drink drink in _drinks)
        {
            _possibleDrinks[i] = drink;
            i++;
        }
        _currentIngredients.Clear();
        _currentDrink = null;
    }

}