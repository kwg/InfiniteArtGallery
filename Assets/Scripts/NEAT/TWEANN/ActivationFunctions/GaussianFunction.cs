using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gauss function for x
/// </summary>
public class GaussianFunction : IActivationFunction
{

    private string name = "gauss";

    public double Function(double x)
    {
        return Gaussian(x, 1, 0);
    }

    public string Name()
    {
        return name;
    }

    double Gaussian(double x, double sig, double mu)
    {
        double result = double.NaN;

        double second = System.Math.Exp(-0.5 * ((x - mu) / sig) * ((x - mu) / sig));
        double first = (1 / (sig * System.Math.Sqrt(2 * System.Math.PI)));
        result =  first * second;

        return result;
    }
}
