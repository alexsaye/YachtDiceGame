using System;
using UnityEngine;

/// <summary>
/// Provides easing functions for smooth interpolation.
/// </summary>
public static class Ease
{
    public static readonly Func<float, float> Linear = t => t;
    public static Func<float, float> PowIn(float pow) => t => Mathf.Pow(t, pow);
    public static Func<float, float> PowOut(float pow) => t => 1 - Mathf.Pow(1 - t, pow);
    public static Func<float, float> PowInOut(float pow) => t => t < 0.5 ? Mathf.Pow(t * 2, pow) / 2 : 1 - Mathf.Pow(1 - (t - 0.5f) * 2, pow) / 2;
    public static readonly Func<float, float> QuadIn = PowIn(2);
    public static readonly Func<float, float> QuadOut = PowOut(2);
    public static readonly Func<float, float> QuadInOut = PowInOut(2);
}
