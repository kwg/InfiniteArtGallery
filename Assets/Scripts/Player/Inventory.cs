using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public GameObject HUD;

    private int numberOfInventorySlots = 9;
    private HUD hud;

    List<InventorySlot> slots;

    private IInventoryItem[] items;
    private int ActiveSlot { get; set; }

    private void Start()
    {
        slots = new List<InventorySlot>();
        List<InventorySlot> tempSlots = new List<InventorySlot>();

        foreach (InventorySlot slot in FindObjectsOfType<InventorySlot>())
        {
            tempSlots.Add(slot);
        }

        if(numberOfInventorySlots != tempSlots.Count) throw new Exception("Expected " + numberOfInventorySlots + " slots in scene but found " + tempSlots.Count);

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
        hud.AddSlots(slots);
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
}
