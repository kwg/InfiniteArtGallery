using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door<T> {

    GenotypePortal<T> geno;
    Portal p;
    int doorID;
    PortalController pc;

    public Door(int doorID, PortalController pc) 
    {
        geno = new GenotypePortal<T>();
        this.doorID = doorID;
        this.pc = pc;
        CreatePortal();
        
    }

    private void CreatePortal()
    {
        p = pc.SpawnPortal();

        p.PaintDoor(new Color(Random.value, Random.value, Random.value));

        // TODO make a method to do this correctly
        float x_spacing = 9.9f;
        float z_spacing = 9.9f;
        float y_spacing = 2.5f;

        Vector3[] vecs = {
            new Vector3((0 + x_spacing), (0 + y_spacing), 0),
            new Vector3(0, (0 + y_spacing), (0 + z_spacing)),
            new Vector3((0 - x_spacing), (0 + y_spacing), 0),
            new Vector3(0, (0 + y_spacing), (0 - z_spacing)),
        };

        // put each portal on a wall
        p.transform.position = vecs[doorID];
        p.transform.Rotate(new Vector3(0, (-90 * doorID), 0)); // HACK Hardcoded - fix once rooms can change the number of portals


        // give each portal an ID
        p.SetPortalID(doorID);

        // give each portal a destination ID
        p.SetDestinationID((2 + p.GetPortalID()) % 4);

    }

    public void SetGenotypePortal(GenotypePortal<T> geno)
    {
        this.geno = geno;
    }

    public GenotypePortal<T> GetGenotype()
    {
        return geno;
    }

    
}
