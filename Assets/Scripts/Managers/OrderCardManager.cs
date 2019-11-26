using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderCardManager : MonoBehaviour
{
    public static OrderCardManager Instance { get; private set; }
    public bool Initialized { get; private set; }
    public static bool DoesExist { get => Instance != null; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

}
