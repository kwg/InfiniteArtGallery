using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for neural network controllers
/// </summary>
public interface INetwork {

    /// <summary>
    /// Number of nodes in the input layer
    /// </summary>
    /// <returns>Number of input nodes</returns>
    int NumInputs();

    /// <summary>
    /// Number of nodes in the output layer
    /// </summary>
    /// <returns>Number of outputs</returns>
    int NumOutputs();

    float[] Process(float[] inputs);

    /// <summary>
    /// Clear an internal state
    /// </summary>
    void Flush();

}
