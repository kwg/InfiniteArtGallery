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
    int archetypeIndex;

    List<TWEANNNode> nodes;

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
        this.archetypeIndex = archetypeIndex;

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
            nodes.Add(new TWEANNNode(fType, NTYPE.OUTPUT, innovation--));

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
        int outputStart = nodes.Count - numOutputs;
    }

    public TWEANN(TWEANNGenotype g)
    {
        archetypeIndex = g.GetArchetypeIndex();
        nodes = new List<TWEANNNode>(g.GetNodes().Count);
        int countIn = 0, countOut = 0;
        foreach(NodeGene node in g.GetNodes()) {
            TWEANNNode tempNode = new TWEANNNode(node.fType, node.nType, node.GetInnovation(), false, node.GetBias());
            nodes.Add(tempNode);
            if (node.nType == NTYPE.INPUT)
            {
                countIn++;
            }
            else if (node.nType == NTYPE.OUTPUT)
            {
                countOut++;
            }

        }
        this.numInputs = countIn;
        this.numOutputs = countOut;
        foreach(LinkGene link in g.GetLinks())
        {
            TWEANNNode source = GetNodeByInnovationID(link.GetSourceInnovation());
            TWEANNNode target = GetNodeByInnovationID(link.GetTargetInnovation());
            //TODO add asserts
            source.Connect(target, link.GetWeight(), link.GetInnovation(), false, false);
        }
    }


    public int NumInputs()
    {
        return numInputs;
    }

    public int NumOutputs()
    {
        return numOutputs;
    }

    public List<TWEANNNode> GetNodes()
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
        for(int j = 0; j < nodes.Count; j++)
        {
            nodes[j].ActivateAndTransmit();
        }


        double[] result = new double[numOutputs];
        // TODO loop through outputs and copy to result;
        for (int i = numInputs; i < nodes.Count; i++)
        {
            //result[i - numInputs] = nodes[i].GetSum();
            result[0] = nodes[nodes.Count - 1].Output();
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
        for(int i = 0; i < nodes.Count; i++)
        {
            if(nodes[i].GetInnovationID() == innovationID)
            {
                result = nodes[i];
            }
            else
            {
                //throw new System.ArgumentException("No node found with innovationID " + innovationID);
            }
        }

        return result;
    }
}
