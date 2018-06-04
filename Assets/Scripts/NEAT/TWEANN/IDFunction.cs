using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Returns itself
/// </summary>
public class IDFunction : IActivationFunction
{

    private string name = "ID";

    public double Function(double x)
    {
        return x;
    }

    public string Name()
    {
        return name;
    }
}

