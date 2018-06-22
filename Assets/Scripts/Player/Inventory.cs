using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public GameObject HUD;
    private HUD hud;
    private int slotCount;
    private IInventoryItem[] items;
    public event EventHandler<InventoryEventArgs> ItemAdded;
    private int ActiveSlot { get; set; }

    private void Start()
    {
        hud = HUD.GetComponent<HUD>();
        slotCount = hud.NumberOfSlots();
        items = new IInventoryItem[slotCount];
        ActiveSlot = 0;
    }

    public void AddItem(IInventoryItem item)
    {
        hud.UpdateInventoryThumbnail(ActiveSlot, item.Image);
        items[ActiveSlot] = item;
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
            if (ActiveSlot < 0) ActiveSlot = slotCount;
            if (ActiveSlot >= slotCount) ActiveSlot = 0;
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
