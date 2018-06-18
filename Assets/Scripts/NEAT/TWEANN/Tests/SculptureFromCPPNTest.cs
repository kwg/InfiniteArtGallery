using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SculptureFromCPPNTest : MonoBehaviour {
    public GameObject sculpture;
	// Use this for initialization
	void Start () {
        GameObject sculptureProp = Instantiate(sculpture) as GameObject;
        sculptureProp.AddComponent<Sculptures>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
