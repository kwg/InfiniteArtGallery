using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTree {

    private RoomNode startingRoom;
    private IDoorPainter doorPainter;

    public RoomTree(List<Portal> portals)
    {
        doorPainter = new ColorDoorPainter<Color>();
        startingRoom = new RoomNode(doorPainter);

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
