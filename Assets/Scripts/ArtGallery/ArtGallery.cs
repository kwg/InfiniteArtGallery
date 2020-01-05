using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using System.Diagnostics;
using System;

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

    // this is a change


    //command line args and default values
    string[] args = System.Environment.GetCommandLineArgs();
    public int testerID { get; private set; }
    public bool invertY { get; private set; }
    public float functionSpawnRate { get; private set; }
    public int artworkMutationChances { get; private set; }
    public int sculptureMutationChances { get; private set; }
    //public float gameTimer { get; private set; }
    const int DEFAULT_TESTERID = 9897;
    const bool DEFAULT_INVERTY = false;
    const float DEFAULT_FUNCTION_SPAWN_RATE = 1f;
    const int DEFAULT_ARTWORK_MUTATION_CHANCES = 15;
    const int DEFAULT_SCULPTURE_MUTATION_CHANCES = 15;
    const float MAX_GAME_TIME = 1200f;


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

    /// <summary>
    /// Executes external batch file or bash script
    /// </summary>
    /// <param name="script">script to execute</param>
    /// <param name="args">arguments to pass to the script</param>
    public void RunExternalScript(string script, string args)
    {

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = script,
            Arguments = args
        };
        Process.Start(startInfo);
    }

    /// <summary>
    /// This executes before any Start() methods 
    /// </summary>
    private void Awake()
    {
        //Init
        EvolutionaryHistory.InitializeEvolutionaryHistory();
        testerID = DEFAULT_TESTERID;
        invertY = DEFAULT_INVERTY;
        functionSpawnRate = DEFAULT_FUNCTION_SPAWN_RATE;
        artworkMutationChances = DEFAULT_ARTWORK_MUTATION_CHANCES;
        sculptureMutationChances = DEFAULT_SCULPTURE_MUTATION_CHANCES;
        //gameTimer = MAX_GAME_TIME;


        //set reference to the player
        player = FindObjectOfType<Player>();

        //parse command line args and set variables
        if (args[0] != null)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-testerID")
                {
                    int result;
                    if (int.TryParse(args[i + 1], out result))
                    {
                        testerID = result;
                    }
                }
                else if (args[i] == "-invertY")
                {
                    invertY = true;
                }
                else if (args[i] == "-functionSpawnRate")
                {
                    float result;
                    if(float.TryParse(args[i + 1], out result))
                    {
                        functionSpawnRate = result;
                    }
                }
                else if (args[i] == "-artworkMutationChances")
                {
                    int result;
                    if (int.TryParse(args[i + 1], out result))
                    {
                        artworkMutationChances = result;
                    }
                    
                }
                else if (args[i] == "-sculptureMutationChances")
                {
                    int result;
                    if (int.TryParse(args[i + 1], out result))
                    {
                        sculptureMutationChances = result;
                    }

                }
                else if (args[i] == "-gameTimer")
                {
                    float result;
                    if (float.TryParse(args[i + 1], out result) && result <= MAX_GAME_TIME)
                    {
                        //gameTimer = result;
                    }

                }
            }
        }

        // starting functions
        availableFunctions = new List<FTYPE> { FTYPE.ID, FTYPE.TANH, FTYPE.SQUAREWAVE, FTYPE.GAUSS, FTYPE.SINE, FTYPE.SAWTOOTH, FTYPE.ABSVAL };
        if (activeFunctions == null)
        {
            activeFunctions = new List<FTYPE>();
            ActivateFunction(FTYPE.TANH);
        }
    }

    // Use this for initialization
    void Start()
    {
        artgallery = this;

        
        //FIXME PROTOTYPE set a random seed value here instead and save that value for a play session
        seed = testerID; //testerID
        //seed = UnityEngine.Random.Range(0, 9999999);
        UnityEngine.Random.InitState(seed);


        //RunExternalScript("test.bat", "");

        // Build the game room
        GameObject roomProp = Instantiate(roomObject) as GameObject;
        gameRoom = roomProp.GetComponent<Room>();
        gameRoom.SetArtGallery(this);

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

    public List<FTYPE> GetAvailableActivationFunctions()
    {
        return availableFunctions;
    }

    private int ConvertToInt(String intString)
    {
        int i = 0;
        if (!Int32.TryParse(intString, out i))
        {
            i = -1;
        }
        return i;
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
        if(!Directory.Exists(Application.dataPath + "/../Subject-" + testerID + "/" + generatedImagesCounter))
        {
            Directory.CreateDirectory(Application.dataPath + "/../Subject-" + testerID + "/" + generatedImagesCounter);
        }
        File.WriteAllBytes(Application.dataPath + "/../Subject-" + testerID + "/" + generatedImagesCounter + "/artwork_" + artworkID + "_" + ((artworkID == selectedArt) ? "selected" : "not_selected") + ".png", bytes);
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

    public void ResetSculpture(Sculpture s)
    {
        s.NewSculpture();
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
        if (!Directory.Exists(Application.dataPath + "/../Subject-" + testerID + "/" + generatedSculpturesCounter))
        {
            Directory.CreateDirectory(Application.dataPath + "/../Subject-" + testerID + "/" + generatedSculpturesCounter);
        }
        File.WriteAllBytes(Application.dataPath + "/../Subject-" + testerID + "/" + generatedSculpturesCounter + "/sculpture_" + sculptureID + "_" + ".csv", bytes);

    }

    public void SaveSeed(int seed)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(seed.ToString());
        if (!Directory.Exists(Application.dataPath + "/../Subject-" + testerID))
        {
            Directory.CreateDirectory(Application.dataPath + "/../Subject-" + testerID);
        }
        File.WriteAllBytes(Application.dataPath + "/../Subject-" + testerID + "/seed.txt", bytes);

    }

    public RoomConfiguration GetLobby()
    {
        return lobby;
    }

    public FTYPE GetRandomCollectedFunction()
    {
        return availableFunctions[UnityEngine.Random.Range(0, availableFunctions.Count)];
    }

    public bool ActivateFunctionsEmpty()
    {
        return activeFunctions.Count == 0;
    }

    public bool FunctionIsActive(FTYPE fTYPE)
    {
        return activeFunctions.Contains(fTYPE);
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

        ActivationFunctions.ActivateFunction(activeFunctions);
    }

    public void DeactivateFunction(FTYPE fTYPE)
    {
        if (activeFunctions.Contains(fTYPE))
        {
            activeFunctions.Remove(fTYPE);
        }
    }

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
            UnityEngine.Debug.Log("Making new room...");
            room.AddRoom(portalID, new RoomConfiguration(room, destinationID, portalID, room.GetArtworks(), room.sculptures));
        }
        else
        {
            room.MutateSculptures();
        }
        room = room.GetRoomByPortalID(portalID);

        gameRoom.ConfigureRoom(room.GetParentID(), GetImagesFromArtworks(room.GetArtworks()), room.sculptures);
        gameRoom.RedrawRoom();

        //gameRoom.ClearReturnPortalDecorations();
        //if (room.GetParentID() > -1) gameRoom.SetReturnPortalDecoration(room.GetParentID());


        //SaveRoom();
    }

    public void RemoveRoom(int portalID)
    {
        if(room.GetRoomByPortalID(portalID) != null)
        {
            room.RemoveRoom(portalID);
        }
    }

    public static long NextRoomID()
    {
        return roomID++;
    }

    // Update is called once per frame
    void Update ()
    {
        //gameTimer -= Time.deltaTime;
        //if(gameTimer <= 0)
        {
            //GameOver();
        }

        Artwork[] art = room.GetArtworks(); // FIXME This is not a very functional way of dealing with the threads. However removing threads is not an option.
        for (int a = 0; a < art.Length; a++)
        {
            if (art[a].NeedsRedraw())
            {
                art[a].ApplyImageProcess();
                gameRoom.Locked = false;
            }
        }
    }

    void GameOver()
    {
        UnityEngine.Debug.Log("Quitting game due to timeout...");
        Application.Quit();
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
