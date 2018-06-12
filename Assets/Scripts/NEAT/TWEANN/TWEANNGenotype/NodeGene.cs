﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGene : Gene {

    public FTYPE fType;
    public NTYPE nType;
    public float bias;

    public NodeGene(NTYPE nType, FTYPE fType, long innovation) : base(innovation)
    {
        this.fType = fType;
        this.nType = nType;
    }

    public float GetBias()
    {
        return bias;
    }

    public void SetBias(float bias)
    {
        this.bias = bias;
    }

    public bool IsEqualTo(System.Object o)
    {
        NodeGene other = (NodeGene) o;
        return innovation == other.innovation;
    }

    public NodeGene Clone()
    {
        return new NodeGene(nType, fType, innovation);
    }

    // TODO ToString()

    public override string ToString()
    {
        return "Node: " + innovation + ", NTYPE: " + nType + ", FTYPE: " + fType + ", bias: " + bias;
    }
}
