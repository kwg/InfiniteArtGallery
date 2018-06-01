using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Primary controller. Handles rooms and portals (physical) of the scene and links them to the doors in the 
/// population. Controls interaction between each side.
/// </summary>
public class PopulationController : MonoBehaviour {



    /* 
     * Maintains generations and represents a generation as a room
     * 
     * A room has doors with genetics and parents and use a portal that has been 
     * decorated using the results of the gentics and lineage as a model to display in the scene. A Player
     * selects a door by walking into the teleport portal. If the selected portal is a parent of this room, 
     * the previous generation is loaded. While the child room that was left still exists in here, unless the
     * exact same path is selected again, the child room will never(*) be used again. At any time, the path 
     * through the rooms will always be a line with no crossings or branches
     */

    int generationID;

    RoomNode currentRoom; // configuration of the current room
    RoomNode previousRoom;

    /* Static numbers to get 4 portals in a square room */
    int numPortals = 4;
    int numWalls = 4; // use later if we want to change room shape
    int numPortalsPerWall = 1; // how many portals can we fit on a wall?


    // Use this for initialization
    void Start()
    {
        currentRoom = new RoomNode(numPortals);

        

        for (int i = 0; i < numPortals; i++)
        {
            // Position portals into the room correctly
            //portal.transform.parent = GameObject.FindGameObjectWithTag("PrimaryRoom").transform;
            
            

        }

        // spawn player
    }


    private void InitializePopulation()
    {
        // make a list of all portals for THIS room (in case we want to change portals per room)
           // this means EVERY portal needs a unique ID

        // Use that list to create a list of doors

        // use whatever genetics on the doors to get features (color)

        // use portal paint method to decorate the portal with the information from the door



        // making a new room:






    }


    /// <summary>
    /// changes colors of portals to a random color
    /// </summary>
    public void DoColorChange(Portal p)
    {
            p.PaintDoor(new Color(Random.value, Random.value, Random.value));
    }




    // Update is called once per frame
    void Update ()
    {
		
	}
}
