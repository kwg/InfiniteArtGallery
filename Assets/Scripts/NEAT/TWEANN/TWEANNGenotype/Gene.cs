using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gene {

    public long innovation;

    public Gene(long innovation)
    {
        this.innovation = innovation;
    }

    public Gene CopyGene()
    {
        return (Gene) MemberwiseClone();
    }




}
