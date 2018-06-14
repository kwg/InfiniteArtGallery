using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullLinearPiecewiseFunction : IActivationFunction
{

    private string name = "piecewise-full";

    public float Function(float x)
    {
        return Mathf.Max(-1, Mathf.Min(1, x));
    }

    public string Name()
    {
        return name;
    }
}