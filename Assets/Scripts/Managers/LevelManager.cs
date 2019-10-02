using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        [HideInInspector]
        public int _currentMoney;
        public int _moneyToWin = 1000;
        public float _playTime = 120f;
        private float _timePassed;
        public List<TableInteractions> _tables;
        public Transform _door;
        [SerializeField]
        private int _maxQueueLength;
        public Customer[] _customerQueue;
        public CustomerPool _customerPoolPrefab;
        public PukePool _pukePoolPrefab;

        private void Awake()
        {
            _customerQueue = new Customer[_maxQueueLength];
            Instantiate(_pukePoolPrefab);
            Instantiate(_customerPoolPrefab);
            _tables.AddRange(FindObjectsOfType<TableInteractions>());
        }

        private void FixedUpdate()
        {
            _timePassed = Time.deltaTime;
            if(_timePassed >= _playTime)
            {
                //TODO: end level
            }
        }
    }

}
