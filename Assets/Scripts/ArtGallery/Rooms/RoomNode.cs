using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A room in the Art Gallery. Represents a single generation of the Gallery's evolution 
/// </summary>
public class RoomNode : MonoBehaviour {

    private PortalController pc;
    
    //TODO Add Room genetics (for evolving how the room is structured, not for the art)
    
    // Links to other rooms
    private SortedList<int, RoomNode> rooms;

    // Population of doors in this room
    private SortedList<int, Artwork> art;

    private int parentDoorID; // Parent of this room
    private bool isPopulated = false;  // is this room initialized?

    /// <summary>
    /// Create a new RoomNode 
    /// </summary>
    public RoomNode()
    {
        art = new SortedList<int, Artwork>();
        rooms = new SortedList<int, RoomNode>();
    }

    /// <summary>
    /// Initialize a room with random colors
    /// </summary>
    /// <param name="numberOfDoors"></param>
    public void InitializeRoom(int numberOfDoors)
    {
        pc = FindObjectOfType<PortalController>();

        /* Create doors and links to new rooms */
        for (int i = 0; i < numberOfDoors; i++)
        {
            art.Add(i, new Artwork());
            //rooms.Add(i, new RoomNode());  // in a populated room there should never be a null ref
        }
        CreatePortals();
        isPopulated = true;
        RedrawRoom();
    }

    /// <summary>
    /// Initialize a room with new instances of a given genotype
    /// </summary>
    /// <param name="numberOfDoors"></param>
    /// <param name="geno"></param>
    public void InitializeRoom(int numberOfDoors, TWEANNGenotype geno)
    {

    }

    /// <summary>
    /// Initialize a room by loading a list of genos to a set of doors
    /// </summary>
    /// <param name="numberOfDoors"></param>
    /// <param name="genos"></param>
    public void InitializeRoom(int numberOfDoors, List<TWEANNGenotype> genos)
    {

    }



    /* Public methods */
    public void SetParentDoorID(int parentDoorID)
    {
        this.parentDoorID = parentDoorID;
    }

    public Artwork GetDoorByID(int doorID)
    {
        return art[doorID];
    }

    public bool IsPopulated()
    {
        return isPopulated;
    }

    public RoomNode GetRoomByPortalID(int portalID)
    {
        return rooms[portalID];
    }

    public RoomNode GetRoom()
    {
        RoomNode room = null;


        return room;
    }

    public void RedrawRoom()
    {
        //pc.FlushPortals();

        foreach (KeyValuePair<int, Portal> p in pc.GetPortals())
        {
            if (ArtGallery.DEBUG_LEVEL > ArtGallery.DEBUG.NONE) Debug.Log("Painting portal " + p.Key + " with artwork ...");
            p.Value.PaintDoor(art[p.Key].GetArtwork());
        }
    }

    private void CreatePortals()
    {
        foreach(KeyValuePair<int, Artwork> a in art)
        {
            Portal p = pc.SpawnPortal(a.Key);
            if (ArtGallery.DEBUG_LEVEL > ArtGallery.DEBUG.NONE) Debug.Log("received portal with ID " + p.GetPortalID());

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

            // put each portal on a wall
            p.transform.position = vecs[a.Key];
            p.transform.Rotate(new Vector3(0, (-90 * a.Key), 0)); // HACK Hardcoded - fix once rooms can change the number of portals
            p.PaintDoor(a.Value.GetArtwork());

        }

    }
}
