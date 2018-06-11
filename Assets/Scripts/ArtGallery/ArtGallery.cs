using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Primary controller. Handles rooms and portals (physical) of the scene and links them to the doors in the 
/// population. Controls interaction between each side.
/// </summary>
public class ArtGallery : MonoBehaviour {

    public static bool DEBUG = true;
    public PortalController pc;

    /* 
     * Maintains generations and represents a generation as a room
     * 
     * A room has doors with genetics and parents and use a portal that has been 
     * decorated using the results of the gentics and lineage as a model to display in the scene. A Player
     * selects a door by walking into the teleport portal. If the selected portal is a parent of this room, 
     * the previous generation is loaded. While the child room that was left still exists in here, unless the
     * exact same path is selected again, the child room will never(*) be used again. At any time, the path 
     * through the rooms will always be a line with no crossings or branches
     */

    int generationID; // Is this needed?

    RoomNode currentRoom; // configuration of the current room
    RoomNode previousRoom; // Which way is backwards in time?

    /* Static numbers to get 4 portals in a square room */
    int numPortals = 4;
    int numWalls = 4; // use later if we want to change room shape
    int numPortalsPerWall = 1; // how many portals can we fit on a wall?


    // Use this for initialization
    void Start()
    {
        InitializePopulation();

        // spawn player
    }


    private void InitializePopulation() // of rooms
    {
        currentRoom = new RoomNode(pc);
        currentRoom.InitializeRoom(numPortals);
        currentRoom.RedrawRoom();
    }


    public void ChangeRoom(int portalID)
    {

        // FIXME Teleport the player HERE 


        RoomNode newRoom = currentRoom.GetRoomByPortalID(portalID);

        if(newRoom != null && newRoom.IsPopulated())
        {
            currentRoom = newRoom;
            
        }
        else
        {
            //currentRoom.InitializeRoom(numPortals);
        }

        currentRoom.RedrawRoom();
    }



    // Update is called once per frame
    void Update ()
    {
		
	}

    public PortalController GetPortalController()
    {
        return pc;
    }
}
