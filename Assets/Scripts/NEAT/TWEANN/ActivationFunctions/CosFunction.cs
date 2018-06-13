using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Cos function for x. Uses System.Math.Cos(float x).
/// </summary>
public class CosFunction : IActivationFunction
{

    private string name = "cos";

    public float Function(float x)
    {
        return Mathf.Cos(x);
    }

    public string Name()
    {
        return name;
    }
}
