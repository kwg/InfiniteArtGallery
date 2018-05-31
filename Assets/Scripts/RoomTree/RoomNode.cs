using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNode {

    // Links
    SortedList<int, RoomNode> links;
    SortedList<int, >
    

    bool isPopulated = false;

    /// <summary>
    /// Create a new RoomNode 
    /// </summary>
    public RoomNode()
    {
        /* Create doors */
        doors = new SortedList<int, RoomNode>();
    }

    public void BuildDoors(List<Portal> portals)
    {
        foreach(Portal p in portals)
        {
            doors.Add(p.portalID, new RoomNode());
        }

        isPopulated = true;

    }

    public void SetParentRoom(RoomNode parentRoom, int returnPortalID)
    {
        doors[returnPortalID] = parentRoom;
    }

    public RoomNode GetRoomByPortalID(int portalID)
    {
        return doors[portalID];
    }

    public bool IsPopulated()
    {
        return isPopulated;
    }

    public List<Portal> LoadRoom()
    {
        return portals;
    }

}
