using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomConfiguration {

    private RoomConfiguration parentRoom;
    private Artwork[] artworks; 
    private RoomConfiguration[] rooms;
    private int MUTATION_CYCLES = 5; // maximum mutations per evolution

    public RoomConfiguration(RoomConfiguration parentRoom, int returnPortalID, int championPortalID, Artwork champion, int numArtworks)
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
                // champion art
                if(i == championPortalID)
                {
                    //do little
                    artworks[i] = new Artwork(champion.GetGenotype().Copy());
                    int mutations = 1;
                    for (int m = MUTATION_CYCLES; m > mutations; m--)
                    {
                        artworks[i].GetGenotype().Mutate();
                    }
                }
                // return art
                if(i == returnPortalID)
                {
                    // do nothing - save some cpu
                }
                if(i != championPortalID || i != returnPortalID)
                {
                    // all other art
                    artworks[i] = new Artwork(champion.GetGenotype().Copy());
                    int mutations = System.Math.Abs(championPortalID - i) + 1;
                    for (int m = MUTATION_CYCLES; m > mutations; m--)
                    {
                        artworks[i].GetGenotype().Mutate();
                    }
                }
            }
        }

    }

    public RoomConfiguration(RoomConfiguration parentRoom, int returnPortalID, int championPortalID, Artwork champion) : this(parentRoom, returnPortalID, championPortalID, champion, parentRoom.GetArtworks().Length) { }

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


