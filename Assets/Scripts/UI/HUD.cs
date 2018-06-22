using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

    public GameObject hud;
    public Sprite deselectedSlotSprite;
    public Sprite selectedSlotSprite;
    int selectedSlotID;
    List<InventorySlot> slots;

	// Use this for initialization
	void Start () {
        selectedSlotID = 0;
        slots = new List<InventorySlot>();
        foreach (InventorySlot slot in FindObjectsOfType<InventorySlot>())
        {
            if(slot.tag == "invSlot")
            {
                slots.Add(slot);
            }
        }
        //slots.Sort();
        Debug.Log("Found " + slots.Count + " inventory slots in the hud");
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
        slots[inventorySlot].ChangeThumbnail(thumbnail);
    }
    
    // Update is called once per frame
	void Update () {
		

	}
}
