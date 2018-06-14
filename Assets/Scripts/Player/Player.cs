using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    bool isInverted = false;
    public GameObject FPC;

    public void Start()
    {
        //FPC = gameObject;
        isInverted = OptionsMenu.isInverted;
        float yAxis = FPC.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_MouseLook.YSensitivity;
        if (!isInverted && Mathf.Sign(yAxis) < 0)
        {
            yAxis = yAxis * -1;
            FPC.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_MouseLook.YSensitivity = yAxis;

        }
        else if (isInverted && Mathf.Sign(yAxis) > 0)
        {
            yAxis = yAxis * -1;
            FPC.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_MouseLook.YSensitivity = yAxis;
        }
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
            FindObjectOfType<PortalController>().DoTeleport(this, collider.gameObject.GetComponent<Portal>().GetPortalID());
        }
    }

    public void Update()
    {

    }
}
