using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Managers;

/// <summary>
/// How customer Interacts with player
/// </summary>
public class CustomerInteraction : PlayerUseable
{
    // Reference to the customer script in parent
    private Customer _customer;
    private static Drink[] _allDrinks;

    protected override void Awake()
    {
        base.Awake();
        if (transform.parent == null)
        {
            Debug.LogError("Customer Interaction does not have parent! Destroying");
            Destroy(this);
            return;
        }

        _timer.OnTimerCompleted += Interract;

        // Lets get the customer script from parent
        _customer = transform.parent.GetComponent<Customer>();

        if (_allDrinks == null)
        {
            _allDrinks = Resources.LoadAll<Drink>("Drinks");
            Managers.BeverageManager.Beverage[] tmp = new Managers.BeverageManager.Beverage[_allDrinks.Length];

            for (int i = 0; i < tmp.Length; i++)
            {
                tmp[i] = _allDrinks[i]._drink;
            }

            tmp = tmp.Distinct().ToArray();

            if (tmp.Length != System.Enum.GetNames(typeof(Managers.BeverageManager.Beverage)).Length - 1)
            {
                Debug.LogWarning("There are uneven amounts of drinks between bewerage and drinks!\nDrinks in resources"
                    + tmp.Length + "\nDrinks in Beverages" + (System.Enum.GetNames(typeof(Managers.BeverageManager.Beverage)).Length - 1));
            }
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _timer.OnTimerCompleted -= Interract;
    }

    // Let's poll the customer state for now.
    // TODO: Change to event based
    private void Update()
    {
        switch (_customer.CurrentState)
        {
            case Managers.AIManager.State.None:
                break;
            case Managers.AIManager.State.Moving:
                break;
            case Managers.AIManager.State.Waiting:
                break;
            case Managers.AIManager.State.Served:
                break;
            case Managers.AIManager.State.PassedOut:
                _interactionTime = 3;
                _requiresEmptyHands = true;
                break;
            case Managers.AIManager.State.Fighting:
                _requiresEmptyHands = true;
                _interactionTime = 2;
                break;
            case Managers.AIManager.State.Ordered:
                _requiresEmptyHands = false;
                _interactionTime = 0.5f;
                break;
            default:
                break;
        }
    }

    private void Interract()
    {
        switch (_customer.CurrentState)
        {
            case Managers.AIManager.State.None:
                break;
            case Managers.AIManager.State.Moving:
                break;
            case Managers.AIManager.State.Waiting:
                break;
            case Managers.AIManager.State.Served:
                break;
            case Managers.AIManager.State.PassedOut:
                _customer.Leave(Managers.LevelManager.Instance.Exit);
                break;
            case Managers.AIManager.State.Fighting:
                _customer.Leave(Managers.LevelManager.Instance.Exit);
                _customer.FightOpponent.Leave(Managers.LevelManager.Instance.Exit);
                break;
            case Managers.AIManager.State.Ordered:
                if (User.CurrentlyHeld == PlayerState.Holdables.Nothing) break;

                if (_customer.Served(ConvertBeverageToDrink(User.HeldDrink), User.Glass))
                {
                    // TODO: Tell the customer to drink muchos alcohol
                    Debug.Log("CORRECTLY SERVED!");
                    User.CurrentlyHeld = PlayerState.Holdables.Nothing;
                    User.HeldDrink = Managers.BeverageManager.Beverage.None;
                    Glass glass = User.GetComponentInChildren<Glass>();
                    glass.transform.parent = _customer.transform;
                    glass.transform.position = _customer.transform.position;

                    if (_customer._happyIndicator != null)
                    {
                        _customer._happyIndicator.SetActive(true);
                    }

                    if (glass._isDirty)
                    {

                        LevelManager.Instance.Happiness -= LevelManager.Instance._dirtyGlassUnhappiness;
                    }
                    else
                    {
                        LevelManager.Instance.Happiness += LevelManager.Instance._correctDrinkHappiness;
                    }
                }
                else if (_customer.Served(User.CurrentlyHeld))
                {
                    // TODO: Tell the customer to drink muchos alcohol
                    Debug.Log("CORRECTLY SERVED FOOD!");
                    User.CurrentlyHeld = PlayerState.Holdables.Nothing;
                    LevelManager.Instance.Happiness += LevelManager.Instance._correctDrinkHappiness;
                    if (User._carriedFood != null)
                    {
                        User._carriedFood.SetActive(false);
                    }
                    if (_customer._happyIndicator != null)
                    {
                        _customer._happyIndicator.SetActive(true);
                    }
                }
                else
                {
                    // TODO: Lower happiness and other stuff
                    Debug.Log("INCORRECTLY SERVED!");
                    LevelManager.Instance.Happiness -= LevelManager.Instance._wrongDrinkUnhappiness;
                    if (_customer._angryIndicator != null)
                    {
                        _customer._angryIndicator.SetActive(true);
                    }
                }
                break;
            default:
                break;
        }
    }

    private Drink ConvertBeverageToDrink(Managers.BeverageManager.Beverage beverage)
    {
        foreach (Drink drink in _allDrinks)
            if (drink._drink == beverage)
                return drink;
        return null;
    }
}
