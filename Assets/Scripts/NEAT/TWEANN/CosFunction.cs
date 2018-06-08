using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Cos function for x. Uses System.Math.Cos(double x).
/// </summary>
public class CosFunction : IActivationFunction
{

    private string name = "cos";

    public double Function(double x)
    {
        return System.Math.Cos(x);
    }

    public string Name()
    {
        return name;
    }
}
