using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTree {

    private RoomNode startingRoom;

    public RoomTree(List<Portal> portals)
    {
        startingRoom = new RoomNode();
        startingRoom.BuildDoors(portals);

    }

    public RoomNode GetStartingRoom()
    {
        return startingRoom;
    }

    public void SetStartingRoom(RoomNode newStartingRoom)
    {
        startingRoom = newStartingRoom;
    }
}
