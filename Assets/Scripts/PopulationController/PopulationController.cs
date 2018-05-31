using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Primary controller. Handles rooms and portals (physical) of the scene and links them to the doors in the 
/// population. Controls interaction between each side.
/// </summary>
public class PopulationController : MonoBehaviour {


    public GameObject portalObject;
    SortedList<int, Portal> portals; // physical portals

    SortedList<int, Door<GenotypePortal<Color>>> doors; // logical doors - this is the population for the NN

    RoomNode currentRoom; // configuration of the current room

    /* Static numbers to get 4 portals in a square room */
    int numPortals = 4;
    int numWalls = 4; // use later if we want to change room shape
    int numPortalsPerWall = 1; // how many portals can we fit on a wall?


    // Use this for initialization
    void Start()
    {
        portals = new SortedList<int, Portal>();
        doors = new SortedList<int, Door<GenotypePortal<Color>>>();
        currentRoom = new RoomNode();

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

            portals.Add(i, p);
        }

        // assign doors to portals
        //currentRoom.BuildDoors(portals);
        for(int i = 0; i < numPortals; i++){
        {
                doors[i] = new Door<GenotypePortal<Color>>();   
        }

        // do genetics
        // randomly paint portals
        foreach(Portal p in portals)
        {
            DoColorChange(p);
        }


        // spawn player
    }


    private void InitializePopulation()
    {
        // make a list of all portals for THIS room (in case we want to change portals per room)
           // this means EVERY portal needs a unique ID

        // Use that list to create a list of doors

        // use whatever genetics on the doors to get features (color)

        // use portal paint method to decorate the portal with the information from the door



        // making a new room:






    }


    /// <summary>
    /// changes colors of portals to a random color
    /// </summary>
    public void DoColorChange(Portal p)
    {
            p.PaintDoor(new Color(Random.value, Random.value, Random.value));
    }

    /// <summary>
    /// Controls teleporting of the player to the destination portal based on the selected portal
    /// </summary>
    /// <param name="player">Player to be teleported</param>
    /// <param name="portalID">ID of the portal selected by the player</param>
    public void DoTeleport(Player player, int portalID)
    {

        Vector3 destination = new Vector3();

        /* Find destination portal and get destination position */
        foreach (Portal p in FindObjectsOfType<Portal>())
        {
            if (portalID == p.destinationID)
            {
                RoomNode destinationRoom = currentRoom;
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

        destination.y -= 1.6f ; // Fix exit height for player (match center of portal to center of player)

        player.transform.position = destination;

        // FIXME load room
        RoomNode parentRoom = currentRoom;
        currentRoom = currentRoom.GetRoomByPortalID(portalID);
        if (!currentRoom.IsPopulated())
        {
            currentRoom.BuildDoors(portals); // TODO does this break if we add more physical rooms and portals?
            currentRoom.SetParentRoom(parentRoom, portalID);
            //DoColorChange();
        }
        else // room exists, load it
        {
            portals = currentRoom.LoadRoom();
        }

    }


    // Update is called once per frame
    void Update ()
    {
		
	}
}
