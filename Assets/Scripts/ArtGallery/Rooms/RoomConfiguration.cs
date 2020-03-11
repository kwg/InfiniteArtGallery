
public class RoomConfiguration {

    public int ArtArchetypeIndex { get; }
    public GeneticArt[] roomArt { get; }


    public RoomConfiguration(int championPortalID, GeneticArt[] _roomArt)
    {
        roomArt = _roomArt;
        GeneticArt champion = roomArt[championPortalID];
        for (int i = 0; i < roomArt.Length; i++)
        {
            roomArt[i].SetGenotype(champion.GetGenotype());
            roomArt[i].Mutate();
        }
    }

    /// <summary>
    /// Constructor for the initial room. invoked once per game
    /// </summary>
    /// <param name="numArtworks"></param>
    public RoomConfiguration(int numArtworks)
    {
        ArtArchetypeIndex = EvolutionaryHistory.NextPopulationIndex();
        EvolutionaryHistory.archetypes[ArtArchetypeIndex] = new TWEANNGenotype(8, 4, ArtArchetypeIndex).Nodes;

        roomArt = new GeneticArt[numArtworks]; 

        for (int i = 0; i < numArtworks; i++)
        {
            roomArt[i] = new GeneticArt(ArtArchetypeIndex);
            //roomArt[i + 1] = new Sculpture();
        }


    }

    public GeneticArt[] GetRoomArt()
    {
        return roomArt;
    }

    public void SetArtwork(int artworkID, GeneticArt artwork)
    {
        roomArt[artworkID] = artwork;
    }

}


