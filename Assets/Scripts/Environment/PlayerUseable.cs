using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameInput;

public abstract class PlayerUseable : MonoBehaviour, IUseable
{
    private bool _withinRange;
    protected ScaledOneShotTimer _timer;
    [SerializeField][Tooltip("How long the action takes to finish")]
    protected float _interactionTime = 2;

    protected virtual void Awake()
    {
        _timer = gameObject.AddComponent<ScaledOneShotTimer>();
    }

    public virtual void Use()
    {
        
    }

    public virtual void Use(GameObject player)
    {
        if (_withinRange) _timer.StartTimer(_interactionTime);
    }

    protected void Update()
    {
        if(!_withinRange && _timer.IsRunning)
        {
            InterruptAction();
        }
    }

    public virtual void InterruptAction() 
    {
        _timer.StopTimer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerMovement>() != null)
        {
            _withinRange = true;
            collision.GetComponent<PlayerMovement>()._useableObject = gameObject;
        }
            
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerMovement>() != null)
        {
            _withinRange = true;
            collision.GetComponent<PlayerMovement>()._useableObject = null;
        }

    }
}
