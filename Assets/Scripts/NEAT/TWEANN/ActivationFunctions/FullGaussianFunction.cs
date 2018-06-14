using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullGaussianFunction : IActivationFunction
{

    private string name = "gauss-full";

    public float Function(float x)
    {
        return Mathf.Exp(-x * x) * 2 - 1;
    }

    public string Name()
    {
        return name;
    }
}
