using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO This should be modified to dynamically build the room based on parameters set by ArtGallery. What about a lobby?
/// <summary>
/// The only room in the Art Gallery.
/// </summary>
public class Room : MonoBehaviour {

    private bool debug = ArtGallery.DEBUG_LEVEL < ArtGallery.DEBUG.NONE;

    public GameObject portalObject;
    public GameObject sculptureObject;
    SortedList<int, Portal> portals; // Portal index is door index
    List<Sculptures> sculpturesCollection;
    int PortalCount = 0; // TODO not in use - decide what to do with it

    private int rewindPortalID; // Parent of this room
    private bool isPopulated = false;  // is this room initialized?
    private Texture2D[] images;

    /// <summary>
    /// Initial creation of the room
    /// </summary>
    /// <param name="numberArtworks"></param>
    public void InitializeRoom(Texture2D[] images)
    {
        this.images = images;
        rewindPortalID = -1;
        portals = new SortedList<int, Portal>();
        CreatePortals();
        //CreateSculptures();
        isPopulated = true;
    }

    /// <summary>
    /// Initialize a room with given artworks and parent ID (loading a room)
    /// </summary>
    /// <param name="artworks">SortedList of artwork to be hung on the walls</param>
    public void ConfigureRoom(int rewindPortalID, Texture2D[] images)
    {
        this.rewindPortalID = rewindPortalID;
        /* Create art */
        this.images = images;
        isPopulated = true;

    }

    /* Public methods */
    public void SetParentID(int rewindPortalID)
    {
        this.rewindPortalID = rewindPortalID;
    }

    public int GetRewindPortalID()
    {
        return rewindPortalID;
    }

    public bool IsPopulated()
    {
        return isPopulated;
    }

    public void RedrawRoom()
    {
        foreach(KeyValuePair<int, Portal> p in portals)
        {
            p.Value.PaintDoor(images[p.Key]);
        }
    }

    //just one sculpture for now
    private void CreateSculptures()
    {
        GameObject sculpture = Instantiate(sculptureObject) as GameObject;
        sculpture.AddComponent<Sculptures>();

        //p.transform.position = vecs[img.Key];
        //p.transform.Rotate(new Vector3(0, (-90 * img.Key), 0)); // HACK Hardcoded - fix once rooms can change the number of portals
    }

    private void CreatePortals()
    {
        for(int i = 0; i < images.Length; i++)
        {
            Portal p = SpawnPortal(i);
            if (debug) Debug.Log("received portal with ID " + p.GetPortalID());

            // TODO make a method to do this correctly
            float x_spacing = 9.9f;
            float z_spacing = 9.9f;
            float y_spacing = 2.5f;

            Vector3[] vecs = {
                new Vector3((0 + x_spacing), (0 + y_spacing), 0),
                new Vector3(0, (0 + y_spacing), (0 + z_spacing)),
                new Vector3((0 - x_spacing), (0 + y_spacing), 0),
                new Vector3(0, (0 + y_spacing), (0 - z_spacing)),
            };

            // put each portal on a wall
            p.transform.position = vecs[i];
            p.transform.Rotate(new Vector3(0, (-90 * i), 0)); // HACK Hardcoded - fix once rooms can change the number of portals
            p.PaintDoor(images[i]);
        }
    }

    private Portal SpawnPortal(int portalID)
    {
        GameObject portalProp = Instantiate(portalObject) as GameObject;
        portalProp.AddComponent<Portal>();
        Portal p = portalProp.GetComponent<Portal>();
        // give each portal an ID
        p.SetPortalID(portalID);

        // give each portal a destination ID
        p.SetDestinationID((2 + p.GetPortalID()) % 4);
        if(debug) Debug.Log("Portal created with ID " + p.GetPortalID() + " and DestinationId " + p.GetDestinationID());
        portals.Add(portalID, p);
        return p;
    }

    public void DoTeleport(Player player, int portalID)
    {
        if (debug) Debug.Log("starting teleport form portal " + portalID + " = " + portals[portalID].GetPortalID());
        Vector3 destination = new Vector3(0, 20, 0);
        for (int i = 0; i < portals.Count; i++)
        {
            if (portals[i].GetPortalID() == portals[portalID].GetDestinationID())
            {
                destination = portals[i].gameObject.transform.position; // set destination to exit portal position
            }
        }
        // Bump player to just outside of the portal collision box based on the location of the portal relative to the center
        if (destination.x < 0)
        {
            destination.x += 0.25f;
        }
        else
        {
            destination.x -= 0.25f;
        }

        if (destination.z < 0)
        {
            destination.z += 0.25f;
        }
        else
        {
            destination.z -= 0.25f;
        }

        destination.y -= 1.6f; // Fix exit height for player (player is 1.8 tall, portal is 5, center of portal is 2.5, center of player is 0.9. 2.5 - 0.9 = 1.6)

        player.transform.position = destination;

        /* FIXME Now tell the population controller that the player has moved 
         * by sending the portal (equiv to door) index to the population controller
         * 
         */

        FindObjectOfType<ArtGallery>().ChangeRoom(portalID, portals[portalID].GetDestinationID());
    }

    public void SetReturnPortalDecoration(int portalID)
    {
        if(portalID > -1) portals[portalID].SetEmmisive(Color.white);
    }

    public void ClearReturnPortalDecorations()
    {
        foreach(KeyValuePair<int, Portal> p in portals)
        {
            p.Value.SetEmmisive(new Color(0f, 0f, 0f, 0f));

        }
    }
}
