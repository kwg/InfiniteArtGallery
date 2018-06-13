using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareWaveFunction : IActivationFunction
{

    private string name = "sin";

    public float Function(float x)
    {
        return SquareWave(x, 1, 1);
    }

    private float SquareWave(float x, float p, float a)
    {
        float result = 0;
        float sineCalculation = Mathf.Sin(2 * Mathf.PI / p * x);
        if(sineCalculation != 0) result = a * 1 / sineCalculation * Mathf.Abs(sineCalculation);
        return result;
    }

    public string Name()
    {
        return name;
    }
}
