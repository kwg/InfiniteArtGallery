using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkGene : Gene {

    protected long sourceInnovation, targetInnovation;
    protected double weight;
    protected bool active;

    public LinkGene(long sourceInnovation, long targetInnovation, double weight, long innovation) : base(innovation)
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

    public string ToString()
    {
        return "Link with ID: " + innovation + " connects node " + sourceInnovation + " to node " + targetInnovation + " and has weight of " + weight;
    }
}
