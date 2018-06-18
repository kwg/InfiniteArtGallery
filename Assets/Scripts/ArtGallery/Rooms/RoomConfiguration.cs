using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomConfiguration {

    private RoomConfiguration parentRoom;
    private Artwork[] artworks; 
    private RoomConfiguration[] rooms;

    public RoomConfiguration(RoomConfiguration parentRoom, int returnPortalID, Artwork champion, int numArtworks)
    {
        Debug.Log("Creating a new room with " + numArtworks + " artworks");
        this.parentRoom = parentRoom;

        rooms = new RoomConfiguration[numArtworks];
        artworks = new Artwork[numArtworks];

        rooms[returnPortalID] = parentRoom;
                
        // clone champion to each artwork and mutate
        {
            for (int i = 0; i < numArtworks; i++)
            {
                artworks[i] = new Artwork(champion.GetGenotype().Copy());
                artworks[i].GetGenotype().Mutate();
            }
        }

    }

    public RoomConfiguration(RoomConfiguration parentRoom, int returnPortalID, Artwork champion) : this(parentRoom, returnPortalID, champion, parentRoom.GetArtworks().Length) { }

    public RoomConfiguration(int numArtworks)
    {
        parentRoom = null;
        rooms = new RoomConfiguration[numArtworks];
        artworks = new Artwork[numArtworks];
        for (int i = 0; i < numArtworks; i++)
        {
            artworks[i] = new Artwork();
        }
    }

    public void AddRoom(int artworkID, RoomConfiguration newRoom)
    {
        rooms[artworkID] = newRoom;
    }

    public RoomConfiguration GetParent()
    {
        return parentRoom;
    }

    public int GetParentID()
    {
        int id = -1;

        for(int r = 0; r < rooms.Length; r++)
        {
            if (parentRoom != null && rooms[r] == parentRoom)
            {
                id = r;
                break;
            }
        }

        return id;
    }

    public Artwork[] GetArtworks()
    {
        return artworks;
    }

    public void SetArtwork(int artworkID, Artwork artwork)
    {
        artworks[artworkID] = artwork;
    }

    public RoomConfiguration GetRoomByPortalID(int portalID)
    {
        return rooms[portalID];
    }

    public RoomConfiguration GetRoomByArtwork(Artwork champion)
    {
        RoomConfiguration result = null;
        for(int a = 0; a < artworks.Length; a++)
        {
            if (artworks[a] == champion) result = rooms[a];
            break;
        }

        return result;
    }
}


