using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNode {

    List<RoomNode> doors;
    IDoorPainter doorPainter;


    /// <summary>
    /// Create a new RoomNode 
    /// </summary>
    /// <param name="doorPainter"></param>
    public RoomNode(IDoorPainter doorPainter)
    {
        this.doorPainter = doorPainter;
    }

    public void BuildDoors(List<Portal> portals)
    {
        /* Reset doors */
        List<RoomNode> doors = new List<RoomNode>();

        /* Number of portals contained in this room */
        int numPortals = portals.Count;

        foreach(Portal p in portals)
        {
            // TODO logic here for building children nodes based on portals

            /* figure out destination portal ID */
            int destinationPortal = ((numPortals / 2) + p.portalID) % numPortals;






        }
    }
}
