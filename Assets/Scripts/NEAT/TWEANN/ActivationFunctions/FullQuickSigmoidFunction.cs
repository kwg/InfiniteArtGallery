using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullQuickSigmoidFunction : QuickSigmoidFunction {

    private string name = "sigmoid(approx)-full";

    public new float Function(float x)
    {
        return (2 * base.Function(x)) - 1;
    }

    public string Name()
    {
        return name;
    }
}
