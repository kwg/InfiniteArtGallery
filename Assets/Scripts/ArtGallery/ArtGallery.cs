using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Primary controller. Handles rooms and portals (physical) of the scene and links them to the doors in the 
/// population. Controls interaction between each side.
/// </summary>
public class ArtGallery : MonoBehaviour {

    public enum DEBUG { NONE = 0, POLITE = 1, VERBOSE = 2 };
    public static DEBUG DEBUG_LEVEL = DEBUG.POLITE;


    public int STARTING_NUM_ARTWORKS;

    public GameObject roomObject;
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

    // Use this for initialization
    void Start()
    {
        ActivationFunctions.ActivateAllFunctions();
        GameObject roomProp = Instantiate(roomObject) as GameObject;
        roomProp.AddComponent<RoomNode>();
        currentRoom = roomProp.GetComponent<RoomNode>();
        InitializePopulation();


        // spawn player
    }


    private void InitializePopulation() // of rooms
    {
        Debug.Log("InitializePopulation is being called");
        currentRoom.InitializeRoom(STARTING_NUM_ARTWORKS);
        currentRoom.RedrawRoom();
    }


    public void ChangeRoom(int portalID)
    {

        // FIXME Teleport the player HERE?


        currentRoom.ChangeRoomByPortalID(portalID);

        //currentRoom.RedrawRoom();
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
