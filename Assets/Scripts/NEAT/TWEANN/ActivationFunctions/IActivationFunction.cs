using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Interface for Activation functions
/// </summary>
public interface IActivationFunction {

    /// <summary>
    /// Function from real number to real number
    /// </summary>
    /// <param name="x">Function input</param>
    /// <returns>Function output</returns>
    float Function(float x);

    /// <summary>
    /// Name of this function
    /// </summary>
    /// <returns>Function name</returns>
    string Name();

}
