using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Sine function for x. Uses System.Math.Sin(double x).
/// </summary>
public class SineFunction : IActivationFunction {

    private string name = "sin";

    public double Function(double x)
    {
        return System.Math.Sin(x);
    }

    public string Name()
    {
        return name;
    }
}
