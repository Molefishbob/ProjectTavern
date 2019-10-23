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
    public PlayerState User { get; private set; }

    [SerializeField]
    protected bool _requiresEmptyHands = true;
    public bool RequiresEmptyHands { get => _requiresEmptyHands; }

    [SerializeField]
    protected bool _showProgressBar = true;
    public bool ShowProgressBar { get => _showProgressBar; }

    protected virtual void Awake()
    {
        _timer = gameObject.AddComponent<ScaledOneShotTimer>();
    }

    // Start is only used to give enought time for other implementations,
    // In case they need to use the User, before data is cleared.
    private void Start()
    {
        _timer.OnTimerCompleted += ClearInfo;
    }

    public virtual void Use()
    {
        Debug.LogError("Use action not set!", gameObject);
    }

    public virtual void Use(PlayerState player)
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

    protected virtual void OnDestroy()
    {
        _timer.OnTimerCompleted -= ClearInfo;
    }
}
