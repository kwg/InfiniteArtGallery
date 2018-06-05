using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationController : MonoBehaviour {

    List<Portal> portals;
    RoomTree rooms;

    int numPortals = 4;
    int numWalls; // use later if we want to change room chape
    int numPortalsPerWall; // how many portals can we fit on a wall?

    int minRoomSize; // 20? 



	// Use this for initialization
	void Start ()
    {
        for(int i = 0; i < numPortals; i++)
        {
            portals.Add(new Portal());
        }




	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
