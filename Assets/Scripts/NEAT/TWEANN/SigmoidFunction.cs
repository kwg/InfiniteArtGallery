using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SigmoidFunction : IActivationFunction
{
    private string name = "sigmoid";

    public double Function(double x)
    {
        return (1.0 / (1.0 + System.Math.Exp(-x)));
    }


    public string Name()
    {
        return name;
    }
}
