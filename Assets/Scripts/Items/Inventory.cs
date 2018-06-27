using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public GameObject HUD;

    private int numberOfInventorySlots = 9;
    private HUD hud;

    new Camera camera;
    ArtGallery ag;

    float interactionDistance = 50f; // maximum distance to check for raycast collision


    List<InventorySlot> slots;

    private IInventoryItem[] items;
    private int ActiveSlot { get; set; }

    private void Start()
    {
        camera = FindObjectOfType<Camera>();
        ag = FindObjectOfType<ArtGallery>();
        slots = new List<InventorySlot>();
        List<InventorySlot> tempSlots = new List<InventorySlot>();

        foreach (InventorySlot slot in FindObjectsOfType<InventorySlot>())
        {
            tempSlots.Add(slot);
        }

        //if(numberOfInventorySlots != tempSlots.Count) throw new Exception("Expected " + numberOfInventorySlots + " slots in scene but found " + tempSlots.Count);

        for (int s = 0; s < tempSlots.Count; s++)
        {
            string invTag = "inv" + s;
            foreach(InventorySlot slot in tempSlots)
            {
                if(slot.tag == invTag)
                {
                    slots.Add(slot);
                }
            }
        }
        
        hud = HUD.GetComponent<HUD>();
        hud.AddSlots(TRAYS.inventory, 9);
        ActiveSlot = 0;
        hud.SelectSlot(ActiveSlot);
        items = new IInventoryItem[numberOfInventorySlots]; // item storage - contains geno and other data we want to save as well as the thumbnail


    }

    public void AddItem(IInventoryItem item)
    {
        if(items[ActiveSlot] == null) // Only add an item to the active slot if the slot is empty
        {
            items[ActiveSlot] = item;
            hud.UpdateInventoryThumbnail(ActiveSlot, item.Image);
        }
        else // Overwrite inventory slot
        {
            throw new Exception("Inventory overwriting is not implemented");
        }
    }

    public void ChangeActiveSlot(int newSlot)
    {
        ActiveSlot = newSlot;
    }

    public void CycleActiveSlot(int delta)
    {
        if(delta == -1 || delta == 1) // ensure delta is a single +/- int
        {
            int oldSlot = ActiveSlot;
            ActiveSlot = ActiveSlot + delta;
            if (ActiveSlot < 0) ActiveSlot = numberOfInventorySlots - 1;
            if (ActiveSlot >= numberOfInventorySlots) ActiveSlot = 0;
            hud.SelectSlot(ActiveSlot);
        }
        else
        {
            throw new ArgumentException("Delta was not +/- 1");
        }
    }

    public IInventoryItem GetActiveSlotItem()
    {
        return items[ActiveSlot];
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = new Ray(camera.transform.position, camera.transform.forward * interactionDistance);
            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
                if (hit.collider.tag == "portal")
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
                    AddItem(newArtwork);
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
                    ag.GetArtwork(p.GetPortalID()).SetGenotypePortal(GetActiveSlotItem().Geno.Copy()); // FIXME Null ref possible here - add checks
                    ag.GetArtwork(p.GetPortalID()).GenerateImageFromCPPN();
                    ag.GetArtwork(p.GetPortalID()).ApplyImageProcess();
                }
            }
        }

        float wheel = Input.GetAxis("Mouse ScrollWheel");
        if (wheel < 0f)
        {
            //scroll down
            CycleActiveSlot(-1);

        }
        else if (wheel > 0f)
        {
            //scroll up
            CycleActiveSlot(1);

        }
    }
}

