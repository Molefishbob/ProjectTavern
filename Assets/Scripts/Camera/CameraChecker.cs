using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

public class CameraChecker : MonoBehaviour
{
    private Renderer _renderer;
    public event GenericEvent OnCheckerNotRendered;
    private int ins = 0;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (ins < 3)
        {
            ins++;
        }
        else if (ins == 3)
        {
            if (_renderer.isVisible) return;

            OnCheckerNotRendered?.Invoke();
        }
    }

}
