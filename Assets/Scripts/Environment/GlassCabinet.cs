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
        User.CurrentlyHeld = PlayerState.Holdables.Glass;
        glass.transform.parent = User.transform;
        glass.transform.position = glass.transform.parent.position + new Vector3(0, 0.4f, 0);
        glass.gameObject.GetComponent<CircleCollider2D>().enabled = false;
    }
}
