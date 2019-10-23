using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Managers.BeverageManager;

public class PlayerState : MonoBehaviour
{
    #region EnumsAndOtherStuff
    public enum Holdables
    {
        Nothing = 0,
        Drink = 1,
        Food = 2,
        Corpse = 3
    }
    #endregion

    #region MemberVariables

    private GameObject _actionBar = null;
    private UnityEngine.UI.Image _actionBarFill = null;

    #endregion

    #region Properties

    public PlayerUseable UseableObject { get; private set; } = null;
    public Holdables CurrentlyHeld = Holdables.Nothing;
    public Beverage HeldDrink = Beverage.None;
    public float UseProgress { get => UseableObject.CopmletePerc; }

    #endregion

    #region Unity Methods

    void Awake()
    {
        _actionBar = gameObject.transform.GetChild(0).GetChild(0).gameObject;
        _actionBarFill = _actionBar.transform.GetChild(0).GetChild(0).GetComponentInChildren<UnityEngine.UI.Image>();
        _actionBarFill.fillAmount = 0;
        _actionBar.SetActive(false);
    }

    private void Update()
    {
        if (UseableObject?.User == this)
        {
            _actionBarFill.fillAmount = UseableObject.CopmletePerc;
        }
        else if (_actionBar.activeSelf && (UseableObject == null || !UseableObject.IsBeingUsed))
        {
            _actionBar.SetActive(false);
        }
    }

    #endregion

    #region Methods

    public void UseUseable()
    {
        if (UseableObject != null && !UseableObject.IsBeingUsed 
            && (CurrentlyHeld == Holdables.Nothing || !UseableObject.RequiresEmptyHands))
        {
            UseableObject.Use(this);

            if (UseableObject.ShowProgressBar)
            {
                _actionBar.SetActive(true);
                _actionBarFill.fillAmount = 0;
            }

            Debug.Log("Action started on " + UseableObject.GetType().ToString());
        }
    }
    
    #endregion

    #region Collision Detection

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerUsable")
        {
            UseableObject = collision.GetComponent<PlayerUseable>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerUsable" && collision.GetComponent<PlayerUseable>() == UseableObject)
        {
            if (UseableObject.IsBeingUsed && UseableObject.User == this)
            {
                UseableObject.InterruptAction();
                Debug.Log(UseableObject.GetType().ToString() + " action interrupted!");
            }
            UseableObject = null;
        }
    }

    #endregion
    
}
