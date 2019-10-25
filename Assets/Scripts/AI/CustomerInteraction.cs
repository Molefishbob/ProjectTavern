using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// How customer Interacts with player
/// </summary>
public class CustomerInteraction : PlayerUseable
{
    // Reference to the customer script in parent
    private Customer _customer;

    protected override void Awake()
    {
        base.Awake();
        _timer.OnTimerCompleted += Interract;

        // Lets get the customer script from parent
        _customer = transform.parent.GetComponent<Customer>();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _timer.OnTimerCompleted -= Interract;
    }

    // Let's poll the customer state for now.
    // TODO: Change to event based
    private void Update()
    {
        switch (_customer.CurrentState)
        {
            case Managers.AIManager.State.None:
                break;
            case Managers.AIManager.State.Moving:
                break;
            case Managers.AIManager.State.Waiting:
                break;
            case Managers.AIManager.State.Served:
                break;
            case Managers.AIManager.State.PassedOut:
                _interactionTime = 3;
                _requiresEmptyHands = true;
                break;
            case Managers.AIManager.State.Fighting:
                _requiresEmptyHands = true;
                _interactionTime = 3;
                break;
            case Managers.AIManager.State.Ordered:
                _requiresEmptyHands = false;
                _interactionTime = 0.5f;
                break;
            default:
                break;
        }
    }

    private void Interract()
    {
        switch (_customer.CurrentState)
        {
            case Managers.AIManager.State.None:
                break;
            case Managers.AIManager.State.Moving:
                break;
            case Managers.AIManager.State.Waiting:
                break;
            case Managers.AIManager.State.Served:
                break;
            case Managers.AIManager.State.PassedOut:
                break;
            case Managers.AIManager.State.Fighting:
                break;
            case Managers.AIManager.State.Ordered:
                break;
            default:
                break;
        }
    }
}
