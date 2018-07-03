using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Primary controller. Handles rooms and portals (physical) of the scene and links them to the doors in the 
/// population. Controls interaction between each side.
/// </summary>
public class ArtGallery : MonoBehaviour {

    public enum DEBUG { NONE = 0, POLITE = 1, VERBOSE = 2 };
    public static DEBUG DEBUG_LEVEL = DEBUG.NONE;
    public const int STARTING_NUM_ARTWORKS = 4;
    public GameObject roomObject; // RoomObject for room to load (set in the editor)
    [HideInInspector] public Room gameRoom; // Reference to the in-game room that the player is currently in (set by the script)
    public GameObject FPC; // Reference to the first person contoller ( set in the editor)

    //private SortedList<int, RoomConfiguration> history;
    private RoomConfiguration lobby; // Root of the room tree
    private RoomConfiguration room; // current room
    public static long roomID = 0;

    //Game state information
    int seed;
    
    // active functions
    FTYPE[] activeFunctions;
    // collectedFunctions
    FTYPE[] collectedFunctions;

    // Use this for initialization
    void Start()
    {
        seed = 1234567;
        Random.InitState(seed);
        EvolutionaryHistory.InitializeEvolutionaryHistory();

        // Build the game room
        GameObject roomProp = Instantiate(roomObject) as GameObject;
        gameRoom = roomProp.GetComponent<Room>();

        // starting functions
        collectedFunctions = new FTYPE[] {FTYPE.ID, FTYPE.TANH, FTYPE.SQUAREWAVE, FTYPE.GAUSS, FTYPE.SINE };
        activeFunctions = new FTYPE[] { FTYPE.ID, FTYPE.GAUSS, FTYPE.SINE };

        //activate functions
        // Testing: activating all functions
        //ActivationFunctions.ActivateAllFunctions();
        ActivationFunctions.ActivateFunction(activeFunctions);

        // build the lobby
        lobby = new RoomConfiguration(STARTING_NUM_ARTWORKS);
        room = lobby;
        gameRoom.InitializeRoom(GetImagesFromArtworks(room.GetArtworks()));

    }

    public RoomConfiguration GetLobby()
    {
        return lobby;
    }

    public Artwork GetArtwork(int portalID)
    {
        return room.GetArtworks()[portalID];
    }

    public void ChangeRoom(int portalID, int destinationID)
    {
        // is the desitnation a new room or a return?
        if (room.GetRoomByPortalID(portalID) == null)
        {
            room.AddRoom(portalID, new RoomConfiguration(room, destinationID, portalID, room.GetArtworks()[portalID]));
        }
        room = room.GetRoomByPortalID(portalID);

        gameRoom.ConfigureRoom(room.GetParentID(), GetImagesFromArtworks(room.GetArtworks()));
        gameRoom.RedrawRoom();

        gameRoom.ClearReturnPortalDecorations();
        if (room.GetParentID() > -1) gameRoom.SetReturnPortalDecoration(room.GetParentID());

    }

    public static long NextRoomID()
    {
        return roomID++;
    }

    // Update is called once per frame
    void Update ()
    {
        Artwork[] art = room.GetArtworks(); // FIXME This is not a very functional way of dealing with the threads. However removing threads is not an option.
        for (int a = 0; a < art.Length; a++)
        {
            if (art[a].NeedsRedraw())
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
