using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

/// <summary>
/// Primary controller. Handles rooms and portals (physical) of the scene and links them to the doors in the 
/// population. Controls interaction between each side.
/// </summary>
public class ArtGallery : MonoBehaviour {

    public static ArtGallery artgallery = null;

    public enum DEBUG { NONE = 0, POLITE = 1, VERBOSE = 2 };
    public static DEBUG DEBUG_LEVEL = DEBUG.POLITE;
    public const int STARTING_NUM_ARTWORKS = 4;
    public GameObject roomObject; // RoomObject for room to load (set in the editor)
    [HideInInspector] public Room gameRoom; // Reference to the in-game room that the player is currently in (set by the script)
    public GameObject FPC; // Reference to the first person contoller ( set in the editor)
    public Player player;
    string[] args = System.Environment.GetCommandLineArgs();
    string testerID = "0001";

    //private SortedList<int, RoomConfiguration> history;
    private RoomConfiguration lobby; // Root of the room tree
    private RoomConfiguration room; // current room
    public static long roomID = 0;

    //Game state information
    int seed;
    List<List<Artwork>> generatedImages;
    int generatedImagesCounter = 0;
    int generatedImagesSelectedID = -1;
    List<List<Color[,,]>> generatedSculptures;
    int generatedSculpturesCounter = 0;
    int generatedSculpturesSelectedID = -1;
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
        if (args[0] != null)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-testerID")
                {
                    testerID = args[i + 1];
                }
            }
        }
        
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

    private void SaveRoom()
    {

    }

    public void SaveImage(Artwork artwork)
    {
        if (generatedImages == null)
        {
            generatedImages = new List<List<Artwork>>();
        }
        if (generatedImages.Count <= generatedImagesCounter)
        {
            generatedImages.Add(new List<Artwork>());
        }
        else if (generatedImages[generatedImagesCounter].Count >= 4)
        {
            generatedImages.Add(new List<Artwork>());
            //generatedImagesCounter++;
        }
        generatedImages[generatedImagesCounter].Add(artwork);
        SavePNG(artwork.GetImage(), generatedImagesCounter, (generatedImages[generatedImagesCounter].Count), generatedImagesSelectedID);
    }

    public void SavePNG(Texture2D tex, int seqID, int artworkID, int selectedArt)
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
    }

    public void SaveVox(Color[,,] voxArray)
    {
        if (generatedSculptures == null)
        {
            generatedSculptures = new List<List<Color[,,]>>();
        }
        if (generatedSculptures.Count <= generatedSculpturesCounter)
        {
            generatedSculptures.Add(new List<Color[,,]>());
        }
        else if (generatedSculptures[generatedSculpturesCounter].Count >= 4)
        {
            generatedSculptures.Add(new List<Color[,,]>());
            //generatedSculpturesCounter++;
        }
        generatedSculptures[generatedSculpturesCounter].Add(voxArray);
        SaveCSV(voxArray, generatedSculpturesCounter, (generatedSculptures[generatedSculpturesCounter].Count));
    }

    public void SaveCSV(Color[,,] voxArray, int seqID, int sculptureID)
    {
        string voxCSV = "";
        
        for(int x = 0; x < voxArray.GetLength(0); x++)
        {
            for(int z = 0; z < voxArray.GetLength(1); z++)
            {
                for(int y = 0; y < voxArray.GetLength(2); y++)
                {
                    voxCSV += x + ", " + z + ", " + y + ", ";
                    voxCSV += voxArray[x, z, y].r.ToString() + ", ";
                    voxCSV += voxArray[x, z, y].g.ToString() + ", ";
                    voxCSV += voxArray[x, z, y].b.ToString() + ", ";
                    voxCSV += voxArray[x, z, y].a.ToString() + "\n";
                }
            }
        }

        byte[] bytes = Encoding.UTF8.GetBytes(voxCSV); 
        if (!Directory.Exists(Application.dataPath + "/../" + testerID + "/" + generatedSculpturesCounter))
        {
            Directory.CreateDirectory(Application.dataPath + "/../" + testerID + "/" + generatedSculpturesCounter);
        }
        File.WriteAllBytes(Application.dataPath + "/../" + testerID + "/" + generatedSculpturesCounter + "/sculpture_" + sculptureID + "_" + ".csv", bytes);

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
        // HACK PROTOTYPE manual manipulation of saving vars 
        generatedImagesSelectedID = portalID;
        generatedImagesCounter++;
        generatedSculpturesCounter++;

        // is the desitnation a new room or a return?
        if (room.GetRoomByPortalID(portalID) == null)
        {
            Debug.Log("Making new room...");
            room.AddRoom(portalID, new RoomConfiguration(room, destinationID, portalID, room.GetArtworks(), room.sculptures));
        }
        room = room.GetRoomByPortalID(portalID);

        gameRoom.ConfigureRoom(room.GetParentID(), GetImagesFromArtworks(room.GetArtworks()), room.sculptures);
        gameRoom.RedrawRoom();

        gameRoom.ClearReturnPortalDecorations();
        if (room.GetParentID() > -1) gameRoom.SetReturnPortalDecoration(room.GetParentID());


        SaveRoom();
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
