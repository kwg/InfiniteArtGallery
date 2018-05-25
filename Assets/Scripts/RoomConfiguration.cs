using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomConfiguration  {

    //ArrayList portalConfigs;
    ArrayList portals;

    Dictionary<int, long> portalConfigs;

    public RoomConfiguration(ArrayList roomPortals) // FIXME: portal count is static this way - fix it to change the number of portals on the fly
    {
        portalConfigs = new Dictionary<int, long>();

        portals = roomPortals;

        foreach (Portal portal in portals)
        {
            portalConfigs.Add(portal.portalID, portal.GetCurrentGenerationID());
        }
    }

    public void ReloadRoomLayout()
    {

        foreach (Portal portal in portals)
        {
            portal.SetGenotype(portalConfigs[portal.portalID]);
        }
    }

}
