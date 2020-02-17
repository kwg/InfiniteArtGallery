using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad
{
//    public static List<SaveGameRoomConfiguration> roomTree = new List<SaveGameRoomConfiguration>();

//    public static void Save(ArtGallery artGallery)
//    {
//        /* roomconfig
//         * artwork[] artworks
//         *   - tweanngenotype geno
//         *     - list<nodes> nodes
//         *       - long innovation
//         *       - FTYPE fTYPE
//         *       - NTYPE nTYPE
//         *       - float bias
//         *     - list<links> links
//         *       - long innovation
//         *       - long sourceInnovation
//         *       - long targetInnovation
//         *       - float weight
//         *       - bool active (not in use)
//         *     - long id
//         *     - int numberInputs
//         *     - int numberOutputs
//         *     - int archetypeIndex
//         * sculpture[] sculptures
//         *   - tweanngenotype geno
//         *     - list<nodes> nodes
//         *       - long innovation
//         *       - FTYPE fTYPE
//         *       - NTYPE nTYPE
//         *       - float bias
//         *     - list<links> links
//         *       - long innovation
//         *       - long sourceInnovation
//         *       - long targetInnovation
//         *       - float weight
//         *       - bool active (not in use)
//         *     - long id
//         *     - int numberInputs
//         *     - int numberOutputs
//         *     - int archetypeIndex
//         *     - bool transparency
//         * Roomconfiguration ParentRoom
//         * RoomConfiguration[] linkedRooms
//         */


//        //FIXME This needs to be recursive! This will take a bit of planning.


//        // Get the lobby
//        RoomConfiguration lobby = artGallery.GetLobby();
//        // Build a new save for the room config
//        SaveGameRoomConfiguration newRoom = new SaveGameRoomConfiguration
//        {
//            ParentRoom = null,
//            LinkedRooms = new SaveGameRoomConfiguration[lobby.rooms.Length],
//            Artworks = new SaveGameArtwork[lobby.artworks.Length],
//            //Sculptures = new SaveGameSculpture[lobby.sculptures.Length],
//        };
//        // Add the rooom
//        roomTree.Add(newRoom);

//        //Now get each linked room to the lobby (traverse the tree)
//        SaveGameRoomConfiguration[] tempLinkedRooms = new SaveGameRoomConfiguration[lobby.rooms.Length];
//        for (int i = 0; i < tempLinkedRooms.Length; i++)
//        {
//            tempLinkedRooms[i] = new SaveGameRoomConfiguration
//            {
//                ParentRoom = newRoom
//            };
//        }
//        // Get a reference to the index given to the new room
//        int newRoomIndex = roomTree.IndexOf(newRoom);




//    }
}

public class SaveGameRoomConfiguration
{
    public SaveGameRoomConfiguration ParentRoom { get; set; }
    public SaveGameRoomConfiguration[] LinkedRooms { get; set; }
    public SaveGameArtwork[] Artworks { get; set; }
    public SaveGameSculpture[] Sculptures { get; set; }

}

public class SaveGameGenotype
{
    public List<SaveGameNode> Nodes { get; set; }
    public List<SaveGameLink> Links { get; set; }
    public long ID { get; set; }
    public int NumberInputs { get; set; }
    public int NumberOutputs { get; set; }
    public int ArchetypeIndex { get; set; }
}

public class SaveGameNode
{
    public long Innovation { get; set; }
    public FTYPE Ftype { get; set; }
    public NTYPE Ntype { get; set; }
    public float Bias { get; set; }
}

public class SaveGameLink
{
    public long Innovation { get; set; }
    public long SourceInnovation { get; set; }
    public long TargetInnovation { get; set; }
    public float Weight { get; set; }
}

public class SaveGameArtwork
{
    public SaveGameGenotype geno { get; set; }

    SaveGameArtwork(Artwork art)
    {
        TWEANNGenotype TGeno = art.Art.GetGenotype();
        geno = new SaveGameGenotype
        {
            ID = TGeno.ID,
            NumberInputs = TGeno.numInputs,
            NumberOutputs = TGeno.numOutputs,
            ArchetypeIndex = TGeno.archetypeIndex,
        };
    }
}

public class SaveGameSculpture
{
    public SaveGameGenotype geno { get; set; }
    public bool Transparency { get; set; }
}