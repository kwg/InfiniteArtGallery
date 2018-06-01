﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTree {

    private RoomNode startingRoom;

    public RoomTree()
    {
        startingRoom = new RoomNode(4);

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
