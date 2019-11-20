using System;
using System.Collections.Generic;
using UnityEngine;
using static Managers.IngredientManager;
using static Managers.BeverageManager;

public class Glass : PlayerUseable
{
    public List<DrinkIngredient> _currentIngredients = new List<DrinkIngredient>();
    public float UseTime { get => _interactionTime; set => _interactionTime = value; }
    private List<Drink> _drinks;
    private Drink[] _possibleDrinks = new Drink[10];
    private Drink _currentDrink = null;
    private string _drinkName = "";
    private string[] _beverageNames;

    public Drink CurrentDrink { get { return _currentDrink; } }

    protected override void Awake()
    {
        base.Awake();
        _timer.OnTimerCompleted += TakeGlass;
    }

    protected override void Start()
    {
        _drinks = new List<Drink>();
        int i = 0;
        foreach (Drink drink in Resources.LoadAll<Drink>("Drinks"))
        {
            _drinks.Add(drink);
            _possibleDrinks[i] = drink;
            i++;
        }
        _beverageNames = Enum.GetNames(typeof(Beverage));
        _timer.OnTimerCompleted += UpdateUser;

    }

    protected override void OnDestroy()
    {
        
        _timer.OnTimerCompleted -= TakeGlass;
        _timer.OnTimerCompleted -= UpdateUser;
    }

    /// <summary>
    /// Picks up glass, makes it users child, positions the glass into position and disables glasses trigger. 
    /// Trigger should only be activated when glass is not a useable gameobject
    /// </summary>
    public void TakeGlass()
    {
        User.CurrentlyHeld = PlayerState.Holdables.Glass;
        transform.parent = User.transform;
        transform.position = transform.parent.position + new Vector3(0, 0.4f, 0);
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
    }

    /// <summary>
    /// Puts glass away from uesrs hands to given gameobjects transform and glass the targets child object.
    /// </summary>
    /// <param name="trans">Target transform to give glass to</param>
    public void PutGlassDown(Transform trans)
    {
        gameObject.transform.parent = trans;
        gameObject.transform.position = trans.position;
    }

    /// <summary>
    /// Adds ingredient to the list of ingredients that are currently in the glass.
    /// </summary>
    /// <param name="ingredient">Ingredient to be added</param>
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

    /// <summary>
    /// Goes through all ingredients in all possible drinks, and compares them to what ingredients are in the glass to see if they make an actual drink.
    /// </summary>
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
                    GetDrinkName();
                }
            }
        }
        if (_currentDrink == null)
        {
            Debug.Log("Warning, no valid drink!");
            GetDrinkName();
        }
    }

    /// <summary>
    /// Visually updates users held drink.
    /// </summary>
    public void GetDrinkName()
    {

        if (_currentDrink != null)
        {
            _drinkName = _currentDrink.ToString();
        }
        else
        {
            _drinkName = "";
        }

        if(!_drinkName.Equals("")) _drinkName = _drinkName.Remove(_drinkName.Length - 8, 8);

        Beverage bev = Beverage.None;

        for (int i = 0; i < _beverageNames.Length; i++)
        {
            if (_beverageNames[i].Trim().Equals(_drinkName.Trim()))
            {
                bev = (Beverage)Enum.Parse(typeof(Beverage), _drinkName);
                User.HeldDrink = bev;
                break;
            }
            else
            {
                User.HeldDrink = Beverage.None;
            }
        }
    }

    /// <summary>
    /// Empties glass...duh
    /// </summary>
    public void EmptyGlass()
    {
        GetPossibleDrinks();
        _currentIngredients.Clear();
        _currentDrink = null;
    }

    /// <summary>
    /// Goes through all possible drinks in the game, and puts them in an array.
    /// </summary>
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