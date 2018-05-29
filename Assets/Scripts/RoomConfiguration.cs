using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomConfiguration  {

   /* collection of portals in the room */
    Portal[] portals;

    /* generation ID for each portal */
    int[] portalConfigs;

    int rewindPortalID;

    /* debug flag */
    static bool DEBUG = true;

    // TODO Portal count is static this way - fix it to change the number of portals on the fly
    /// <summary>
    /// Configuration of each portal in the room
    /// </summary>
    /// <param name="roomPortals">Collection of portals in the room</param>
    public RoomConfiguration(Portal[] roomPortals, int rewindID) 
    {
        ConsoleDebug("Starting init: ");
        portals = roomPortals;
        rewindPortalID = rewindID;

        // HACK This is not safe!! If portal IDs are ever changed or assigned in a non sequential way this will crash
        portalConfigs = new int[portals.Length];

        foreach (Portal portal in portals)
        {
            portalConfigs[portal.portalID] =  portal.GetCurrentGenerationID();
        }

        ConsolPortalConfigsOutput("Init complete: portals: ");
    }

    public void ReloadRoomLayout()
    {
        ConsoleDebug("ReloadRoomLayout()");
        foreach (Portal portal in portals)
        {
            portal.SetGenotype(portalConfigs[portal.portalID]);
        }
    }

    public int GetPortalGenerationByID(int portalID)
    {
        return portalConfigs[portalID];
    }

    public int getRewindPortalID()
    {
        return rewindPortalID;
    }

    private void ConsoleDebug(string msg)
    {
        string r = "RoomConfiguration: ";
        if (DEBUG)
        {
            Debug.Log(r + msg);
        }
    }

    private void ConsolPortalOutput(string msg)
    {
        string r = "RoomConfiguration: " + msg + "RoomConfiguration.portals[";

        if (DEBUG)
        {
            for (int p = 0; p < portals.Length; p++)
            {
                if (p < portals.Length - 1)
                    r += portals[p].portalID + ", ";
                else
                {
                    r += portals[p].portalID;
                }
            }
        }

        r += "]";
        Debug.Log(r);
    }

    private void ConsolPortalConfigsOutput(string msg)
    {
        string r = "RoomConfiguration: " + msg + "RoomConfiguration.portalsConfig: [";

        if (DEBUG)
        {
            for (int p = 0; p < portalConfigs.Length; p++)
            {
                if (p < portalConfigs.Length - 1)
                    r += portalConfigs[p] + ", ";
                else
                {
                    r += portalConfigs[p];
                }
            }
        }

        r += "]";
        Debug.Log(r);
    }

}
