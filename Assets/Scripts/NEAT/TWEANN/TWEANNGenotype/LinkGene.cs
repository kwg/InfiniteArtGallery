using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkGene : Gene {

    protected long sourceInnovation, targetInnovation;
    protected float weight;
    protected bool active;

    public LinkGene(long sourceInnovation, long targetInnovation, float weight, long innovation) : base(innovation)
    {
        this.sourceInnovation = sourceInnovation;
        this.targetInnovation = targetInnovation;
        this.weight = weight;
        active = true;
    }

    public bool IsActive() // Always active
    {
        return active;
    }

    public void SetActive(bool active)
    {
        this.active = active;
    }
    
    public bool IsReccurent()
    {
        return false;
    }

    public float GetWeight()
    {
        return weight;
    }

    public void SetWeight(float weight)
    {
        this.weight = weight;
    }

    public long GetSourceInnovation()
    {
        return sourceInnovation;
    }

    public long GetTargetInnovation()
    {
        return targetInnovation;
    }

    public LinkGene Clone()
    {
        return new LinkGene(sourceInnovation, targetInnovation, weight, innovation);
    }

    // TODO ToString()

    public override string ToString()
    {
        string result = "(";
        result += "inno=" + innovation;
        result += ",source=" + sourceInnovation;
        result += ",target=" + targetInnovation;
        result += ",weight=" + weight;
        result += ",active=" + IsActive();
        //result += ",recurrent=" + IsRecurrent();
        //result += ",frozen=" + IsFrozen();
        result +=  ")";
        return result;
    }
}
