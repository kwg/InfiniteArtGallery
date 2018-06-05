using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door<T> {

    GenotypePortal<T> geno;
    Portal p;
    int doorID;
    PortalController pc;

    /// <summary>
    /// Create a new door in a room with a Portal geno. 
    /// </summary>
    /// <param name="doorID">Index of this door</param>
    /// <param name="pc">Reference to the portal controller that can spawn and decorate portals in the scene</param>
    public Door(int doorID, PortalController pc) 
    {
        geno = new GenotypePortal<T>();
        this.doorID = doorID;
        this.pc = pc;
        CreatePortal();
        
    }

    private void CreatePortal()
    {
        p = pc.SpawnPortal(doorID);
        if (ArtGallery.DEBUG) Debug.Log("received portal with ID " + p.GetPortalID());

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
        RedecorateDoor();

    }

    public void RedecorateDoor()
    {
        // FIXME Get this color from the geno
        pc.DecoratePortal(doorID, new Color(Random.value, Random.value, Random.value));

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
