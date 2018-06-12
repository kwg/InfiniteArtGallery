using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TanH function for x. Uses system.Math.TanH(double x).
/// </summary>
public class TanHFunction : IActivationFunction
{

    private string name = "tanh";

    public double Function(double x)
    {
        return System.Math.Tanh(x);
    }

    public string Name()
    {
        return name;
    }
}
