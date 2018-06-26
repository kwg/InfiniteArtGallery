using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public Inventory inventory; // reference to the game inventory
    new Camera camera;
    ArtGallery ag;
    public GameObject FPC;
    private bool isInverted;
    float interactionDistance = 30f; // maximum distance to check for raycast collision

    public void Start()
    {
        camera = FindObjectOfType<Camera>();
        ag = FindObjectOfType<ArtGallery>();

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
            FindObjectOfType<Room>().DoTeleport(this, collider.gameObject.GetComponent<Portal>().GetPortalID());
        }
    }

    public void Update()
    {
        // FIXME Move all of this to inventory. pass the collider to inventory and have it figure out what to do next. this is getting messy and hard to debug
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = new Ray(camera.transform.position, camera.transform.forward * interactionDistance);
            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
                if(hit.collider.tag == "portal")
                {
                    Portal p = hit.collider.gameObject.GetComponent<Portal>();
                    Texture2D img = new Texture2D(256, 256, TextureFormat.ARGB32, false); // HACK hardcoded width and height
                    Graphics.CopyTexture(p.GetImage(), img);
                    int portalID = p.GetPortalID();
                    TWEANNGenotype geno = ag.GetArtwork(portalID).GetGenotype().Copy();

                    SavedArtwork newArtwork = new SavedArtwork
                    {
                        Image = Sprite.Create(img, new Rect(0, 0, img.width, img.height), new Vector2(0.5f, 0.5f)) as Sprite,
                        Geno = geno

                    };
                    inventory.AddItem(newArtwork);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = new Ray(camera.transform.position, camera.transform.forward * interactionDistance);
            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
                if (hit.collider.tag == "portal")
                {
                    Portal p = hit.collider.gameObject.GetComponent<Portal>();
                    ag.GetArtwork(p.GetPortalID()).SetGenotypePortal(inventory.GetActiveSlotItem().Geno.Copy());
                    ag.GetArtwork(p.GetPortalID()).GenerateImageFromCPPN();
                    ag.GetArtwork(p.GetPortalID()).ApplyImageProcess();
                }
            }
        }

        float wheel = Input.GetAxis("Mouse ScrollWheel");
        if(wheel < 0f )
        {
            //scroll down
            inventory.CycleActiveSlot(-1);

        }
        else if(wheel > 0f)
        {
            //scroll up
            inventory.CycleActiveSlot(1);

        }
    }
}
