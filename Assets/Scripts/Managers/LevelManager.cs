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
        public List<TableInteractions> _tables;
        public Transform _door;
        [SerializeField]
        private int _maxQueueLength = 5;
        public Customer[] _customerQueue;
        public CustomerPool _customerPoolPrefab;
        public PukePool _pukePoolPrefab;
        private ScaledOneShotTimer _levelTimer;

        private void Awake()
        {
            _customerQueue = new Customer[_maxQueueLength];
            Instantiate(_pukePoolPrefab);
            Instantiate(_customerPoolPrefab);
            _tables.AddRange(FindObjectsOfType<TableInteractions>());
            if (GameObject.Find("Door") != null)
            {
                _door = GameObject.Find("Door").transform;
            }else
            {
                Debug.LogError("No door found.");
            }
            _levelTimer = gameObject.AddComponent<ScaledOneShotTimer>();
            _levelTimer.OnTimerCompleted += EndLevel;
        }

        private void OnDestroy()
        {
            _levelTimer.OnTimerCompleted -= EndLevel;
        }

        private void Start()
        {
            _levelTimer.StartTimer(_playTime);
        }

        private void EndLevel()
        {
            //TODO: end level
        }

    }

}
