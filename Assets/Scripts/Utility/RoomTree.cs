using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTree {

    private RoomNode startingRoom;
    private IDoorPainter<Color> doorPainter;

    public RoomTree(List<Portal> portals)
    {
        doorPainter = new ColorDoorPainter();
        startingRoom = new RoomNode(portals, doorPainter);

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
