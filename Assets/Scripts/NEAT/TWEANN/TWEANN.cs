using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Topology and Weight Evolving Neural Network
/// </summary>
public class TWEANN : INetwork
{

    long ID;
    int numInputs;
    int numOutputs;

    TWEANNNode[] nodes;

    /// <summary>
    /// Create a new random TWEANN
    /// </summary>
    /// <param name="numInputs">Number of input sensors</param>
    /// <param name="numOutputs">Number of policy neurons per node</param>
    /// <param name="featureSelective">If true, nerons are only sparsely connected, fully connected otherwise</param>
    /// <param name="fType">Type of activation function on neurons</param>
    /// <param name="archetypeIndex">Archtype to align with for crossover</param>
    public TWEANN(int numInputs, int numOutputs, bool featureSelective, FTYPE fType, int archetypeIndex)
    {
        this.numInputs = numInputs;
        this.numOutputs = numOutputs;

        nodes = new TWEANNNode[numInputs + numOutputs];

        long innovation = -1;

        for(int i = 0; i < numInputs; i++)
        {
            TWEANNNode n = new TWEANNNode(fType, NTYPE.INPUT, innovation--);
            nodes[i] = n;
        }

        long linkInnovationBound = innovation - 1;

        for(int j = 0; j < numOutputs; j++)
        {
            nodes[numInputs + j] = new TWEANNNode(fType, NTYPE.OUTPUT, innovation--);

            int[] inputSources = new int[numInputs]; // HACK making this be fully connected for now
            for(int i = 0; i < numInputs; i++)
            {
                inputSources[i] = i;
            }
            for(int k = 0; k < inputSources.Length; k++)
            {
                // FIXME !weight is set to 0.5 for testing!
                nodes[inputSources[k]].Connect(nodes[numInputs + j], 0.5, linkInnovationBound - (j * numInputs) - inputSources[k], false, false);
            }

        }

        int outputStart = nodes.Length - numOutputs;




    }


    public int NumInputs()
    {
        return numInputs;
    }

    public int NumOutputs()
    {
        return numOutputs;
    }

    public TWEANNNode[] GetNodes()
    {
        return nodes;
    }

    public double[] Process(double[] inputs)
    {

        // Load inputs
        // TODO Sanity checks
        for(int i = 0; i < numInputs; i++)
        {
            nodes[i].Load(inputs[i]);
        }

        // Activate nodes in forward order
        for(int j = 0; j < nodes.Length; j++)
        {
            nodes[j].ActivateAndTransmit();
        }


        double[] result = new double[numOutputs];
        // TODO loop through outputs and copy to result;
        for(int i = numInputs; i < nodes.Length; i++)
        {
            //result[i - numInputs] = nodes[i].GetSum();
            result[0] = nodes[nodes.Length - 1].Output();
        }

        // TODO option for importing CPPNs from original Picbreeder
        return result;
    }

    public void Flush()
    {
        throw new System.NotImplementedException();
    }

    public double GetWeightBetween(long sourceInnovation, long targetInnovation)
    {
        return GetNodeByInnovationID(sourceInnovation).GetLinkToTargetNode(GetNodeByInnovationID(targetInnovation)).GetWeight();
    }


    private TWEANNNode GetNodeByInnovationID(long innovationID)
    {
        TWEANNNode result = null;

        foreach(TWEANNNode node in nodes)
        {
            if(node.GetInnovationID() == innovationID)
            {
                result = node;
            }
            else
            {
                throw new System.ArgumentException("No node found with innovationID " + innovationID);
            }
        }

        return result;
    }
}
