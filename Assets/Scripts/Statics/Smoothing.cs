using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Smoothing
{
    public static float SmoothStep(float t)
    {
        return t * t * (3f - 2f * t);
    }

    public static float SmootherStep(float t)
    {
        return t * t * t * (t * (6f * t - 15f) + 10f);
    }

    public static float InCosErp(float t)
    {
        return 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
    }

    public static float OutSinErp(float t)
    {
        return Mathf.Sin(t * Mathf.PI * 0.5f);
    }
}
