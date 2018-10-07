using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public GameObject HUD;

    private const TRAYS tray = TRAYS.inventory;
    private int numberOfInventorySlots = 9;
    private HUD hud;

    new Camera camera;
    ArtGallery ag;

    float interactionDistance = 50f; // maximum distance to check for raycast collision

    List<InventorySlot> slots;

    private IInventoryItem[] items;
    private TWEANNGenotype sculptureGeno;
    private int ActiveSlot { get; set; }

    private void Start()
    {
        camera = FindObjectOfType<Camera>();
        ag = FindObjectOfType<ArtGallery>();
        slots = new List<InventorySlot>();
        items = new IInventoryItem[numberOfInventorySlots]; // item storage - contains geno and other data we want to save as well as the thumbnail

        hud = HUD.GetComponent<HUD>();
        hud.AddSlots(tray, numberOfInventorySlots);
        ActiveSlot = 0;
        hud.SelectSlot(tray, ActiveSlot);

        ag.player.inventory = this;
    }

    private void FindNextEmptySlot()
    {
        for(int i = 0; i < items.Length; i++)
        {
            if(items[i] == null)
            {
                ChangeActiveSlot(i);
                hud.SelectSlot(tray, ActiveSlot);
                break;
            }
        }
    }


    private bool Contains(IInventoryItem comp) // FIXME compare isnt working for items - switch compare to geno ID?
    {
        bool result = false;
        foreach(IInventoryItem i in items)
        {
            if (i == comp)
            {
                result = true;
                break;
            }
        }
        return result;
    }

    public void AddItem(IInventoryItem item)
    {
        if(items[ActiveSlot] == null && !Contains(item)) // Only add an item to the active slot if the slot is empty
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
            hud.SelectSlot(tray, ActiveSlot);
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
                    Texture2D img = new Texture2D(p.GetImage().width, p.GetImage().height, TextureFormat.ARGB32, false);
                    Graphics.CopyTexture(p.GetImage(), img);
                    int portalID = p.GetPortalID();
                    TWEANNGenotype geno = ag.GetArtwork(portalID).GetGenotype().Copy();

                    SavedArtwork newArtwork = new SavedArtwork
                    {
                        Image = Sprite.Create(img, new Rect(0, 0, img.width, img.height), new Vector2(0.5f, 0.5f)) as Sprite,
                        Geno = geno

                    };
                    AddItem(newArtwork);
                    FindNextEmptySlot();
                }

                if (hit.collider.tag == "sculpture")
                {
                    Sculpture s = hit.collider.gameObject.GetComponent<Sculpture>();

                    sculptureGeno = s.GetComponent<Sculpture>().GetGenotype();

                    ag.SelectSculpture(s);
                    //s.GetComponent<Sculpture>().SetSelected(!s.GetComponent<Sculpture>().GetSelected());

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
                    if(GetActiveSlotItem() != null)
                    {
                        ag.GetArtwork(p.GetPortalID()).SetGenotype(GetActiveSlotItem().Geno.Copy()); // FIXME Null ref possible here - add checks
                        ag.GetArtwork(p.GetPortalID()).Refresh();
                        ag.GetArtwork(p.GetPortalID()).ApplyImageProcess();
                        items[ActiveSlot] = null;
                        hud.UpdateInventoryThumbnail(ActiveSlot, null);
                    }
                    else
                    {
                        // do nothing for now
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Q))
        {
            CycleActiveSlot(-1);
        }
        if (Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.E))
        {
            CycleActiveSlot(1);
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

