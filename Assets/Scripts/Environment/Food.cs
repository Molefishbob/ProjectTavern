using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : PlayerUseable
{
    [SerializeField][Range (0, 30)]
    private int _temperature;

    public override void Use()
    {
        
    }

}