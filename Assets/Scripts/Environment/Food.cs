using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : PlayerUseable
{
    [SerializeField][Range (15, 40)]
    private int _temperature;

    protected override void Awake()
    {
        base.Awake();
        _timer.OnTimerCompleted += GetFood;
    }

    private void GetFood()
    {
        Debug.Log("You got some yumyum");
    }
    
}