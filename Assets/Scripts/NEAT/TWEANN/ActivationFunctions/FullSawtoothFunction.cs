using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullSawtoothFunction : IActivationFunction
{

    private string name = "sawtooth-full";

    public float Function(float x)
    {
        return FullSawtooth(x, 1);
    }

    private float FullSawtooth(float x, float a)
    {
        return 2 * ((x / a) - Mathf.Floor(1 / 2 + x / a));
    }

    public string Name()
    {
        return name;
    }
}
