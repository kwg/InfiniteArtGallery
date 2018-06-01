using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour {
    
    public GameObject portalObject;
    SortedList<int, Portal> portals;
    int PortalCount = 0;
	// Use this for initialization
	void Start () {
        portals = new SortedList<int, Portal>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Portal SpawnPortal(int portalID)
    {

        GameObject portalProp = Instantiate(portalObject) as GameObject;
        Portal p = portalProp.AddComponent<Portal>();
        // give each portal an ID
        p.SetPortalID(portalID);

        // give each portal a destination ID
        p.SetDestinationID((2 + p.GetPortalID()) % 4);
        Debug.Log("Portal created with ID " + p.GetPortalID() + " and DestinationId " + p.GetDestinationID());
        portals.Add(portalID, p);
        return p;
    }

    public SortedList<int, Portal> GetPortals()
    {
        return portals;
    }

    public static PortalController GetPortalController() {
        return FindObjectOfType<PortalController>();
    }

    public void DoTeleport(Player player, int portalID)//TODO fix this
    {
        Debug.Log("starting teleport form portal " + portalID + " = " + portals[portalID].GetPortalID());
        Vector3 destination = new Vector3(20, 20, 20);
        for(int i = 0; i < portals.Count; i++)
        {
            if(portals[i].GetPortalID() == portals[portalID].GetDestinationID())
            {
            Debug.Log("Found portal with ID " + portals[i].GetPortalID());
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
    }
}
