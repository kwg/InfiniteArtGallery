using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationController : MonoBehaviour {


    public GameObject portalObject;

    List<Portal> portals; // physical portals
    RoomTree rooms; // room configuration

    /* Static numbers to get 4 portals in a square room */
    int numPortals = 4;
    int numWalls = 4; // use later if we want to change room shape
    int numPortalsPerWall = 1; // how many portals can we fit on a wall?


    // Use this for initialization
    void Start()
    {
        portals = new List<Portal>();
        rooms = new RoomTree(portals);

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

        for (int i = 0; i < numPortals; i++)
        {
            // Add portal
            GameObject portal = Instantiate(portalObject);

            // Position portals into the room correctly
            portal.transform.parent = GameObject.FindGameObjectWithTag("PrimaryRoom").transform;
            
            // put each portal on a wall
            portal.transform.position = vecs[i];
            portal.transform.Rotate(new Vector3(0, (-90 * i), 0)); // HACK Hardcoded - fix once rooms can change the number of portals


            // give each portal an ID
            Portal p = portal.gameObject.GetComponent<Portal>();
            p.SetPortalID(i);

            // give each portal a destination ID
            p.SetDestinationID(((numPortals / 2) + p.portalID) % numPortals);

            portals.Add(p);
        }

        // do genetics
        // paint portals
        DoColorChange();


        // spawn player
    }


    public void DoColorChange()
    {
        foreach(Portal p in portals)
        {
            // TODO make colors change from genetics
            p.SetColor(new Color(Random.value, Random.value, Random.value));
        }
    }

    public void DoTeleport(Player player, int portalID)
    {

        Vector3 destination = new Vector3();

        /* Find destination portal and get destination position */
        foreach (Portal p in FindObjectsOfType<Portal>())
        {
            if (portalID == p.destinationID)
            {
                RoomNode destinationRoom = rooms.GetStartingRoom();
                destination = p.gameObject.transform.position; // set destination to exit portal position
            }
        }

        /* Bump player to just outside of the portal collision box based on the location of the portal 
         * relative to the center */
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

        destination.y += 0.1f ; // Fix exit height for player 

        player.transform.position = destination;
    }


    // Update is called once per frame
    void Update ()
    {
		
	}
}
