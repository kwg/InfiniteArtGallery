using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Responsible for updating the on screen UI elements of the HUD
/// </summary>
public class HUD : MonoBehaviour {

    public GameObject hud;
    public Sprite deselectedSlotSprite;
    public Sprite selectedSlotSprite;
    int selectedSlotID;
    List<InventorySlot> slots;

	// Use this for initialization
	void Start () {
        selectedSlotID = 0;                                                                     // start the player with slot 0 selected
        slots = new List<InventorySlot>();                                                      // list of inventory slots
        foreach (InventorySlot slot in FindObjectsOfType<InventorySlot>())                      // Add scene UI inventory slots to slots - this may change if the game modifies the number of slots
        {
            if(slot.tag == "invSlot")
            {
                slots.Add(slot);
            }
        }
        slots.Reverse();
        UpdateSelectedInventorySlot();
    }

    public int NumberOfSlots()
    {
        return slots.Count;
    }

    public void SelectSlot(int slot)
    {
        if(slot < 0 || slot > slots.Count)
        {
            throw new System.Exception("Selected slot does not exist in the HUD: " + slot);
        }
        else
        {
            selectedSlotID = slot;
            UpdateSelectedInventorySlot();
        }
    }

	private void UpdateSelectedInventorySlot()
    {
        // check for active inventory slot and change the decoration for it
        foreach (InventorySlot slot in slots)
        {
            slot.ChangeBorder(deselectedSlotSprite);
        }

        slots[selectedSlotID].ChangeBorder(selectedSlotSprite);
    }
	
    public void UpdateInventoryThumbnail(int inventorySlot, Sprite thumbnail)
    {
        if(inventorySlot < 0 || inventorySlot > slots.Count)
        {
            throw new System.Exception("Selected slot does not exist in the HUD: " + inventorySlot);
        }
        else
        {
            slots[inventorySlot].ChangeThumbnail(thumbnail);
        }
    }
    
    // Update is called once per frame
	void Update () {
		

	}
}
