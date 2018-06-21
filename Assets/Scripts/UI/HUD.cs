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
        Debug.Log("Found " + slots.Count + " inventory slots in the hud");
        UpdateSelectedInventorySlot();

    }

    public void SelectSlot(int slot)
    {
        selectedSlotID = slot;
        UpdateSelectedInventorySlot();
    }

	private void UpdateSelectedInventorySlot()
    {
        // check for active inventory slot and change the decoration for it
        foreach (InventorySlot slot in slots)
        {
            slot.ChangeSprite(deselectedSlotSprite);
        }

        slots[selectedSlotID].ChangeSprite(selectedSlotSprite);
    }
	
    
    // Update is called once per frame
	void Update () {
		

	}
}
