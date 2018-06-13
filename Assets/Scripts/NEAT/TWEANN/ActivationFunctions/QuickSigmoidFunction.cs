using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSigmoidFunction : IActivationFunction
{

    private string name = "sigmoid(approx)";

    public float Function(float x)
    {
        return 1.0f / (1.0f + quickExp(-x));
    }

    public string Name()
    {
        return name;
    }

    private float quickExp(float val)
    {
        long tmp = (long) (1512775 * val + 1072632447);
        return (float) System.BitConverter.Int64BitsToDouble(tmp << 32);
    }
}
