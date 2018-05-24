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

    void BuildPopulation()
    {
        /* Create a list of all portals in the room and initialize a random color for each one */
        foreach (Portal portal in FindObjectsOfType<Portal>())
        {
            portals.Add(portal);
            portal.InitializePortal(this, new GenotypePortal<Color>());
            //portal.gameObject.GetComponent<GenotypePortal<Color>>().RandomizeRGB();
        }
    }

   public void TriggerBreeding(Portal selectedPortal)
    {

        
        foreach(Portal portal in portals)
        {
            /* Exit portal */
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
                // all other portals should breed
                GenotypePortal<Color> childGeno = (GenotypePortal<Color>) portal.GetGenotypePortalColor().Crossover(selectedPortal.GetGenotypePortalColor());
                portal.SetGenotype(childGeno);
            }
        }
    }
}
