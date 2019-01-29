using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodeGene : Gene {

    public FTYPE fTYPE;
    public NTYPE nTYPE;
    public float bias;

    public NodeGene(NTYPE nTYPE, FTYPE fTYPE, long innovation) : base(innovation)
    {
        this.fTYPE = fTYPE;
        this.nTYPE = nTYPE;
        bias = 0.0f;
    }

    public float GetBias()
    {
        return bias;
    }

    public void SetBias(float bias)
    {
        this.bias = bias;
    }

    public void SetFTYPE(FTYPE fTYPE)
    {
        this.fTYPE = fTYPE;
    }

    public FTYPE GetFTYPE()
    {
        return fTYPE;
    }

    public bool IsEqualTo(System.Object o)
    {
        NodeGene other = (NodeGene) o;
        return Innovation == other.Innovation;
    }

    public NodeGene Clone()
    {
        return new NodeGene(nTYPE, fTYPE, Innovation);
    }

    public override string ToString()
    {
        string result = "(";
        result += "(inno=" + Innovation;
        result += ",ftype=" + ActivationFunctions.ActivationName(fTYPE);
        result += ",ntype=" + nTYPE;
        //result += ",frozen=" + IsFrozen();
        result += ",bias=" + GetBias();
        result +=  ")";
        return result;
    }
}
