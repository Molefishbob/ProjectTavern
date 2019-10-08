using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;
using static Managers.AIManager;

[CreateAssetMenu(fileName = "New base actions",menuName = "Actions/Base Actions",order = 2)]
public class BaseActions : Action
{
    /// <summary>
    /// Moves the AI to the desired location
    /// </summary>
    /// <param name="polyNav">PolyNavAgent</param>
    /// <param name="pos">Move position</param>
    public void Move(PolyNavAgent polyNav, Vector3 pos)
    {
        polyNav.SetDestination(pos);
    }

    /// <summary>
    /// Tells opponent to set state to fight
    /// </summary>
    /// <param name="opponent">The selected fight opponent</param>
    public void Fight(Customer opponent)
    {
        opponent.ChangeState(State.Fighting);
    }
}