using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanableMess : PlayerUseable
{
    public override void Use()
    {
        Debug.Log("You cleaned some rancid puke");
        //TODO: something else than destroy
        Destroy(gameObject);
    }

    

}
