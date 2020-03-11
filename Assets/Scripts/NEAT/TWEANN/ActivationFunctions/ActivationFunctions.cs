using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// All activation functions for use by neural networks
/// </summary>
public class ActivationFunctions  {

    /* Map of all activation functions by their enum FTYPE */
    static Dictionary<FTYPE, IActivationFunction> activationFunctions = new Dictionary<FTYPE, IActivationFunction>
    {
        { FTYPE.SINE, new SineFunction() },
        { FTYPE.COS, new CosFunction() },
        { FTYPE.GAUSS, new GaussianFunction() },
        { FTYPE.TANH, new TanHFunction() },
        { FTYPE.ID, new IDFunction() },
        { FTYPE.SIGMOID, new SigmoidFunction() },
        { FTYPE.ABSVAL, new ClippedAbsValFunction() },
        { FTYPE.PIECEWISE, new FullLinearPiecewiseFunction() },
        { FTYPE.HLPIECEWISE, new HalfLinearPiecewiseFunction() },
        { FTYPE.SAWTOOTH, new SawtoothFunction() },
        { FTYPE.FULLSAWTOOTH, new FullSawtoothFunction() },
        { FTYPE.TRIANGLEWAVE, new TriangleWaveFunction() },
        { FTYPE.SQUAREWAVE, new SquareWaveFunction() },
        { FTYPE.FULLSIGMOID, new FullSigmoidFunction() },
        { FTYPE.FULLGAUSS, new FullGaussianFunction() },
        { FTYPE.APPROX, new QuickSigmoidFunction() },
        { FTYPE.FULLAPPROX, new FullQuickSigmoidFunction() }
    };


    private static FunctionCollection functionCollection = new FunctionCollection();

    public static void ActivateAllFunctions()
    {
        foreach(FTYPE f in System.Enum.GetValues(typeof(FTYPE)))
        {
            functionCollection.AddFunction(f);
        }
    }

    public static List<FTYPE> GetFunctionList()
    {
        return functionCollection.GetFunctionList();
    }

    public static bool ActivateFunction(FTYPE fType)
    {
        return functionCollection.AddFunction(fType); 
    }

    public static bool ActivateFunction(List<FTYPE> fTypes)
    {
        return functionCollection.AddFunction(fTypes);
    }

    public static bool DeactivateFunction(FTYPE fType)
    {
        return functionCollection.RemoveFunction(fType);
    }

    public static int GetActiveFunctionCount()
    {
        return functionCollection.Count;
    }


    /// <summary>
    /// Use an activation function of a node by <see cref="FTYPE" />
    /// </summary>
    /// <param name="fType">Enum <see cref="FTYPE" /></param>
    /// <param name="sum">Input sent to node</param>
    /// <returns></returns>
    public static float Activation(FTYPE fType, float sum)
    {
        return activationFunctions[fType].Function(sum);
    }

    /// <summary>
    /// Get a random activation function
    /// </summary>
    /// <returns>FTYPE</returns>
    public static FTYPE GetRandom()
    {
        return functionCollection.GetRandom();
    }

    public static FTYPE GetWeightedRandom()
    {
        return functionCollection.GetWeightedRandom();
    }

    //public static List<FTYPE> GetInactiveFunctions()
    //{
    //    return inactiveFunctions;
    //}

    public static string ActivationName(FTYPE fType)
    {
        string result = "";
        result = activationFunctions[fType].Name();
        return result;
    }

}
