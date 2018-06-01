using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Sine function for x. Uses Mathf.Sin(float x). X is cast as float for the function.
/// </summary>
public class SineFunction : IActivationFunction {

    public double Function(double x)
    {
        return Mathf.Sin((float) x);
    }

    public string Name()
    {
        return "sin";
    }
}
