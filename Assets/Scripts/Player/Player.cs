using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    // game state flags
    bool hasArtwork;
    bool hasRecurrency;
    bool hasAnimations;
    bool hasSculptures;
    bool hasRobots;
    bool hasSounds;

    int baseNumberOfArtworks;
    int baseNumberOfAnimations;
    int baseNumberOfSculptures;
    int baseNumberOfRobots;
    int baseNumberOfSounds;

    public void Start()
    {

    }
    
    /// <summary>
    /// Handle collisions
    /// </summary>
    /// <param name="collider">Object player collided with</param>
    void OnTriggerEnter(Collider collider)
    {
        /* TAG: portal */
        if(collider.gameObject.tag == "portal")
        {
            if(ArtGallery.DEBUG_LEVEL > ArtGallery.DEBUG.NONE) Debug.Log("player activating portal " + collider.gameObject.GetComponent<Portal>().GetPortalID());
            /* Tell portal controller to handle collision between specified portal and this player */
            FindObjectOfType<Room>().DoTeleport(this, collider.gameObject.GetComponent<Portal>().GetPortalID());
        }
    }

    public void Update()
    {

    }
}
