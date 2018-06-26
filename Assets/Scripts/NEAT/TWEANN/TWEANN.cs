﻿using System.Collections;
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
    public int ArchetypeIndex { get; set; }

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
        ArchetypeIndex = archetypeIndex;

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
                nodes[inputSources[k]].Connect(nodes[numInputs + j], RandomGenerator.NextGaussian(), linkInnovationBound - (j * numInputs) - inputSources[k], false, false);
            }

        }
        int outputStart = nodes.Count - numOutputs;
    }

    public TWEANN(TWEANNGenotype g)
    {
        ArchetypeIndex = g.GetArchetypeIndex();
        nodes = new List<TWEANNNode>(g.GetNodes().Count);
        int countIn = 0, countOut = 0;
        foreach(NodeGene node in g.GetNodes()) {
            TWEANNNode tempNode = new TWEANNNode(node.fTYPE, node.nTYPE, node.Innovation, false, node.GetBias());
            nodes.Add(tempNode);
            if (node.nTYPE == NTYPE.INPUT)
            {
                countIn++;
            }
            else if (node.nTYPE == NTYPE.OUTPUT)
            {
                countOut++;
            }

        }
        numInputs = countIn;
        numOutputs = countOut;
        foreach(LinkGene link in g.GetLinks())
        {
            TWEANNNode source = GetNodeByInnovationID(link.GetSourceInnovation());
            TWEANNNode target = GetNodeByInnovationID(link.GetTargetInnovation());
            //TODO add asserts
            if (source == null) throw new System.Exception("Source not found with innovation " + link.GetSourceInnovation() + " \n " + ToString());
            if (target == null) throw new System.Exception("Target not found with innovation " + link.GetTargetInnovation() + " \n " + ToString());

            source.Connect(target, link.GetWeight(), link.Innovation, false, false);
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

    public float[] Process(float[] inputs)
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


        float[] result = new float[numOutputs];
        //Debug.Log("Number of outputs in result: " + result.Length);
        // TODO loop through outputs and copy to result;
        for (int i = nodes.Count - numOutputs, c = 0; i < nodes.Count; i++, c++)
        {
            result[c] = nodes[i].Output();
            //result[i] = nodes[nodes.Count - 1].Output();
        }

        // TODO option for importing CPPNs from original Picbreeder
        return result;
    }

    public void Flush()
    {
        throw new System.NotImplementedException();
    }

    public float GetWeightBetween(long sourceInnovation, long targetInnovation)
    {
        return GetNodeByInnovationID(sourceInnovation).GetLinkToTargetNode(GetNodeByInnovationID(targetInnovation)).GetWeight();
    }


    private TWEANNNode GetNodeByInnovationID(long innovationID)
    {
        TWEANNNode result = null;
        for(int i = 0; i < nodes.Count; i++)
        {
            if(nodes[i].GetInnovation() == innovationID)
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

    public override string ToString()
    {
        string result = "";
        result += numInputs + " Inputs\n";
		result += numOutputs + " Outputs\n";
		//result += numModes + " Modes\n";
		result += "Forward\n";
		for (int i = 0; i < nodes.Count; i++)
        {
			result += nodes[i] + "\n";
		}
        return result;
    }
}
