using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Population : MonoBehaviour {

    ArrayList portals;
    Dictionary<int, RoomConfiguration> roomConfigs;
    int roomID;

    int InnovationID;

    int rewindPortalID;

	// Use this for initialization
	void Start ()
    {
        InnovationID = 0;
        roomID = 0;

        portals = new ArrayList();
        BuildPopulation();

        roomConfigs = new Dictionary<int, RoomConfiguration>();
        SaveRoomConfig();

        rewindPortalID = -1;
	}
	
    /// <summary>
    /// Initialize a population of portals
    /// </summary>
    private void BuildPopulation()
    {
        /* Create a list of all portals in the room and initialize a random color for each one */
        foreach (Portal portal in FindObjectsOfType<Portal>())
        {
            portals.Add(portal);
            portal.InitializePortal(this, new GenotypePortal<Color>());
        }
    }

    private void SaveRoomConfig()
    {
        roomConfigs.Add(roomID++, new RoomConfiguration(portals));

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
        /* rewind evolution */
        if(selectedPortal.portalID == rewindPortalID)
        {
            roomConfigs.Remove(roomID--);
            LoadRoomConfig(roomID);
            rewindPortalID--;
        }

        /*  */ 
        else
        {
            /* Store current room configuration */


            /* Loop through every portal in the population and decide how to handle it based on its relation
             * to the selected portal.
             *  RULES:
             *  selected portal should remain the same
             *  exit portal should be an undo
             *  remaining portals should breed with selected portal
             */
            foreach(Portal portal in portals)
            {
                /* destination portal */
                if(portal.portalID == selectedPortal.destinationID)
                {
                    /*
                    // destination portal should be a random slection of non selected portals
                    int randomID = Random.Range(0, portals.Count);
                    while (randomID == selectedPortal.portalID)
                    {
                        randomID = Random.Range(0, portals.Count);
                    }

                    Portal rndPortal = (Portal)portals[randomID];
                    portal.SetColor(rndPortal.GetColor());
                    */

                    portal.SetColor(new Color(0, 0, 0));
                    rewindPortalID = portal.portalID;
                }
                /* selectedPortal */
                else if(portal.portalID == selectedPortal.portalID)
                {
                    // selected portal should randomize
                    // portal.SetColor(new Color(Random.value, Random.value, Random.value));

                    // Do nothing - leave the selectedPortal unchanged

                }
                /* All other portals */
                else
                {
                    // all other portals should breed with the selected portal
                    GenotypePortal<Color> childGeno = (GenotypePortal<Color>) portal.GetGenotypePortalColor().Crossover(selectedPortal.GetGenotypePortalColor());
                    portal.AddGenotype(childGeno);
                }
            }
        }
    }

    public int GetInnovationID()
    {
        return InnovationID++;
    }
}
