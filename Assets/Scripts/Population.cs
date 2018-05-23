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
        /* Create a list of all portals in the room */
        foreach (Portal portal in FindObjectsOfType<Portal>())
        {
            portals.Add(portal);
            GenotypePortal geno = portal.gameObject.GetComponent<GenotypePortal>();
            geno.RandomizeRGB();
        }
    }
}
