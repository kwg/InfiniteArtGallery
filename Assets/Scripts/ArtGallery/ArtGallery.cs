using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;


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
        //gameRoom.SetArtGallery(this);

        //activate functions
        // Testing: activating all functions
        //ActivationFunctions.ActivateAllFunctions();
        ActivationFunctions.ActivateFunction(activeFunctions);

        // build the lobby
        room = new RoomConfiguration(STARTING_NUM_ARTWORKS);
        gameRoom.InitializeRoom(room);

        List<Artwork> artToSave = new List<Artwork>();


        //room.SetSculptures(gameRoom.GetSculptures());

    }

    public List<FTYPE> GetAvailableActivationFunctions()
    {
        return availableFunctions;
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

    public GeneticArt GetArtwork(int portalID)
    {
        return room.GetRoomArt()[portalID];
    }

    public void ChangeRoom(int portalID, int destinationID)
    {
        // is the desitnation a new room or a return?
        //if (room.GetRoomByPortalID(portalID) == null)
        //{
        //    UnityEngine.Debug.Log("Making new room...");
        //    room.AddRoom(portalID, new RoomConfiguration(room, destinationID, portalID, room.GetArtworks(), room.sculptures));
        //}
        //else
        //{
            //room.MutateSculptures();
        //}
        room = new RoomConfiguration((portalID), room.GetRoomArt());

        //gameRoom.ClearReturnPortalDecorations();
        //if (room.GetParentID() > -1) gameRoom.SetReturnPortalDecoration(room.GetParentID());


        //SaveRoom();
    }

    //public void RemoveRoom(int portalID)
    //{
    //    if(room.GetRoomByPortalID(portalID) != null)
    //    {
    //        room.RemoveRoom(portalID);
    //    }
    //}

    public static long NextRoomID()
    {
        return roomID++;
    }

    // Update is called once per frame
    void Update ()
    {

    }

}
