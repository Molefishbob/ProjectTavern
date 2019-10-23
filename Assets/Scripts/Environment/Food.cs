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

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _timer.OnTimerCompleted -= GetFood;
    }

    private void GetFood()
    {
        User.CurrentlyHeld = PlayerState.Holdables.Food;
        Debug.Log("You got some yumyum");
    }
}