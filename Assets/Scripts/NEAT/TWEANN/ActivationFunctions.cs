using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationFunctions  {

    Dictionary<FTYPE, IActivationFunction> activationFunctions;

    public ActivationFunctions()
    {
        activationFunctions.Add(FTYPE.SINE, new SineFunction());
    }

    public double Activation(FTYPE fType, double sum)
    {
        return activationFunctions[fType].Function(sum);
    }

}
