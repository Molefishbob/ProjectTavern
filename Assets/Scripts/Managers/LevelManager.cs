using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        public int _currentMoney;
        public int _moneyToWin = 1000;
        public float _playTime = 120f;
        private float _timePassed;
        public List<TableInteractions> _tables;
        public Transform _door;
        [SerializeField]
        private static int _maxQueueLength;
        public Customer[] _customerQueue = new Customer[_maxQueueLength];
        public CustomerPool _customerPoolPrefab;
        public PukePool _pukePoolPrefab;

        private void Awake()
        {
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
