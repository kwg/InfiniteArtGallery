using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkGene : Gene {

    protected long sourceInnovation, targetInnovation;
    protected double weight;

    public LinkGene(long sourceInnovation, long targetInnovation, double weight, long innovation) : base(innovation)
    {
        this.sourceInnovation = sourceInnovation;
        this.targetInnovation = targetInnovation;
        this.weight = weight;
    }

    public bool IsActive() // Always active
    {
        return true;
    }

    public void SetActive(bool active)
    {
        // Do not change always active
    }
    
    public bool IsReccurent()
    {
        return false;
    }

    public double GetWeight()
    {
        return weight;
    }

    public void SetWeight(double weight)
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
}
