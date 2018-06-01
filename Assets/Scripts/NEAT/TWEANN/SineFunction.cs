using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineFunction : IActivationFunction {

    public SineFunction()
    {

    }

    public double Function(double x)
    {
        return Mathf.Sin((float) x);
    }

    public string Name()
    {
        return "sin";
    }
}
