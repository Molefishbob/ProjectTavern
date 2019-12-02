using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class OrderCardManager : MonoBehaviour
    {
        public static OrderCardManager Instance { get; private set; }
        public bool Initialized { get; private set; }
        public static bool DoesExist { get => Instance != null; }

        private OrderCard _cardTemplate = null;
        private List<OrderCard> _spawnedCards = new List<OrderCard>();
        private List<Customer.MyOrder> _allOrders = new List<Customer.MyOrder>();
        private float _xMovement = 2.05f;
        private float _yMovement = 1.1f;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            _cardTemplate = transform.GetChild(0).GetComponent<OrderCard>();
            _cardTemplate.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }

        public void AddCard(Customer.MyOrder order, Customer from = null, bool isTestCard = false)
        {
            if (order._order != PlayerState.Holdables.Drink)
                return;

            _allOrders.Add(order);

            foreach (var item in Managers.LevelManager.Instance.PossibleDrinks)
                if (item._drink == order._drinkOrder && item._ingredients.Count == 1)
                    return;

            if (!isTestCard)
                foreach (var item in _spawnedCards)
                    if (item.Order._drinkOrder == order._drinkOrder)
                        return;

            _spawnedCards.Add(Instantiate(_cardTemplate, transform));
            _spawnedCards[_spawnedCards.Count - 1].gameObject.SetActive(true);
            Vector3 tmp = _spawnedCards[_spawnedCards.Count - 1].transform.position;
            tmp.y -= _yMovement;
            _spawnedCards[_spawnedCards.Count - 1].transform.position = tmp;
            tmp.y += _yMovement;
            _spawnedCards[_spawnedCards.Count - 1].Move(tmp, false);
            _spawnedCards[_spawnedCards.Count - 1].SetInfo(order, from);

            for (int i = 0; i < _spawnedCards.Count - 1; i++)
            {
                _spawnedCards[i].Move(new Vector2(-_xMovement, 0), true);
            }
        }

        public void RemoveCard(Customer.MyOrder order, Customer from = null)
        {
            if (_allOrders.Remove(order))
            {
                if (_allOrders.Contains(order))
                    return;
                OrderCard removeThis = null;
                foreach (OrderCard item in _spawnedCards)
                {
                    if (item.Order._drinkOrder == order._drinkOrder)
                    {
                        removeThis = item;
                        item.Move(new Vector2(0, -_yMovement), true);
                        item.MarkForDeletion();
                        break;
                    }
                }

                for (int i = 0; i < _spawnedCards.IndexOf(removeThis); i++)
                {
                    _spawnedCards[i].Move(new Vector2(_xMovement, 0), true);
                }

                _spawnedCards.Remove(removeThis);
            }
        }
    }
}
