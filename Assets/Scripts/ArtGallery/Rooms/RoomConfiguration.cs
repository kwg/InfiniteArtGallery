using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomConfiguration {

    public int ArtArchetypeIndex { get; set; }

    public RoomConfiguration parentRoom { get; set; }

    //KNC these might be in the wrong place. let's talk about these and see if we should move them... or maybe we're just thinking about this class incorrectly
    public Artwork[] artworks { get; set; }
    public Sculpture[] scultures { get; set; }

    public RoomConfiguration[] rooms { get; set; }

    // mutation
    private int MUTATION_CYCLES = 5; // maximum mutations per evolution

    public RoomConfiguration(RoomConfiguration parentRoom, int returnPortalID, int championPortalID, Artwork champion, int numArtworks)
    {
        if(ArtGallery.DEBUG_LEVEL < ArtGallery.DEBUG.NONE) Debug.Log("Creating a new room with " + numArtworks + " artworks");
        this.parentRoom = parentRoom;

        rooms = new RoomConfiguration[numArtworks];
        if (ArtGallery.DEBUG_LEVEL < ArtGallery.DEBUG.NONE) Debug.Log("Clearing artworks...");
        artworks = new Artwork[numArtworks];
        if (ArtGallery.DEBUG_LEVEL < ArtGallery.DEBUG.NONE) Debug.Log("Created new artworks: " + artworks.Length);
        rooms[returnPortalID] = parentRoom;
                
        // clone champion to each artwork and mutate
        for (int i = 0; i < numArtworks; i++)
        {
            TWEANNGenotype geno = new TWEANNGenotype(champion.GetGenotype().Copy());
            // champion art
            if(i == championPortalID)
            {
                //do little
                geno.Mutate();
            }
            // return art
            else if(i == returnPortalID)
            {
                // do nothing - save some cpu
            }
            else
            {
                // all other art
                int mutations = System.Math.Abs(championPortalID - i) + 1;
                for(int m = 0; m < MUTATION_CYCLES - mutations; m++)
                {
                    geno.Mutate();
                }
            }
            artworks[i] = new Artwork(geno);
        }

        //FIXME This does nothing yet. Sculptures need added to the room config
        // Mutate Sculptures
        foreach(Sculpture sculpture in ArtGallery.FindObjectOfType<Room>().GetComponents<Sculpture>())
        {
            sculpture.Mutate();
        }
    }

    public RoomConfiguration(RoomConfiguration parentRoom, int returnPortalID, int championPortalID, Artwork champion) : this(parentRoom, returnPortalID, championPortalID, champion, parentRoom.GetArtworks().Length) { }

    /// <summary>
    /// Constructor for the initial room. invoked once per game
    /// </summary>
    /// <param name="numArtworks"></param>
    public RoomConfiguration(int numArtworks)
    {
        ArtArchetypeIndex = EvolutionaryHistory.NextPopulationIndex();
        EvolutionaryHistory.archetypes[ArtArchetypeIndex] = new TWEANNGenotype(4, 3, ArtArchetypeIndex).Nodes;

        parentRoom = null;
        rooms = new RoomConfiguration[numArtworks];
        artworks = new Artwork[numArtworks];
        for (int i = 0; i < numArtworks; i++)
        {
            artworks[i] = new Artwork(ArtArchetypeIndex);
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


