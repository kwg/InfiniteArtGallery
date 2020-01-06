using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour {

    private bool debug = ArtGallery.DEBUG_LEVEL < ArtGallery.DEBUG.NONE;

    public GameObject portalObject;
    SortedList<int, Portal> portals; // Portal index is door index
    int PortalCount = 0; // TODO not in use - decide what to do with it

    // Use this for initialization
    void Start () {
        portals = new SortedList<int, Portal>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void FlushPortals()
    {
        portals = new SortedList<int, Portal>();
    }

    public Portal SpawnPortal(int portalID)
    {

        GameObject portalProp = Instantiate(portalObject) as GameObject;
        portalProp.AddComponent<Portal>();
        Portal p = portalProp.GetComponent<Portal>();
        // give each portal an ID
        p.SetPortalID(portalID);

        // give each portal a destination ID
        p.SetDestinationID((2 + p.GetPortalID()) % 4);
        if(debug) Debug.Log("Portal created with ID " + p.GetPortalID() + " and DestinationId " + p.GetDestinationID());
        portals.Add(portalID, p);
        return p;
    }

    public SortedList<int, Portal> GetPortals()
    {
        return portals;
    }


    private Portal GetPortalByID(int portalID)
    {
        Portal p = null;

        foreach(KeyValuePair<int, Portal> kvpPortal in portals)
        {
            if(portalID == kvpPortal.Key)
            {
                p = kvpPortal.Value;
            }
        }

        return p;
    }
}
