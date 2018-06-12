using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gauss function for x
/// </summary>
public class GaussianFunction : IActivationFunction
{

    private string name = "gauss";

    public float Function(float x)
    {
        return Gaussian(x, 1, 0);
    }

    public string Name()
    {
        return name;
    }

    float Gaussian(float x, float sig, float mu)
    {
        float result = float.NaN;

        float second = Mathf.Exp(-0.5f * ((x - mu) / sig) * ((x - mu) / sig));
        float first = (1f / (sig * Mathf.Sqrt(2 * Mathf.PI)));
        result =  first * second;

        return result;
    }
}
