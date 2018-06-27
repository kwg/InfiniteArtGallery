using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TRAYS { functions, inventory }

/// <summary>
/// Responsible for updating the on screen UI elements of the HUD
/// </summary>
public class HUD : MonoBehaviour
{


    public GameObject functionSlotObject;
    public GameObject inventorySlotObject;
    public GameObject functionTray;
    public GameObject inventoryTray;



    int selectedInvSlot;
    List<GameObject> inventorySlots;
    List<GameObject> functionSlots;

    // Use this for initialization
    void Start()
    {


        selectedInvSlot = 0;                                                                     // start the player with slot 0 selected
        inventorySlots = new List<GameObject>();
        functionSlots = new List<GameObject>();
        AddFunctionSlot();
    }

    public void AddSlots(TRAYS tray, int count)
    {
        switch (tray)
        {
            case TRAYS.functions:
                for (int f = 0; f < count; f++)
                {
                    GameObject functionSlotProp = Instantiate(functionSlotObject);
                    functionSlotProp.transform.SetParent(functionTray.transform, false);
                    functionSlots.Add(functionSlotProp);
                }
                break;
            case TRAYS.inventory:
                for (int i = 0; i < count; i++)
                {
                    GameObject inventorySlotProp = Instantiate(inventorySlotObject);
                    inventorySlotProp.transform.SetParent(inventoryTray.transform, false);
                    inventorySlots.Add(inventorySlotProp);
                }
                break;
            default:
                break;
        }

        UpdateSelectedSlots();
    }

    public void AddFunctionSlot()
    {
        for (int f = 0; f < 4; f++)
        {
            GameObject functionSlotProp = Instantiate(functionSlotObject);
            functionSlotProp.transform.SetParent(functionTray.transform, false);
        }
    }

    public void SelectSlot(int slot)
    {
        if (slot < 0 || slot > inventorySlots.Count)
        {
            throw new System.Exception("Selected slot does not exist in the HUD: " + slot);
        }
        else
        {
            selectedInvSlot = slot;
            UpdateSelectedSlots();
        }
    }

    private void UpdateSelectedSlots()
    {
        // check for active inventory slot and change the decoration for it
        foreach (GameObject invSlotProp in inventorySlots)
        {
            invSlotProp.GetComponent<InventorySlot>().DeselectSlot();
        }

        inventorySlots[selectedInvSlot].GetComponent<InventorySlot>().SelectSlot();
    }

    public void UpdateInventoryThumbnail(int inventorySlot, Sprite thumbnail)
    {
        if (inventorySlot < 0 || inventorySlot > inventorySlots.Count)
        {
            throw new System.Exception("Selected slot does not exist in the HUD: " + inventorySlot);
        }
        else
        {
            inventorySlots[inventorySlot].GetComponent<InventorySlot>().ChangeThumbnail(thumbnail);
        }
    }

    // Update is called once per frame
    void Update()
    {


    }
}