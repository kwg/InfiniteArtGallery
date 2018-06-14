using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SigmoidFunction : IActivationFunction
{
    private string name = "sigmoid";

    public float Function(float x)
    {
        return (1.0f / (1.0f + Mathf.Exp(-x)));
    }


    public string Name()
    {
        return name;
    }
}
