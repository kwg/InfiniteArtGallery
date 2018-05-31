using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    /// <summary>
    /// Handle collisions
    /// </summary>
    /// <param name="collider">Object player collided with</param>
    void OnTriggerEnter(Collider collider)
    {
        /* TAG: portal */
        if(collider.gameObject.tag == "portal")
        {
            /* Tell population controller to handle collision between specified portal and this player */
            FindObjectOfType<PopulationController>().DoTeleport(this, collider.gameObject.GetComponent<Portal>().portalID);
        }
    }
}
