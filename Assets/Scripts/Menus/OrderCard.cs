using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OrderCard : MonoBehaviour
{
    public Customer.MyOrder Order { get; private set; }
    public Customer FromCustomer { get; private set; }
    [HideInInspector]
    public RectTransform RectTransform { get => (RectTransform)transform; }
    public bool IsGoingToBeDeleted { get; private set; }
    
    private UnscaledOneShotTimer _timer = null;
    private float _movementTime = 0.5f;

    private Vector3 _startPos;
    private Vector3 _endPos;

    [SerializeField]
    private GameObject[] _indgredientbBlocks = null;
    [SerializeField]
    private TextMeshProUGUI[] _ingredientText = null;
    [SerializeField]
    private TextMeshProUGUI _drinkText = null;

    private void Awake()
    {
        _timer = gameObject.AddComponent<UnscaledOneShotTimer>();
    }

    private void OnDestroy()
    {
        if (IsGoingToBeDeleted)
            _timer.OnTimerCompleted -= DestroySelf;
    }

    private void Update()
    {
        if (_timer.IsRunning)
        {
            transform.position = Vector3.Lerp(_startPos, _endPos, Smoothing.SmootherStep(_timer.NormalizedTimeElapsed));
        }
    }

    public void SetInfo(Customer.MyOrder order, Customer from = null)
    {
        Order = order;
        FromCustomer = from;

        if (order._order != PlayerState.Holdables.Drink)
            return;

        _drinkText.text = order._drinkOrder.ToString();

        Drink tmp = null;
        foreach (var item in Managers.LevelManager.Instance.PossibleDrinks)
            if (item._drink == order._drinkOrder)
                tmp = item;

        for (int i = 0; i < tmp._ingredients.Count; i++)
        {
            _ingredientText[i].text = tmp._ingredients[i].ToString();
        }

        for (int i = tmp._ingredients.Count; i < _indgredientbBlocks.Length; i++)
        {
            _indgredientbBlocks[i].SetActive(false);
        }
    }

    public void MarkForDeletion()
    {
        IsGoingToBeDeleted = true;
        _timer.OnTimerCompleted += DestroySelf;
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void Move(Vector2 position, bool relative)
    {
        if (IsGoingToBeDeleted)
            return;

        if (_timer.IsRunning)
            transform.position = _endPos;
        _startPos = transform.position;
        if (relative)
        {
            _endPos = _startPos + new Vector3(position.x, position.y);
        }
        else
        {
            _endPos = new Vector3(position.x, position.y);
        }
        _timer.StartTimer(_movementTime);
    }
}
