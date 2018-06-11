using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNode {

    private PortalController pc;


    //TODO Room genetics
    
    // Links to other rooms
    private SortedList<int, RoomNode> rooms;

    // Population of doors in this room
    private SortedList<int, Door<Color>> doors;

    private int parentDoorID; // Parent of this room
    private bool isPopulated = false;  // is this room initialized?

    /// <summary>
    /// Create a new RoomNode 
    /// </summary>
    public RoomNode(PortalController pc)
    {
        this.pc = pc;
        doors = new SortedList<int, Door<Color>>();
        rooms = new SortedList<int, RoomNode>();
    }

    /// <summary>
    /// Initialize a room with random colors
    /// </summary>
    /// <param name="numberOfDoors"></param>
    public void InitializeRoom(int numberOfDoors)
    {

        /* Create doors and links to new rooms */
        for (int i = 0; i < numberOfDoors; i++)
        {
            doors.Add(i, new Door<Color>(i, pc));
            rooms.Add(i, new RoomNode(pc));  // in a populated room there should never be a null ref
        }

        isPopulated = true;

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

    public Door<Color> GetDoorByID(int doorID)
    {
        return doors[doorID];
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
        pc.FlushPortals();

        foreach (KeyValuePair<int, Door<Color>> kvpDoors in doors)
        {
            //kvpDoors.Value.RedecorateDoor();
            Debug.Log("RedrawDoor: " + kvpDoors.Value.ToString());
        }
    }
}
