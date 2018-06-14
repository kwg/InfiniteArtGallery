using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullNodeGene : NodeGene {

    float bias;

    public FullNodeGene(NTYPE nType, FTYPE fType, long innovation, float bias) : base(nType, fType, innovation)
    {
        this.bias = bias;
    }

    public float GetBias()
    {
        return bias;
    }
}
