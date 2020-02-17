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

    static List<FTYPE> activeFunctions = new List<FTYPE>();
    static List<FTYPE> inactiveFunctions = new List<FTYPE>
    {
        FTYPE.SINE,
        //FTYPE.COS,
        FTYPE.GAUSS,
        FTYPE.TANH,
        FTYPE.ID,
        //FTYPE.SIGMOID,
        //FTYPE.SIGMOID, 
        FTYPE.ABSVAL,
        //FTYPE.PIECEWISE,
        //FTYPE.HLPIECEWISE,
        FTYPE.SAWTOOTH,
        //FTYPE.FULLSAWTOOTH,
        //FTYPE.TRIANGLEWAVE,
        FTYPE.SQUAREWAVE,
        //FTYPE.FULLSIGMOID,
        //FTYPE.FULLGAUSS,
        //FTYPE.APPROX,
        //FTYPE.FULLAPPROX
    };

    public static void ActivateAllFunctions()
    {
        List<FTYPE> lastInactive = new List<FTYPE>(inactiveFunctions.Count);
        foreach(FTYPE inactive in inactiveFunctions)
        {
            lastInactive.Add(inactive);
        }
        foreach(FTYPE f in lastInactive)
        {
            ActivateFunction(f);
        }
    }


    public static bool ActivateFunction(FTYPE fType)
    {
        bool successful = false;
        if (inactiveFunctions.Contains(fType))
        {
            activeFunctions.Add(fType);
            inactiveFunctions.Remove(fType);
            successful = true;
        }
        return successful;
    }

    public static bool ActivateFunction(List<FTYPE> fTypes)
    {
        bool successful = false;
        foreach(FTYPE fType in fTypes)
        {
            if (inactiveFunctions.Contains(fType))
            {
                activeFunctions.Add(fType);
                inactiveFunctions.Remove(fType);
                successful = true;
            }
        }
        return successful;
    }

    public static bool DeactivateFunction(FTYPE fType)
    {
        bool successful = false;
        if (activeFunctions.Contains(fType))
        {
            inactiveFunctions.Add(fType);
            activeFunctions.Remove(fType);
            successful = true;
        }
        return successful;
    }

    public static int GetActiveFunctionCount()
    {
        return activeFunctions.Count;
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
    public static FTYPE RandomFTYPE2()
    {
        FTYPE type = FTYPE.ID;
        int rnd = Random.Range(0, 6) + 1;
        switch (rnd)
        {
            case 1:
                type = FTYPE.TANH;
                break;
            case 2:
                type = FTYPE.SIGMOID;
                break;
            case 3:
                type = FTYPE.SINE;
                break;
            case 4:
                type = FTYPE.COS;
                break;
            case 5:
                type = FTYPE.GAUSS;
                break;
            case 6:
                type = FTYPE.ID;
                break;
            case 7:
                type = FTYPE.FULLAPPROX;
                break;
            case 8:
                type = FTYPE.APPROX;
                break;
            case 9:
                type = FTYPE.ABSVAL;
                break;
            case 10:
                type = FTYPE.PIECEWISE;
                break;
            case 11:
                type = FTYPE.HLPIECEWISE;
                break;
            case 12:
                type = FTYPE.SAWTOOTH;
                break;
            case 13:
                type = FTYPE.STRETCHED_TANH;
                break;
            case 14:
                type = FTYPE.RE_LU;
                break;
            case 15:
                type = FTYPE.SOFTPLUS;
                break;
            case 16:
                type = FTYPE.LEAKY_RE_LU;
                break;
            case 17:
                type = FTYPE.FULLSAWTOOTH;
                break;
            case 18:
                type = FTYPE.TRIANGLEWAVE;
                break;
            case 19:
                type = FTYPE.SQUAREWAVE;
                break;
            case 20:
                type = FTYPE.FULLSIGMOID;
                break;
            case 21:
                type = FTYPE.FULLGAUSS;
                break;
            case 22:
                type = FTYPE.SIL;
                break;
            case 23:
                type = FTYPE.DSIL;
                break;
            default:
                break;

        }

        return type;
    }

    public static FTYPE RandomFTYPE()
    {
        Debug.Log("count for ActivationFunctions active functions = " + activeFunctions.Count);
        Debug.Log(activeFunctions[0].ToString());

        return activeFunctions[Random.Range(0, activeFunctions.Count)];
    }

    public static List<FTYPE> GetInactiveFunctions()
    {
        return inactiveFunctions;
    }

    public static string ActivationName(FTYPE fType)
    {
        string result = "";
        result = activationFunctions[fType].Name();
        return result;
    }

}
