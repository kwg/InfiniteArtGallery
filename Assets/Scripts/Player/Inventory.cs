using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Inventory : MonoBehaviour {

    public GameObject HUD;
    private HUD hud;
    private int slotCount;
    private List<IInventoryItem> items = new List<IInventoryItem>();
    public event EventHandler<InventoryEventArgs> ItemAdded;
    private int ActiveSlot { get; set; }

    private void Start()
    {
        hud = HUD.GetComponent<HUD>();
        slotCount = hud.NumberOfSlots();
        ActiveSlot = 1;
    }

    public void AddItem(IInventoryItem item)
    {
        //if(items.Count < slotCount) // add the item
        //{
        //    items.Add(item);
        //    item.OnPickup();

        //    if(ItemAdded != null)
        //    {
        //        ItemAdded(this, new InventoryEventArgs(item));
        //        hud.UpdateInventoryThumbnail(ActiveSlot, item.Image);
        //    }
        //}

        hud.UpdateInventoryThumbnail(ActiveSlot - 1, item.Image);
    }

    public void ChangeActiveSlot(int newSlot)
    {
        ActiveSlot = newSlot;
    }

    public void CycleActiveSlot(int delta)
    {
        if(delta == -1 || delta == 1) // ensure delta is a single +/- int
        {
            ActiveSlot = ActiveSlot + delta;
            if (ActiveSlot < 1) ActiveSlot = slotCount;
            if (ActiveSlot > slotCount) ActiveSlot = 1;
            hud.SelectSlot(ActiveSlot - 1);
        }
        else
        {
            throw new ArgumentException("Delta was not +/- 1");
        }
    }
}
