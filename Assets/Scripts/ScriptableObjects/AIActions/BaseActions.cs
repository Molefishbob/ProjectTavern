using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Managers.BeverageManager;

[CreateAssetMenu(fileName = "New base actions",menuName = "Actions/Base Actions",order = 2)]
public class BaseActions : Action
{
    public void Move(Vector3 movementVector, float turnAngle)
    {
        // TODO: Movement Action
        // TODO: Turning Action
    }

    public void Drink(/* add drink */)
    {
        // TODO: Make drink
    }

    public void Fight(/*add enemy */)
    {
        // TODO: Fight enemy
    }

    public void PassOut()
    {
        // TODO: Passout logic
        //       Also add piss/vomit reflex
    }

    public Beverage Order()
    {
        // TODO: Place order
        return 0;
    }
}