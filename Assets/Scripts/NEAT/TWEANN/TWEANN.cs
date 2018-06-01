using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TWEANN : INetwork
{

    long ID;
    int numInputs;
    int numOutputs;
    double[] preferenceFatigue;
    int neuronsPerModule;

    List<TWEANNNode> nodes;

    /// <summary>
    /// Create a new random TWEANN
    /// </summary>
    /// <param name="numInputs">Number of input sensors</param>
    /// <param name="numOutputs">Number of policy neurons per node</param>
    /// <param name="featureSelective">If true, nerons are only sparsely connected, fully connected otherwise</param>
    /// <param name="fType">Type of activation function on neurons</param>
    /// <param name="numModes">Number of modes for a multitask network, should be 1 if not multitask</param>
    /// <param name="archetypeIndex">Archtype to align with for crossover</param>
    public TWEANN(int numInputs, int numOutputs, bool featureSelective, FTYPE fType, int numModes, int archetypeIndex)
    {
        this.numInputs = numInputs;
        this.numOutputs = numOutputs;
        preferenceFatigue = new double[numOutputs];

        nodes = new List<TWEANNNode>(numInputs + numOutputs);

        long innovation = -1;

        for(int i = 0; i < numInputs; i++)
        {
            TWEANNNode n = new TWEANNNode(fType, NTYPE.INPUT, innovation--);
            nodes.Add(n);
        }

        long linkInnovationBound = innovation - 1;

        for(int j = 0; j < numOutputs; j++)
        {
            int[] inputSources = new int[numInputs]; // HACK making this be fully connected for now
            for(int i = 0; i < numInputs; i++)
            {
                inputSources[i] = i;
            }
            for(int i = 0; i < inputSources.Length; i++)
            {
                TWEANNNode output = nodes[numInputs + j];
                nodes[inputSources[i]].Connect(output, Random.Range(-1, 1), linkInnovationBound - (j * numInputs) - inputSources[i], false, false);
            }

        }

        int outputStart = nodes.Count - numOutputs;




    }


    public int NumInputs()
    {
        return numInputs;
    }

    public int NumOutputs()
    {
        return numOutputs;
    }

    public double[] Process(double[] inputs)
    {
        double[] result = new double[0];

        // Load inputs
        // TODO Sanity checks
        for(int i = 0; i < numInputs; i++)
        {
            nodes[i].Load(inputs[i]);
        }

        // Activate nodes in forward order
        for(int j = 0; j <nodes.Count; j++)
        {
            nodes[j].ActivateAndTransmit();
        }

        // TODO option for importing CPPNs from original Picbreeder

        // Outputs
        // Not using modes
        double[] preferences = new double[] { 1.0 };  // HACK hard coded in value for single mode

        // Subtract fatigue
        for(int i = 0; i < preferenceFatigue.Length; i++)
        {
            preferences[i] -= preferenceFatigue[i];
        }

        neuronsPerModule = 1;

        double[] outputs = new double[neuronsPerModule];

        outputs = ModuleOutput(0);


        return result;
    }

    public double[] ModuleOutput(int mode)
    {

        throw new System.NotImplementedException();
    }

    public void Flush()
    {
        throw new System.NotImplementedException();
    }

}
