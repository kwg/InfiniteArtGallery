using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Network that encodes some kind of neural network
/// </summary>
/// <typeparam name="T">Evolved phenotype, that must be a neural network</typeparam>
public interface INetworkGenotype<T> where T: INetwork {

    /// <summary>
    /// Number of network modules
    /// </summary>
    /// <returns>Number of modules</returns>
    int NumberOfModules();

    /// <summary>
    /// Assign the module usage of a network phenotype back 
    /// to the genotype which spawned it for logging purposes.
    /// </summary>
    /// <param name="usage">Array where each index corresponds to a module 
    /// and contains the number of times that module was used 
    /// by the network phenotype.</param>
    void SetModuleUsage(int[] usage);

    /// <summary>
    /// Return module usage of the genotype
    /// </summary>
    /// <returns>Array of usage numbers</returns>
    int[] GetModuleUsage();
         
}

