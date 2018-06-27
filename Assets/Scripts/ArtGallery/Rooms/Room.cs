using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO This should be modified to dynamically build the room based on parameters set by ArtGallery. What about a lobby?
/// <summary>
/// The only room in the Art Gallery.
/// </summary>
public class Room : MonoBehaviour
{

    private bool debug = ArtGallery.DEBUG_LEVEL < ArtGallery.DEBUG.NONE;

    public GameObject portalObject;
    public GameObject sculptureObject;
    SortedList<int, Portal> portals; // Portal index is door index
    List<Sculptures> sculpturesCollection;

    int PortalCount = 0; // TODO not in use - decide what to do with it
    int NUM_WALLS = 4;
    int NUM_PORTALS = 4;
    float xSpacing = 9.9f;
    float ySpacing = 2.5f;
    float zSpacing = 9.9f;

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
        foreach (KeyValuePair<int, Portal> p in portals)
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
        ArrayList walls = getWallObjects();
        int numImagesPerWall = images.Length / NUM_WALLS;
        int idSet = 0;
        for(int i = 0; i < NUM_WALLS; i++)
        {
            for (int j = 1; j <= numImagesPerWall; j++)
            {
                Portal p = SpawnPortal(idSet);
                idSet++;
                if (true)//debug)
                {
                    Debug.Log("received portal with ID " + p.GetPortalID());
                    Debug.Log("Wall " + walls[i]);
                }
                float tempZ = ((GameObject)walls[i]).transform.position.z / j;//forward.magnitude;// + (zSpacing / j);
                float tempY = ((GameObject)walls[i]).transform.position.y;// + ySpacing;
                float tempX = ((GameObject)walls[i]).transform.position.x / j;// + (xSpacing / j);
                Quaternion tempRot = ((GameObject)walls[i]).transform.rotation;

                Debug.Log("Portal " + idSet + ":  X=" + tempX + "  Y=" + tempY +" Z=" + tempZ);

                Vector3 wallCenter = new Vector3(tempX, tempY, tempZ);
                Vector3 origin = new Vector3(0, 0, 0);
                Vector3 fromWallTowardCenter = origin - wallCenter;
                Vector3 slightlyAwayFromWall = wallCenter + 0.005f * fromWallTowardCenter;

                //vecs[i];
                //p.transform.position = vecs[i];
                p.transform.position = slightlyAwayFromWall;
                //p.transform.Rotate(new Vector3(0, (-90 * i), 0)); // HACK Hardcoded - fix once rooms can change the number of portals
                p.transform.Rotate(tempRot.eulerAngles);
                p.PaintDoor(images[i]);
            }
        }
    }

    private ArrayList getWallObjects()
    {
        ArrayList walls = new ArrayList();
        foreach (GameObject gameObj in FindObjectsOfType<GameObject>())
        {
            if (gameObj.tag == "wall")
            {
                walls.Add(gameObj);
            }
        }
        return walls;
    }

    private Portal SpawnPortal(int portalID)
    {
        GameObject portalProp = Instantiate(portalObject) as GameObject;
        portalProp.AddComponent<Portal>();
        Portal p = portalProp.GetComponent<Portal>();
        // give each portal an ID
        p.SetPortalID(portalID);

        // give each portal a destination ID
        p.SetDestinationID((2 + p.GetPortalID()) % NUM_PORTALS);
        if (debug) Debug.Log("Portal created with ID " + p.GetPortalID() + " and DestinationId " + p.GetDestinationID());
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
        if (portalID > -1) portals[portalID].SetEmmisive(Color.white);
    }

    public void ClearReturnPortalDecorations()
    {
        foreach (KeyValuePair<int, Portal> p in portals)
        {
            p.Value.SetEmmisive(new Color(0f, 0f, 0f, 0f));

        }
    }
}
