using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour {

    public Inventory inventory { get; set; } // reference to the game inventory
    public Functions functions { get; set; }

    new Camera camera;
    ArtGallery ag;
    public GameObject FPC;
    private bool isInverted;
    float interactionDistance = 30f; // maximum distance to check for raycast collision
    FirstPersonController controller;


    public void Start()
    {
        controller = FPC.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
        camera = FindObjectOfType<Camera>();
        ag = ArtGallery.GetArtGallery();
        ag.player = this;

        //FPC = gameObject;
        isInverted = ag.invertY;
        float yAxis = controller.m_MouseLook.YSensitivity;
        if (!isInverted && Mathf.Sign(yAxis) < 0)
        {
            yAxis = yAxis * -1;
            controller.m_MouseLook.YSensitivity = yAxis;

        }
        else if (isInverted && Mathf.Sign(yAxis) > 0)
        {
            yAxis = yAxis * -1;
            controller.m_MouseLook.YSensitivity = yAxis;
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
            if(ArtGallery.DEBUG_LEVEL > ArtGallery.DEBUG.NONE) Debug.Log("player activating portal " + collider.gameObject.GetComponent<Portal>().PortalID);
            /* Tell portal controller to handle collision between specified portal and this player */
            FindObjectOfType<Room>().DoTeleport(this, collider.gameObject.GetComponent<Portal>().PortalID);
        }

        /* TAG: sculpturePlatform */
        if (collider.gameObject.tag == "sculpturePlatform")
        {
            if (ArtGallery.DEBUG_LEVEL > ArtGallery.DEBUG.NONE) Debug.Log("player activating sculpture teleport ");
            /* Tell portal controller to handle collision between specified portal and this player */
            FindObjectOfType<Room>().DoTeleport(this, collider.gameObject.GetComponent<SculpturePlatform>().PortalID + 4);

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
        //FIXME find a good place to map all the key/joy binds and ref that here
        if (Input.GetKeyDown(KeyCode.I)) // invert mouse
        {
            controller.m_MouseLook.YSensitivity = controller.m_MouseLook.YSensitivity * -1;
        }

    }

    public void TogglePlayerControlls(bool on)
    {
        controller.enabled = on;
    }
}
