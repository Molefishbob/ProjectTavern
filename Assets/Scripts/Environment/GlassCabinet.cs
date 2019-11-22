using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

public class GlassCabinet : PlayerUseable
{
    
    protected override void Awake()
    {
        base.Awake();
        _timer.OnTimerCompleted += GetNewGlass;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _timer.OnTimerCompleted -= GetNewGlass;
    }

    public void GetNewGlass()
    {
        Glass glass = LevelManager.Instance.GetGlass();
        glass.Use(User);
    }
}
