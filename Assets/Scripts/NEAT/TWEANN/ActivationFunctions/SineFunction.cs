using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Sine function for x. Uses System.Math.Sin(float x).
/// </summary>
public class SineFunction : IActivationFunction {

    private string name = "sin";

    public float Function(float x)
    {
        return Mathf.Sin(x);
    }

    public string Name()
    {
        return name;
    }
}
