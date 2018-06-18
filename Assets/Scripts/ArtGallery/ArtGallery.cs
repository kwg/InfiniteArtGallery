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
    public GameObject roomObject; // RoomObject for room to load
    public Room gameRoom; // Reference to the in-game room that the player is currently in


    //private SortedList<int, RoomConfiguration> history;
    private RoomConfiguration lobby; // Root of the room tree
    private RoomConfiguration room; // current room
    public static long roomID = 0;


    // Use this for initialization
    void Start()
    {
        ActivationFunctions.ActivateAllFunctions();
        GameObject roomProp = Instantiate(roomObject) as GameObject;
        gameRoom = roomProp.GetComponent<Room>();

        lobby = new RoomConfiguration(STARTING_NUM_ARTWORKS);
        room = lobby;

        gameRoom.InitializeRoom(GetImagesFromArtworks(room.GetArtworks()));


        // spawn player
    }

    public void ChangeRoom(int portalID, int destinationID)
    {
        // is the desitnation a new room or a return?
        if (room.GetRoomByPortalID(portalID) == null) room.AddRoom(portalID, new RoomConfiguration(room, destinationID, room.GetArtworks()[portalID]));
        room = room.GetRoomByPortalID(portalID);


        gameRoom.InitializeRoom(room.GetParentID(), GetImagesFromArtworks(room.GetArtworks()));
        gameRoom.RedrawRoom();

        gameRoom.ClearReturnPortalDecorations();
        if (room.GetParentID() > -1) gameRoom.SetReturnPortalDecoration(room.GetParentID());





        //Debug.Log("Changing room through portal " + portalID + " with destinationID " + destinationID);
        //gameRoom.ClearReturnPortalDecorations();

        //bool rewinding = false;
        //RoomConfiguration newRoom = room.GetRoomByArtwork(room.GetArtworks()[portalID]);

        //if (gameRoom.GetParentID() == room.GetParentID()) // rewind
        //{
        //    Debug.Log("Opps - Time travel is not implemented");
        //    rewinding = true;
        //}

        //room = newRoom;
        //if (room.GetParentID() > -1) gameRoom.SetReturnPortalDecoration(room.GetParentID());

        //gameRoom.InitializeRoom(room.GetParentID(), GetImagesFromArtworks(room.GetArtworks()));
        //gameRoom.RedrawRoom();

        // FIXME Teleport the player HERE?
    }

    public static long NextRoomID()
    {
        return roomID++;
    }

    // Update is called once per frame
    void Update ()
    {
        Artwork[] art = room.GetArtworks();
        for(int a = 0; a < art.Length; a++)
        {

            if (art[a].HasFinishedProcessing())
            {
                art[a].ApplyImageProcess();
            }
        }
    }

    private Texture2D[] GetImagesFromArtworks(Artwork[] artworks)
    {
        Texture2D[] images = new Texture2D[artworks.Length];
        for(int a = 0; a < artworks.Length; a++)
        {    
            images[a] = artworks[a].GetArtwork();
        }
        return images;
    }
}
