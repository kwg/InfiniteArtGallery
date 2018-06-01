using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour {

    public GameObject portalObject;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Portal SpawnPortal()
    {

        GameObject portalProp = Instantiate(portalObject) as GameObject;
        Portal p = portalProp.AddComponent<Portal>();

        return p;
    }

    public static PortalController GetPortalController() {
        return FindObjectOfType<PortalController>();
    }
}
