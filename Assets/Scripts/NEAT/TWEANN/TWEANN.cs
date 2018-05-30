using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TWEANN : INetwork
{

    long ID;
    int numInputs;
    int numOutputs;

    List<NodeGene> nodes;

    public TWEANN(int numInputs, int numOutputs, bool featureSelective, FTYPE fType, int numModes, int archetypeIndex)
    {

    }


    public int NumInputs()
    {
        return numInputs;
    }

    public int NumOutputs()
    {
        return numOutputs;
    }

    public void Flush()
    {
        throw new System.NotImplementedException();
    }
}
