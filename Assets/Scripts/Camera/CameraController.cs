using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    protected float _prefAspectRatio = 16f / 9f;
    [SerializeField]
    protected CameraChecker[] _checkers;
    private float _defaultSize = 5f;
    private Camera _camera;
    private float _currAspect;

    void Start()
    {
        _camera = GetComponent<Camera>();
        _currAspect = _camera.aspect;
        _camera.orthographicSize = _defaultSize * (_prefAspectRatio / _camera.aspect);
        foreach (CameraChecker check in _checkers)
        {
            check.OnCheckerNotRendered += IncreaseVertSize;
        }
    }

    private void OnValidate()
    {
        CameraChecker[] list = FindObjectsOfType<CameraChecker>();
        _checkers = list;
    }

    private void OnDestroy()
    {
        foreach (CameraChecker check in _checkers)
        {
            check.OnCheckerNotRendered -= IncreaseVertSize;
        }
    }

    private void Update()
    {
        if (Math.Round(_camera.aspect, 2) == Math.Round(_currAspect, 2)) return;

        _currAspect = _camera.aspect;
        _camera.orthographicSize = _defaultSize * (_prefAspectRatio / _camera.aspect);
    }

    private void IncreaseVertSize()
    {
        _camera.orthographicSize = _camera.orthographicSize + 0.05f;
    }
}
