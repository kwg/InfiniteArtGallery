using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Inventory : MonoBehaviour {

    private const int SLOTS = 9;
    private List<IInventoryItem> items = new List<IInventoryItem>();
    public event EventHandler<InventoryEventArgs> ItemAdded;
    private int ActiveSlot { get; set; } 
    
    public void AddItem(IInventoryItem item)
    {
        if(items.Count < SLOTS) // add the item
        {
            items.Add(item);
            item.OnPickup();

            if(ItemAdded != null)
            {
                ItemAdded(this, new InventoryEventArgs(item));
            }
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
            ActiveSlot = (ActiveSlot + delta) % SLOTS;
        }
        else
        {
            throw new ArgumentException("Delta was not +/- 1");
        }
    }
}
