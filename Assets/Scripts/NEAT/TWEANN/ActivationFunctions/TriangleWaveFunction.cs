using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleWaveFunction : FullSawtoothFunction
{

    private string name = "triangle";

    public new float Function(float x)
    {
        return Mathf.Abs(base.Function(x));
    }

    public new string Name()
    {
        return name;
    }
}