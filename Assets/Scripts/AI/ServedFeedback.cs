using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServedFeedback : MonoBehaviour
{
    private Vector3 _startingPosition;
    private Vector3 _endPosition;
    private ScaledOneShotTimer _timer;
    [SerializeField]
    private float _lerptime = 0.5f;


    // Start is called before the first frame update
    private void Awake()
    {
        _timer = gameObject.AddComponent<ScaledOneShotTimer>();
    }

    private void Start()
    {
        _timer.OnTimerCompleted += Disappear;
    }

    private void OnDestroy()
    {
        _timer.OnTimerCompleted -= Disappear;
    }

    private void OnEnable()
    {
        print(_startingPosition);
        _startingPosition = transform.position;
        _timer.StartTimer(_lerptime);
        _endPosition = _startingPosition + new Vector3(0, 0.5f, 0);
    }

    private void OnDisable()
    {
        transform.position = _startingPosition;
        _timer.StopTimer();
    }

    // Update is called once per frame
    private void Update()
    {
        if (_timer.IsRunning)
        {
            transform.position = Vector3.Lerp(_startingPosition, _endPosition, _timer.NormalizedTimeElapsed);
        }
    }

    private void Disappear()
    {
        gameObject.SetActive(false);
    }
}
