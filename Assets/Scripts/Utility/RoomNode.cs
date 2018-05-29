using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNode {

    List<RoomNode> doors;
    IDoorPainter<Color> doorPainter;

    public RoomNode(List<Portal> portals, IDoorPainter<Color> doorPainter)
    {
        this.doorPainter = doorPainter;
        BuildDoors(portals);
    }

    private void BuildDoors(List<Portal> portals)
    {
        foreach(Portal p in portals)
        {
            // TODO logic here for building children nodes based on portals
        }
    }
}
