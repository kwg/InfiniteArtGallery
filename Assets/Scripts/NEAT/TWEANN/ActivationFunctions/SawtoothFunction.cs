using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawtoothFunction : IActivationFunction
{

    private string name = "sawtooth";

    public float Function(float x)
    {
        return x - Mathf.Floor(x);
    }

    public string Name()
    {
        return name;
    }
}
