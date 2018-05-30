using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TWEANN : INetwork
{

    long ID;
    int numInputs;
    int numOutputs;

    List<NodeGene> nodes;




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
