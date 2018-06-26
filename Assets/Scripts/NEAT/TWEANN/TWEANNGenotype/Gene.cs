using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gene {

    public long Innovation { get; set; }

    public Gene(long innovation)
    {
        Innovation = innovation;
    }

    public Gene CopyGene()
    {
        return (Gene) MemberwiseClone();
    }
}
