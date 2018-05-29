using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNode {

    RoomNode parentRoom, northRoom, eastRoom, southRoom, westRoom;
    // rewind portal is parent room

    public RoomNode(RoomNode parentRoom, RoomNode northRoom, RoomNode eastRoom, RoomNode southRoom, RoomNode westRoom)
    {
        this.parentRoom = parentRoom;

        this.northRoom = northRoom;
        this.eastRoom = eastRoom;
        this.southRoom = southRoom;
        this.westRoom = westRoom;

    }

    public RoomNode GetNorthRoom()
    {
        return northRoom;
    }

    public RoomNode GetEastRoom()
    {
        return eastRoom;
    }

    public RoomNode GetSouthRoom()
    {
        return southRoom;
    }

    public RoomNode GetWestRoom()
    {
        return westRoom;
    }

    public RoomNode GetparentRoom()
    {
        return parentRoom;
    }

    public void SetNorthRoom(RoomNode northRoom)
    {
        this.northRoom = northRoom;
    }

    public void SetEastRoom(RoomNode eastRoom)
    {
        this.eastRoom = eastRoom;
    }

    public void SetSouthRoom(RoomNode southRoom)
    {
        this.southRoom = southRoom;
    }

    public void SetWesthRoom(RoomNode westRoom)
    {
        this.westRoom = westRoom;
    }

    public RoomNode GetParentRoom()
    {
        return parentRoom;
    }

}
