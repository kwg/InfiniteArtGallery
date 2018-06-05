using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullNodeGene : NodeGene {

    double bias;

    public FullNodeGene(NTYPE nType, FTYPE fType, long innovation, double bias) : base(nType, fType, innovation)
    {
        this.bias = bias;
    }

    public double GetBias()
    {
        return bias;
    }
}
