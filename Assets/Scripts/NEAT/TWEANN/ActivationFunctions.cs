using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// All activation functions for use by neural networks
/// </summary>
public class ActivationFunctions  {

    /* Map of all activation functions by their enum FTYPE */
    Dictionary<FTYPE, IActivationFunction> activationFunctions;

    public ActivationFunctions()
    {
        activationFunctions.Add(FTYPE.SINE, new SineFunction());
    }

    /// <summary>
    /// Use an activation function of a node by <see cref="FTYPE" />
    /// </summary>
    /// <param name="fType">Enum <see cref="FTYPE" /></param>
    /// <param name="sum">Input sent to node</param>
    /// <returns></returns>
    public double Activation(FTYPE fType, double sum)
    {
        return activationFunctions[fType].Function(sum);
    }

}
