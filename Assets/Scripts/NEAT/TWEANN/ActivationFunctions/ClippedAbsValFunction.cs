using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClippedAbsValFunction : HalfLinearPiecewiseFunction
{

    private string name = "abs";

    public new float Function(float x)
    {
        return base.Function(Mathf.Abs(x));
    }

    public new string Name()
    {
        return name;
    }
}