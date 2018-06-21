using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public Inventory inventory;
    new Camera camera;
    float interactionDistance = 30f;

    public void Start()
    {
        camera = FindObjectOfType<Camera>();
        //inventory = new Inventory();
        
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
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = new Ray(camera.transform.position, camera.transform.forward * interactionDistance);
            Debug.DrawRay(camera.transform.position, camera.transform.forward * interactionDistance);

            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
                Debug.Log("Camera - clicked on " + hit.collider.gameObject.tag);

                if(hit.collider.tag == "portal")
                {
                    Texture2D img = hit.collider.gameObject.GetComponent<Portal>().GetImage();
                    

                }

                // Do something with the object that was hit by the raycast.
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
