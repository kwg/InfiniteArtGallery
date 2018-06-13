using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullSigmoidFunction : SigmoidFunction
{

    private string name = "sigmoid-full";

    public new float Function(float x)
    {
        return (2 * base.Function(x)) -1;
    }

    public new string Name()
    {
        return name;
    }
}
