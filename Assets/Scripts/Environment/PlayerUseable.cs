using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameInput;

/// <summary>
/// Everything usable should extend this class.
/// Creates the basic functionality for them.
/// Extended classes only needs to add a parametless function to _timer.OnTimerCompleted
/// </summary>
public abstract class PlayerUseable : MonoBehaviour, IUseable
{
    protected ScaledOneShotTimer _timer;
    [SerializeField, Tooltip("How long the action takes to finish")]
    protected float _interactionTime = 2;

    /// <summary>
    /// Is the object being used by someone?
    /// </summary>
    public bool IsBeingUsed { get => _timer.IsRunning; }

    /// <summary>
    /// The percent of completion
    /// </summary>
    public float CopmletePerc { get => _timer.NormalizedTimeElapsed; }

    /// <summary>
    /// Who is using this?
    /// </summary>
    public PlayerState User { get; private set; }

    [SerializeField, Tooltip("Does this object require empty hands to be used?")]
    protected bool _requiresEmptyHands = true;

    /// <summary>
    /// Does this object require empty hands to be used?
    /// </summary>
    public bool RequiresEmptyHands { get => _requiresEmptyHands; }

    [SerializeField, Tooltip("Do we want to show the progres bar for this object?")]
    protected bool _showProgressBar = true;

    /// <summary>
    /// Do we want to show the progres bar for this object?
    /// </summary>
    public bool ShowProgressBar { get => _showProgressBar; }

    protected virtual void Awake()
    {
        _timer = gameObject.AddComponent<ScaledOneShotTimer>();
    }

    // Start is only used to give enough time for other implementations,
    // In case they need to use the User, before data is cleared.
    private void Start()
    {
        _timer.OnTimerCompleted += UpdateUser;
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

    private void UpdateUser()
    {
        User.UpdateHeld();
        User.ClearUsable();
    }

    protected virtual void OnDestroy()
    {
        _timer.OnTimerCompleted -= UpdateUser;
        _timer.OnTimerCompleted -= ClearInfo;
    }
}
