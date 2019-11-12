using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZFixer : MonoBehaviour
{
    [SerializeField, Tooltip("How many frames between changes")] private int _refreshRate = 5;
    private int _counter = 0;

#pragma warning disable CS0649
    [SerializeField] private SpriteRenderer _renderer;
#pragma warning restore CS0649

    private void Awake()
    {
        if (_renderer == null)
            _renderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        _counter++;
        if (_counter <= _refreshRate)
        {
            _counter = 0;
            _renderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 10);
        }
    }
}
