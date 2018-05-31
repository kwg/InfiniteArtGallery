using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door<T> {

    SortedList<int, GenotypePortal<T>> genecticHistory; // sorted list of innovationID, genotype
    int portalID;

    public Door() 
    {
        genecticHistory = new SortedList<int, GenotypePortal<T>>();
    }

    public void SetPortalID(int portalID)
    {
        this.portalID = portalID;
    }

    public void AddGenotype(int innovationID, GenotypePortal<T> geno)
    {
        genecticHistory.Add(innovationID, geno);
    }

    public GenotypePortal<T> GetGenotypeByInnovationID(int innovationID)
    {
        return genecticHistory[innovationID];
    }


}
