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

    public void DoTeleport(Player player, int portalID)
    {
        if (debug) Debug.Log("starting teleport form portal " + portalID + " = " + portals[portalID].GetPortalID());
        Vector3 destination = new Vector3(0, 20, 0);
        for(int i = 0; i < portals.Count; i++)
        {
            if(portals[i].GetPortalID() == portals[portalID].GetDestinationID())
            {
                if (ArtGallery.DEBUG_LEVEL > ArtGallery.DEBUG.NONE) Debug.Log("Found portal with ID " + portals[i].GetPortalID());
                destination = portals[i].gameObject.transform.position; // set destination to exit portal position
                
            }
        }
        // Bump player to just outside of the portal collision box based on the location of the portal relative to the center
        if (destination.x < 0)
        {
            destination.x += 0.25f;
        }
        else
        {
            destination.x -= 0.25f;
        }

        if (destination.z < 0)
        {
            destination.z += 0.25f;
        }
        else
        {
            destination.z -= 0.25f;
        }

        destination.y -= 1.6f; // Fix exit height for player (player is 1.8 tall, portal is 5, center of portal is 2.5, center of player is 0.9. 2.5 - 0.9 = 1.6)

        player.transform.position = destination;

        /* FIXME Now tell the population controller that the player has moved 
         * by sending the portal (equiv to door) index to the population controller
         * 
         */

        FindObjectOfType<ArtGallery>().ChangeRoom(portalID);
    }

    public void DecoratePortal(int portalID, Color color)
    {
        GetPortalByID(portalID).PaintDoor(color);
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
