using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Controller for portal population. Maintains a list of all portals in the room and 
/// </summary>
public class Population : MonoBehaviour {

    Portal[] portals;
    List<RoomConfiguration> roomConfigs;

    IDoorPainter doorPainter;
    RoomNode testRoom;

    int InnovationID;

    int rewindPortalID;

    static bool DEBUG = true;

	// Use this for initialization
	void Start ()
    {
        doorPainter = new SimpleDoorPainter<Color>();
        testRoom = new RoomNode();

        roomConfigs = new List<RoomConfiguration>();
        InnovationID = 0;
        rewindPortalID = -1;

        BuildPopulation();


	}
	
    /// <summary>
    /// Initialize a population of portals
    /// </summary>
    private void BuildPopulation()
    {
        portals = new Portal[FindObjectsOfType<Portal>().Length];
        /* Create a list of all portals in the room and initialize a random color for each one */
        ConsoleDebug("Start init:");
        foreach (Portal portal in FindObjectsOfType<Portal>())
        {
            portals[portal.portalID] = portal;
            //portal.InitializePortal(this, new GenotypePortal<Color>());
        }

        SaveRoomConfig();
        ConsoleRoomDebug("Finished init");
    }

    private void SaveRoomConfig()
    {
        roomConfigs.Add(new RoomConfiguration(portals, rewindPortalID));
    }

    private void LoadRoomConfig(int roomID)
    {
        roomConfigs[roomID].ReloadRoomLayout();


    }

    /// <summary>
    /// Controls breeding and mutation of portals based on the selected portal
    /// </summary>
    /// <param name="selectedPortal">Portal with the attributes that was selected by the player</param>
    public void TriggerBreeding(Portal selectedPortal)
    {
        int pid = selectedPortal.portalID;
        int did = selectedPortal.destinationID;
        ConsoleDebug("Start TriggerBreeding: selectedPortalID = " + pid + ", destinationID = " + did);

        /* rewind evolution */
        /* If the selected portal is the designated "rewind" portal, revert room to previous state */
        if(selectedPortal.portalID == rewindPortalID)
        {
            ConsoleDebug("REWIND - Portal slected with ID " + selectedPortal.portalID);
            roomConfigs.RemoveAt(roomConfigs.Count - 1);
            ConsoleDebug("Returning to room configuration " + (roomConfigs.Count - 1));
            LoadRoomConfig(roomConfigs.Count - 1);
            if(roomConfigs.Count >= 0)
            {
                rewindPortalID = roomConfigs[roomConfigs.Count - 1].getRewindPortalID();
            }
            else
            {
                rewindPortalID = -1;
            }

            SetRewindPortalColor();
        }

        /* advance evolution */
        else
        {
            ConsoleDebug("FORWARD - Evolving room " + (roomConfigs.Count - 1) + " with selected portal " + pid);

            /* Store current room configuration */
            //SaveRoomConfig();

            /* Loop through every portal in the population and decide how to handle it based on its relation
             * to the selected portal.
             *  RULES:
             *  selected portal should remain the same
             *  exit portal should be a path to the previous room configuration
             *  remaining portals should breed with selected portal
             */
            foreach(Portal portal in portals)
            {
                /* handle exit portal */
                if(portal.portalID == selectedPortal.destinationID)
                {
                    /* set rewind portal color to black and set rewind id to this portal */
                    rewindPortalID = portal.portalID;
                    ConsoleDebug("Setting portal " + portal.portalID + " to rewind portal");
                }

                /* selectedPortal */
                else if(portal.portalID == selectedPortal.portalID)
                {
                    // Do nothing - leave the selectedPortal unchanged
                    ConsoleDebug("Setting portal " + portal.portalID + " to itself (champion)");
                }

                /* All other portals */
                else
                {
                    // all other portals should breed with the selected portal
                    //GenotypePortal<Color> mostFit = selectedPortal.GetGenotypePortal();
                    //GenotypePortal<Color> lessFit = portal.GetGenotypePortal();

                    //GenotypePortal<Color> childGeno = (GenotypePortal<Color>) lessFit.Crossover(mostFit);
                    //portal.AddGenotype(childGeno);
                    ConsoleDebug("Crossing portal " + portal.portalID + " with champion (" + pid + ")");
                }

            }

            SaveRoomConfig();
            SetRewindPortalColor();
        }

        ConsoleRoomDebug("Finished TriggerBreeding: rewindID = " + rewindPortalID);
    }

    private void SetRewindPortalColor()
    {
        ConsoleDebug("Setting portal " + rewindPortalID + " color to BLACK.");
        if (rewindPortalID != -1)
        {
            portals[rewindPortalID].PaintDoor(new Color(0, 0, 0));
        }
    }

    public int GetInnovationID()
    {
        return InnovationID++;
    }

    private void ConsoleDebug(string msg)
    {
        if (DEBUG)
        {
            Debug.Log("Population: " + msg);
        }
    }

    private void ConsoleRoomDebug(string msg)
    {
        string r = "Population: " + msg + " | Room: room " + (roomConfigs.Count - 1) + ", portals  [";
        if (DEBUG)
        {
            for (int p = 0; p < portals.Length; p++)
            {
                
                if(p < portals.Length - 1)
                    r += roomConfigs[roomConfigs.Count - 1].GetPortalGenerationByID(p) + ", ";
                else
                {
                    r += roomConfigs[roomConfigs.Count - 1].GetPortalGenerationByID(p);
                }
            }
        }
        r += "]";

        Debug.Log(r);
    }
}
