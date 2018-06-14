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


    private class RoomConfiguration
    {
        private int parentID;
        private SortedList<int, Artwork> artworks;

        public RoomConfiguration(int parentID, SortedList<int, Artwork> artworks)
        {
            this.parentID = parentID;
            this.artworks = artworks;
        }

        public RoomConfiguration(int numArtworks)
        {
            parentID = -1;
            artworks = new SortedList<int, Artwork>();

            for (int i = 0; i < numArtworks; i++)
            {
                artworks[i] = new Artwork();
            }
        }

        public SortedList<int, Artwork> GetArtworks()
        {
            return artworks;
        }

        public void SetArtwork(int artworkID, Artwork artwork)
        {
            artworks[artworkID] = artwork;
        }

        public int GetParentID()
        {
            return parentID;
        }
    }

    private SortedList<int, RoomConfiguration> history;
    private RoomConfiguration room;

    /* Delete this  */
    Room gameRoom; // configuration of the current room

    // Use this for initialization
    void Start()
    {
        ActivationFunctions.ActivateAllFunctions();
        GameObject roomProp = Instantiate(roomObject) as GameObject;
        gameRoom = roomProp.GetComponent<Room>();

        room = new RoomConfiguration(STARTING_NUM_ARTWORKS);
        history = new SortedList<int, RoomConfiguration>();
        history.Add(history.Count, room);

        gameRoom.InitializeRoom(GetImagesFromArtworks(room.GetArtworks()));


        // spawn player
    }

    public void ChangeRoom(int portalID, int destinationID)
    {
        Debug.Log("Changing room from " + (history.Count - 1) + " through portal " + portalID + " with destinationID " + destinationID);
        gameRoom.ClearReturnPortalDecorations();

        if (portalID == room.GetParentID()) // rewind
        {
            //Debug.Log("Opps - Time travel is not implemented");
            history.Remove(history.Count - 1);
            room = history[history.Count - 1];
        }
        else // forawrd
        {
            int numArtworks = room.GetArtworks().Count;
            Artwork selected = room.GetArtworks()[portalID];
            history.Add(history.Count, new RoomConfiguration(destinationID, new SortedList<int, Artwork>()));
            room = history[history.Count - 1];

            for (int i = 0; i < numArtworks; i++)
            {
                room.SetArtwork(i, new Artwork(selected.GetGenotype().Copy()));
                room.GetArtworks()[i].GetGenotype().Mutate();
                //room.GetArtworks()[i].GenerateImageFromCPPN();
            }
        }

        if (room.GetParentID() != -1) gameRoom.SetReturnPortalDecoration(room.GetParentID());
        gameRoom.InitializeRoom(room.GetParentID(), GetImagesFromArtworks(room.GetArtworks()));
        gameRoom.RedrawRoom();

        // FIXME Teleport the player HERE?
    }

    // Update is called once per frame
    void Update ()
    {
		
	}

    private SortedList<int, Texture2D> GetImagesFromArtworks(SortedList<int, Artwork> artworks)
    {
        SortedList<int, Texture2D> images = new SortedList<int, Texture2D>();
        foreach (KeyValuePair<int, Artwork> art in room.GetArtworks())
        {
            images[art.Key] = art.Value.GetArtwork();
        }
        return images;
    }
}
