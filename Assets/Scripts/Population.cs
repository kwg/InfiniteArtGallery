using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Population : MonoBehaviour {

    ArrayList portals;

	// Use this for initialization
	void Start ()
    {
        portals = new ArrayList();
        BuildPopulation();
	}
	
	// Update is called once per frame
	void Update ()
    {

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
            //portal.gameObject.GetComponent<GenotypePortal<Color>>().RandomizeRGB();
        }
    }

    /// <summary>
    /// Controls breeding and mutation of portals based on the selected portal
    /// </summary>
    /// <param name="selectedPortal">Portal with the attributes that was selected by the player</param>
    public void TriggerBreeding(Portal selectedPortal)
    {

        /* Loop through every portal in the population and decide how to handle it based on its relation
         * to the selected portal.
         *  RULES:
         *  selected portal should remain the same
         *  exit portal should be an undo? (this will require rooms to have states and maintain a list of states)
         *  remaining portals should breed with selected portal
         */
        foreach(Portal portal in portals)
        {
            /* destination portal */
            if(portal.portalID == selectedPortal.destinationID)
            {
                // destination portal should be a random slection of non selected portals
                int randomID = Random.Range(0, portals.Count);
                while (randomID == selectedPortal.portalID)
                {
                    randomID = Random.Range(0, portals.Count);
                }

                Portal rndPortal = (Portal)portals[randomID];
                portal.SetColor(rndPortal.GetColor());
            }
            /* selectedPortal */
            else if(portal.portalID == selectedPortal.portalID)
            {
                // selected portal should randomize
                portal.SetColor(new Color(Random.value, Random.value, Random.value));

            }
            /* All other portals */
            else
            {
                // all other portals should breed with the selected portal
                GenotypePortal<Color> childGeno = (GenotypePortal<Color>) portal.GetGenotypePortalColor().Crossover(selectedPortal.GetGenotypePortalColor());
                portal.SetGenotype(childGeno);
            }
        }
    }
}
