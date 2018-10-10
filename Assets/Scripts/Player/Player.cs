using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public Inventory inventory { get; set; } // reference to the game inventory
    public Functions functions { get; set; }

    new Camera camera;
    ArtGallery ag;
    public GameObject FPC;
    private bool isInverted;
    float interactionDistance = 30f; // maximum distance to check for raycast collision



    public void Start()
    {
        camera = FindObjectOfType<Camera>();
        ag = FindObjectOfType<ArtGallery>();
        ag.player = this;

        //FPC = gameObject;
        //isInverted = OptionsMenu.isInverted;
        //float yAxis = FPC.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_MouseLook.YSensitivity;
        //if (!isInverted && Mathf.Sign(yAxis) < 0)
       // {
        //    yAxis = yAxis * -1;
         //   FPC.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_MouseLook.YSensitivity = yAxis;

//        }
  //      else if (isInverted && Mathf.Sign(yAxis) > 0)
    //    {
      //      yAxis = yAxis * -1;
      //      FPC.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_MouseLook.YSensitivity = yAxis;
      //  }

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

        /* TAG: Function Pickup */
        if (collider.tag == "FunctionPickup")
        {
            FunctionPickup fp = collider.GetComponent<FunctionPickup>();
            if (!functions.HasFunction(fp.Function))
            {
                functions.AddFunction(fp.Function);
                //ag.ActivateFunction(fp.Function.fTYPE);
                Destroy(fp.gameObject);
            }
            else
            {
                //Destroy(fp.gameObject);

            }
        }
    }

    public void Update()
    {
        // FIXME Move all of this to inventory. pass the collider to inventory and have it figure out what to do next. this is getting messy and hard to debug
        
    }
}
