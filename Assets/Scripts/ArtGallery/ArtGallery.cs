using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Primary controller. Handles rooms and portals (physical) of the scene and links them to the doors in the 
/// population. Controls interaction between each side.
/// </summary>
public class ArtGallery : MonoBehaviour {

    public static ArtGallery artgallery = null;

    public enum DEBUG { NONE = 0, POLITE = 1, VERBOSE = 2 };
    public static DEBUG DEBUG_LEVEL = DEBUG.NONE;
    public const int STARTING_NUM_ARTWORKS = 4;
    public GameObject roomObject; // RoomObject for room to load (set in the editor)
    [HideInInspector] public Room gameRoom; // Reference to the in-game room that the player is currently in (set by the script)
    public GameObject FPC; // Reference to the first person contoller ( set in the editor)
    public Player player;
    string[] args = System.Environment.GetCommandLineArgs();

    //private SortedList<int, RoomConfiguration> history;
    private RoomConfiguration lobby; // Root of the room tree
    private RoomConfiguration room; // current room
    public static long roomID = 0;

    //Game state information
    int seed;
    List<List<Artwork>> generatedImages;
    int generatedImagesCounter = 0;
    int generatedImagesSelectedID = -1;
    List<List<GameObject[,,]>> generatedScultures;
    //FIXME PROTOTYPE SAVING AND COMMANDLINE DATA
    
    // active functions
    List<FTYPE> activeFunctions;
    // collectedFunctions
    List<FTYPE> availableFunctions;

    public static ArtGallery GetArtGallery()
    {
        if(artgallery != null)
        {
            return artgallery;
        }
        else
        {
            throw new System.Exception("ag is null!");
        }
    }


    private void Awake()
    {
        player = FindObjectOfType<Player>();

    }

    // Use this for initialization
    void Start()
    {
        artgallery = this;
        args[0] = "0001"; //HACK PROTOTYPE hardcoded tester ID
        //FIXME PROTOTYPE set a random seed value here instead and save that value for a play session
        seed = 1234567;
        Random.InitState(seed);
        EvolutionaryHistory.InitializeEvolutionaryHistory();

        // Build the game room
        GameObject roomProp = Instantiate(roomObject) as GameObject;
        gameRoom = roomProp.GetComponent<Room>();
        gameRoom.SetArtGallery(this);

        // starting functions
        availableFunctions = new List<FTYPE> { FTYPE.ID, FTYPE.TANH, FTYPE.SQUAREWAVE, FTYPE.GAUSS, FTYPE.SINE };
        if (activeFunctions == null)
        {
            activeFunctions = new List<FTYPE>();
        }


        //activate functions
        // Testing: activating all functions
        //ActivationFunctions.ActivateAllFunctions();
        ActivationFunctions.ActivateFunction(activeFunctions);

        // build the lobby
        lobby = new RoomConfiguration(STARTING_NUM_ARTWORKS);
        room = lobby;
        gameRoom.InitializeRoom(GetImagesFromArtworks(room.GetArtworks()));

        List<Artwork> artToSave = new List<Artwork>();


        lobby.SetSculptures(gameRoom.GetSculptures());

    }

    public void SaveImage(Artwork artwork)
    {
        if(generatedImages == null)
        {
            generatedImages = new List<List<Artwork>>();
        }
        if(generatedImages.Count <= generatedImagesCounter)
        {
            generatedImages.Add(new List<Artwork>());
        }
        else if(generatedImages[generatedImagesCounter].Count >= 4)
        {
            generatedImages.Add(new List<Artwork>());
            generatedImagesCounter++;
        }
        generatedImages[generatedImagesCounter].Add(artwork);
        SavePNG(artwork.GetImage(), generatedImagesCounter, (generatedImages[generatedImagesCounter].Count), generatedImagesSelectedID, args[0]);
    }

    public void SavePNG(Texture2D tex, int seqID, int artworkID, int selectedArt, string testerID)
    {
        // Encode texture into PNG
        byte[] bytes = tex.EncodeToPNG();
        //Destroy(tex);

        // For testing purposes, also write to a file in the project folder
        if(!Directory.Exists(Application.dataPath + "/../" + testerID + "/" + generatedImagesCounter))
        {
            Directory.CreateDirectory(Application.dataPath + "/../" + testerID + "/" + generatedImagesCounter);
        }
        File.WriteAllBytes(Application.dataPath + "/../" + testerID + "/" + generatedImagesCounter + "/artwork_" + artworkID + "_" + ((artworkID == selectedArt) ? "selected" : "not_selected") + ".png", bytes);
        //File.WriteAllBytes(Application.dataPath + "/../" + testerID + "/artwork_" + artworkID + "_" + ((artworkID == selectedArt) ? "selected" : "not_selected") + ".png", bytes);

    }

    public RoomConfiguration GetLobby()
    {
        return lobby;
    }

    public FTYPE GetRandomCollectedFunction()
    {
        return availableFunctions[Random.Range(0, availableFunctions.Count)];
    }

    public void ActivateFunction(FTYPE fTYPE)
    {
        if(activeFunctions == null)
        {
            activeFunctions = new List<FTYPE>();
        }
        if (!activeFunctions.Contains(fTYPE))
        {
            activeFunctions.Add(fTYPE);
        }
    }

    public void DeactivateFunction(FTYPE fTYPE)
    {
        if (activeFunctions.Contains(fTYPE))
        {
            activeFunctions.Remove(fTYPE);
        }
    }

    //FIXME PROTOTYPE need to be able to deactivate functions... wait is this even in use anymore?

    public Artwork GetArtwork(int portalID)
    {
        return room.GetArtworks()[portalID];
    }

    public void ChangeRoom(int portalID, int destinationID)
    {
        generatedImagesSelectedID = portalID;
        // is the desitnation a new room or a return?
        if (room.GetRoomByPortalID(portalID) == null)
        {
            room.AddRoom(portalID, new RoomConfiguration(room, destinationID, portalID, room.GetArtworks()[portalID], room.sculptures));
        }
        room = room.GetRoomByPortalID(portalID);

        gameRoom.ConfigureRoom(room.GetParentID(), GetImagesFromArtworks(room.GetArtworks()), room.sculptures);
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

    public void SelectSculpture(Sculpture sculpture)
    {
        foreach(Sculpture s in room.sculptures)
        {
            if (s.Equals(sculpture))
            {
                s.SetSelected(!s.GetSelected());
            }
            else if (s.GetSelected())
            {
                s.SetSelected(false);
            }
        }
    }
}
