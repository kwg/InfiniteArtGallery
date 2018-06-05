using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGene : Gene {

    public FTYPE fType;
    public NTYPE nType;

    public NodeGene(NTYPE nType, FTYPE fType, long innovation) : base(innovation)
    {
        this.fType = fType;
        this.nType = nType;
    }

    public double GetBias()
    {
        return 0.0;
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
}
