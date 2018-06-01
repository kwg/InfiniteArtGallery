using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Portal SpawnPortal()
    {

        Portal p = gameObject.AddComponent<Portal>();
        GameObject portalProp = Instantiate(Resources.Load("portal", typeof(GameObject))) as GameObject;

        return p;
    }
}
