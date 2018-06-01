using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNode {

    // Links
    SortedList<int, Door<Color>> doors;
    int parentDoorID;
    
    
    bool isPopulated = false;

    /// <summary>
    /// Create a new RoomNode 
    /// </summary>
    public RoomNode(int numberOfDoors)
    {
        /* Create doors */
        doors = new SortedList<int, Door<Color>>();

        for(int d = 0; d < numberOfDoors; d++)
        {
            doors.Add(d, new Door<Color>(d));
        }

        isPopulated = true;
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

    private void LoadRoom()
    {
        foreach (KeyValuePair<int, Door<Color>> d in doors)
        {
            
        }
    }
}
