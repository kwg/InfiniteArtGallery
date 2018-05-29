using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNode {

    List<RoomNode> doors;
    bool isPopulated = false;

    /// <summary>
    /// Create a new RoomNode 
    /// </summary>
    public RoomNode()
    {

    }

    public RoomNode(RoomNode parentRoom)
    {

    }

    public void BuildDoors(List<Portal> portals)
    {
        /* Create doors */
        List<RoomNode> doors = new List<RoomNode>();

        foreach(Portal p in portals)
        {
            doors.Add(new RoomNode());
        }

        isPopulated = true;

    }

    public RoomNode GetRoomByPortalID(int portalID)
    {
        return doors[portalID];
    }
}
