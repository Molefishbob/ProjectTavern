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

        public bool test = false;

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

        public void AddCard(Customer.MyOrder order, Customer from = null)
        {
            foreach (var item in _spawnedCards)
                if (item.Order._drinkOrder == order._drinkOrder || order._order != PlayerState.Holdables.Drink)
                    return;

            _spawnedCards.Add(Instantiate(_cardTemplate, transform));
            _spawnedCards[_spawnedCards.Count - 1].SetInfo(order, from);
            _spawnedCards[_spawnedCards.Count - 1].gameObject.SetActive(true);

            for (int i = 0; i < _spawnedCards.Count - 1; i++)
            {
                _spawnedCards[i].Move(new Vector2(-2, 0), true);
            }
        }

        public void RemoveCard(Customer.MyOrder order, Customer from = null)
        {
            throw new System.NotImplementedException("Kikkeli 8==D");
        }

        private void Update()
        {
            if (test)
            {
                AddCard(new Customer.MyOrder(PlayerState.Holdables.Drink, (Managers.BeverageManager.Beverage) Random.Range(0, 11)));
                test = false;
            }
        }
    }
}
