using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO This should be modified to dynamically build the room based on parameters set by ArtGallery. What about a lobby?
/// <summary>
/// The only room in the Art Gallery.
/// </summary>
public class Room : MonoBehaviour {

    private PortalController pc;
    public static int currentRoomID = 0;

    // Links to other rooms
    private SortedList<int, SortedList<int, Artwork>> rooms;

    // Population of doors in this room
    //private SortedList<int, Artwork> art;

    private int parentDoorID; // Parent of this room
    private bool isPopulated = false;  // is this room initialized?

    /// <summary>
    /// Initialize a room with random art
    /// </summary>
    /// <param name="numberArtworks"></param>
    public void InitializeRoom(int numberArtworks)
    {
        pc = FindObjectOfType<PortalController>();
        rooms = new SortedList<int, SortedList<int, Artwork>>();
        rooms[currentRoomID] = new SortedList<int, Artwork>();
        parentDoorID = -1;
        /* Create art */
        for (int i = 0; i < numberArtworks; i++)
        {
            rooms[currentRoomID].Add(i, new Artwork());
        }
        CreatePortals();
        isPopulated = true;
    }

    /* Public methods */
    public void SetParentDoorID(int parentDoorID)
    {
        this.parentDoorID = parentDoorID;
    }

    public Artwork GetDoorByID(int doorID)
    {
        return rooms[currentRoomID][doorID];
    }

    public bool IsPopulated()
    {
        return isPopulated;
    }

    public void ChangeRoomByPortalID(int portalID)
    {
        Debug.Log("Artwork count for room " + currentRoomID + ": " + rooms[currentRoomID].Count);

        if (parentDoorID != portalID) // Build a new room with the selected artwork
        {
            parentDoorID = pc.GetPortals()[portalID].GetDestinationID();
            int previousRoomID = currentRoomID++;
            rooms.Add(currentRoomID, new SortedList<int, Artwork>());

            for (int i = 0; i < rooms[previousRoomID].Count; i++)
            {
                Debug.Log("WOOT!");
                TWEANNGenotype champion = rooms[previousRoomID][portalID].GetGenotype();
                champion.Mutate();
                rooms[currentRoomID].Add(i, new Artwork(champion));
            }
            RedrawRoom();
        }
        else // Load the selected room (time travel)
        {
            Debug.Log("Opps - Time travel is not implemented");
        }
    }

    public void RedrawRoom()
    {
        for(int p = 0; p < rooms[currentRoomID].Count; p++)
        {
            pc.GetPortals()[p].PaintDoor(rooms[currentRoomID][p].GetArtwork());
        }
    }

    private void CreatePortals()
    {
        foreach(KeyValuePair<int, Artwork> a in rooms[currentRoomID])
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
