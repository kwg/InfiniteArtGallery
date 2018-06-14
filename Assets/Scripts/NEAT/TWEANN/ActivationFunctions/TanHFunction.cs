using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TanH function for x. Uses system.Math.TanH(float x).
/// </summary>
public class TanHFunction : IActivationFunction
{

    private string name = "tanh";

    public float Function(float x)
    {
        return (float) System.Math.Tanh(x);
    }

    public string Name()
    {
        return name;
    }
}
