using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomConfiguration {

    public int ArtArchetypeIndex { get; set; }

    public RoomConfiguration parentRoom { get; set; }

    //FIXME PROTOTYPE these might be in the wrong place. let's talk about these and see if we should move them... or maybe we're just thinking about this class incorrectly
    public Artwork[] artworks { get; set; }
    public Sculpture[] sculptures { get; set; }

    public RoomConfiguration[] rooms { get; set; }

    // mutation
    private int MUTATION_CYCLES = 5; // maximum mutations per evolution

    public RoomConfiguration(RoomConfiguration parentRoom, int returnPortalID, int championPortalID, Artwork[] artworksPassed, Sculpture[] sculptures)
    {
        ArtGallery ag = ArtGallery.GetArtGallery();
        Artwork champion = artworksPassed[championPortalID];

        if (ArtGallery.DEBUG_LEVEL < ArtGallery.DEBUG.NONE) Debug.Log("Creating a new room with " + artworksPassed.Length + " artworks");
        this.parentRoom = parentRoom;

        rooms = new RoomConfiguration[artworksPassed.Length];
        if (ArtGallery.DEBUG_LEVEL < ArtGallery.DEBUG.NONE) Debug.Log("Clearing artworks and sculptures...");
        artworks = new Artwork[artworksPassed.Length];
        this.sculptures = sculptures;
        if (ArtGallery.DEBUG_LEVEL < ArtGallery.DEBUG.NONE) Debug.Log("Created new artworks: " + artworksPassed.Length);
        rooms[returnPortalID] = parentRoom;
        
        // clone champion to each artwork and mutate
        for (int i = 0; i < artworksPassed.Length; i++)
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
                for (int m = 0; m < MUTATION_CYCLES - mutations; m++)
                {
                    geno.Mutate();
                }
            }
            artworks[i] = new Artwork(geno);
        }

        Sculpture[] toMutate = new Sculpture[sculptures.Length];
        TWEANNGenotype sculptureGeno = null;
        for (int s = 0; s < sculptures.Length; s++)
        {
            if (sculptures[s].GetSelected())
            {
                sculptureGeno = new TWEANNGenotype(sculptures[s].GetGenotype().Copy());
                sculptures[s].SetSelected(false);
            }
            else
            {
                toMutate[s] = sculptures[s];
            }
        }

        if(sculptureGeno != null)
        {
            for(int m = 0; m < sculptures.Length; m++)
            {
                Sculpture ms = toMutate[m];
                if (ms != null)
                {
                    for(int mr = 0; mr < Random.Range(1,5); mr++)
                    {
                        sculptureGeno.Mutate();
                    }

                    ms.NewSculpture(sculptureGeno);

                }
            }

        }

    }


    public void UpdateArtwork(Artwork art)
    {

    }


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
        sculptures = new Sculpture[4]; // HACK PROTOTYPE hardcoded var. fix later
        for (int i = 0; i < numArtworks; i++)
        {
            artworks[i] = new Artwork(ArtArchetypeIndex);
        }

        //SetSculptures HAS to be called right after this constructor is called and initialized to create the lobby! this will change later
    }

    public void SetSculptures(Sculpture[] sculptures)
    {
        this.sculptures = sculptures;
    }

    public void AddRoom(int artworkID, RoomConfiguration newRoom)
    {
        rooms[artworkID] = newRoom;
    }

    public void RemoveRoom(int portalID)
    {
        rooms[portalID] = null;
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


