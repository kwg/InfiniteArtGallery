using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullLinkGene : LinkGene {

    protected bool active, recurrent;

    public FullLinkGene(long sourceInnovation, long targetInnovation, float weight, long innovation, bool active, bool recurrent) : base(sourceInnovation, targetInnovation, weight, innovation)
    {
        this.active = active;
        this.recurrent = recurrent;
    }

    public new bool IsActive()
    {
        return active;
    }

    public new void SetActive(bool active)
    {
        this.active = active;
    }

    public new FullLinkGene Clone()
    {
        return new FullLinkGene(sourceInnovation, targetInnovation, weight, Innovation, active, recurrent);
    }
}
