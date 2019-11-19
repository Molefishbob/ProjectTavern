using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Managers.IngredientManager;

public class Glass : PlayerUseable
{
    public List<DrinkIngredient> _currentIngredients = new List<DrinkIngredient>();
    private List<Drink> _drinks;
    private Drink[] _possibleDrinks = new Drink[10];
    private Drink _currentDrink;

    public Drink CurrentDrink { get { return _currentDrink; } }

    protected override void Awake()
    {
        base.Awake();
        _timer.OnTimerCompleted += TakeGlass;
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        _timer.OnTimerCompleted -= TakeGlass;
    }

    protected override void Start()
    {
        base.Start();
        _drinks = new List<Drink>();
        int i = 0;
        foreach (Drink drink in Resources.LoadAll<Drink>("Drinks"))
        {
            _drinks.Add(drink);
            _possibleDrinks[i] = drink;
            i++;
        }
    }

    public void TakeGlass()
    {
        User.CurrentlyHeld = PlayerState.Holdables.Glass;
        transform.parent = User.transform;
        User.ClearUsable();
    }

    public void PutGlassDown()
    {
        gameObject.transform.parent = null;
    }

    public void AddIngredient(DrinkIngredient ingredient)
    {
        if (!_currentIngredients.Contains(ingredient))
        {
            _currentIngredients.Add(ingredient);
        }
        else
        {
            Debug.Log("Glass already contains " + ingredient);
        }

        if (_currentIngredients.Count < 5)
        {
            GetPossibleDrinks();
            CheckCurrentDrink();
        }
        else
        {
            Debug.Log("Too many ingredients. Throw that slop away!");
        }
    }

    public void CheckCurrentDrink()
    {
        _currentDrink = null;
        for (int i = 0; i < _possibleDrinks.Length; i++)
        {
            if (_possibleDrinks[i] == null) continue;

            if (_possibleDrinks[i]._ingredients.Count != _currentIngredients.Count)
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
        if (_currentDrink == null)
        {
            Debug.Log("Warning, no valid drink!");
        }
        print("ffffffffffff" + _currentDrink);
    }

    public void EmptyGlass()
    {
        GetPossibleDrinks();
        _currentIngredients.Clear();
        _currentDrink = null;
    }

    private void GetPossibleDrinks()
    {
        int i = 0;
        foreach (Drink drink in _drinks)
        {
            _possibleDrinks[i] = drink;
            i++;
        }
    }

}