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

    int generationID; // Is this needed?

    Room currentRoom; // configuration of the current room

    // Use this for initialization
    void Start()
    {
        ActivationFunctions.ActivateAllFunctions();
        GameObject roomProp = Instantiate(roomObject) as GameObject;
        roomProp.AddComponent<Room>();
        currentRoom = roomProp.GetComponent<Room>();
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
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
