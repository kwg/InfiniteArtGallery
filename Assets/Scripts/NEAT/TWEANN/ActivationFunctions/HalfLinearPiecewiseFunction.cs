using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfLinearPiecewiseFunction : IActivationFunction
{

    private string name = "piecewise-half";

    public float Function(float x)
    {
        return Mathf.Max(0, Mathf.Min(1, x));
    }

    public string Name()
    {
        return name;
    }
}
