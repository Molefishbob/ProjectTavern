using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using static Managers.BeverageManager;

public class PlayerState : MonoBehaviour
{
    #region EnumsAndOtherStuff
    public enum Holdables
    {
        Nothing = 0,
        Drink = 1,
        Food = 2,
        Corpse = 3,
        Glass = 4
    }
    #endregion

    #region MemberVariables

    private GameObject _actionBar = null;
    private UnityEngine.UI.Image _actionBarFill = null;
    private TextMeshProUGUI _heldText = null;
    private CircleCollider2D _selfCollider = null;

    #endregion

    #region Properties

    /// <summary>
    /// Usable object that is found via collision
    /// </summary>
    public PlayerUseable UseableObject { get; private set; } = null;

    /// <summary>
    /// What is currently in our players hand
    /// </summary>
    public Holdables CurrentlyHeld = Holdables.Nothing;

    /// <summary>
    /// What drink is in the hand?
    /// </summary>
    public Beverage HeldDrink = Beverage.None;

    /// <summary>
    /// How many percents its done
    /// </summary>
    public float UseProgress { get => UseableObject.CopmletePerc; }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Finds the needed references in runtime.
    /// </summary>
    void Awake()
    {
        _actionBar = gameObject.transform.GetChild(0).GetChild(0).gameObject;
        _actionBarFill = _actionBar.transform.GetChild(0).GetChild(0).GetComponentInChildren<UnityEngine.UI.Image>();
        _actionBarFill.fillAmount = 0;
        _actionBar.SetActive(false);
        _heldText = gameObject.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        _selfCollider = gameObject.GetComponent<CircleCollider2D>();
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
    
    /// <summary>
    /// This should happen when the use button is pressed.
    /// Passes necessary info to the useable object
    /// </summary>
    public void UseUseable()
    {
        // Just an extra step to ensure that everything works as intented
        if (UseableObject == null)
        {
            // The colliders in the array are sorted in order of distance from the origin point.
            // Thats just perfect
            List<RaycastHit2D> hitObjects = Physics2D.CircleCastAll(transform.position, _selfCollider.radius, Vector2.down, 0).Where(t => t.collider.gameObject.tag == "PlayerUsable").ToList();

            if (hitObjects.Count < 1)
                return;

            // Get Component badness :---D
            UseableObject = hitObjects[0].collider.gameObject.GetComponent<PlayerUseable>();
        }
        

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
    
    /// <summary>
    /// Update the held item visually
    /// </summary>
    public void UpdateHeld()
    {
        if (CurrentlyHeld == Holdables.Nothing)
        {
            _heldText.text = "";
        }
        else
        {
            _heldText.text = CurrentlyHeld.ToString()[0] + "";

            if (HeldDrink != Beverage.None)
                _heldText.text += "\\" + HeldDrink.ToString()[0] + HeldDrink.ToString()[1];
        }
    }

    /// <summary>
    /// Just clear it.
    /// </summary>
    public void ClearUsable()
    {
        UseableObject = null;
    }

    #endregion

    #region Collision Detection

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerUsable" && UseableObject == null)
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
                UseableObject = null;
            }
            else if (!UseableObject.IsBeingUsed)
            {
                UseableObject = null;
            }
        }
    }

    #endregion
    
}
