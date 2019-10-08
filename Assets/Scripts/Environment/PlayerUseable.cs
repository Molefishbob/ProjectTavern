using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameInput;

public abstract class PlayerUseable : MonoBehaviour, IUseable
{
    protected ScaledOneShotTimer _timer;
    [SerializeField, Tooltip("How long the action takes to finish")]
    protected float _interactionTime = 2;
    public bool IsBeingUsed { get => _timer.IsRunning; }
    public float CopmletePerc { get => _timer.NormalizedTimeElapsed; }
    public GameObject User { get; private set; }

    protected virtual void Awake()
    {
        _timer = gameObject.AddComponent<ScaledOneShotTimer>();
        _timer.OnTimerCompleted += ClearInfo;
    }

    public virtual void Use()
    {

    }

    public virtual void Use(GameObject player)
    {
        _timer.StartTimer(_interactionTime);
        User = player;
    }

    public virtual void InterruptAction()
    {
        _timer.StopTimer();
        ClearInfo();
    }

    private void ClearInfo()
    {
        User = null;
    }
}
