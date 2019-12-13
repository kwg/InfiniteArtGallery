using System;
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
    public GameObject testerIDObject;
    public GameObject countdownObject;
    public GameObject console;

    private int testerID;

    int selectedInventorySlot;
    int selectedFunctionSlot;
    List<GameObject> inventorySlots;
    List<GameObject> functionSlots;

    public bool IsInitialized { get; private set; }

    // Use this for initialization that requires things to be initialized BEFORE any other script executes Start()
    private void Awake()
    {
        selectedInventorySlot = 0;                                                                     // start the player with slot 0 selected
        inventorySlots = new List<GameObject>();
        functionSlots = new List<GameObject>();
        IsInitialized = true;
    }

    // Use this for initialization
    void Start()
    {
        ArtGallery ag = FindObjectOfType<ArtGallery>();

    }

    public List<GameObject> AddSlots(TRAYS tray, int count)
    {
        if (IsInitialized) // Avoid the NPEs
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
                    return functionSlots;
                case TRAYS.inventory:
                    for (int i = 0; i < count; i++)
                    {
                        GameObject inventorySlotProp = Instantiate(inventorySlotObject);
                        inventorySlotProp.transform.SetParent(inventoryTray.transform, false);
                        inventorySlots.Add(inventorySlotProp);
                    }
                    return inventorySlots;
                default:
                    return null;
            }
        }
        else
        {
            return null;
        }
    }

    public void SelectSlot(TRAYS tray, int slot)
    {
        switch (tray)
        {
            case (TRAYS.inventory):
                if (slot < 0 || slot > inventorySlots.Count)
                {
                    throw new System.Exception("Selected slot does not exist in the HUD: " + slot);
                }
                else
                {
                    selectedInventorySlot = slot;
                    UpdateSelectedSlots(TRAYS.inventory);
                }
                break;
            case (TRAYS.functions):
                if (slot < 0 || slot > functionSlots.Count)
                {
                    throw new System.Exception("Selected slot does not exist in the HUD: " + slot);
                }
                else
                {
                    selectedFunctionSlot = slot;
                    UpdateSelectedSlots(TRAYS.functions);
                }
                break;
            default:
                break;
        }
    }

    private void UpdateSelectedSlots(TRAYS tray)
    {
        switch (tray)
        {
            case (TRAYS.inventory):
                // check for active inventory slot and change the decoration for it
                foreach (GameObject invSlotProp in inventorySlots)
                {
                    invSlotProp.GetComponent<InventorySlot>().DeselectSlot();
                }

                inventorySlots[selectedInventorySlot].GetComponent<InventorySlot>().SelectSlot();
                break;
            case (TRAYS.functions):
                // check for active function slot and change the decoration for it
                foreach (GameObject functionSlotProp in functionSlots)
                {
                    FunctionSlot slot = functionSlotProp.GetComponent<FunctionSlot>();
                    slot.DeselectSlot();
                    //slot.SetCount(slot.count);
                }

                functionSlots[selectedFunctionSlot].GetComponent<FunctionSlot>().SelectSlot();
                break;
            default:
                break;
        }
    }

    public void UpdateInventoryThumbnail(int inventorySlot, Sprite thumbnail)
    {
        if (inventorySlot < 0 || inventorySlot > inventorySlots.Count)
        {
            throw new System.Exception("Selected inventory slot does not exist in the HUD: " + inventorySlot);
        }
        else
        {
            if (thumbnail != null)
            {
                inventorySlots[inventorySlot].GetComponent<InventorySlot>().ChangeThumbnail(thumbnail);
            }
            else
            {
                Debug.Log("Thumbnail was null. Resetting thumbnail to default");
                inventorySlots[inventorySlot].GetComponent<InventorySlot>().ResetThumbnail();
            }
        }
    }

    internal void UpdateFunctionThumbnail(int functionSlot, Sprite thumbnail)
    {
        if (functionSlot < 0 || functionSlot > functionSlots.Count)
        {
            throw new System.Exception("Selected function slot does not exist in the HUD: " + functionSlot);
        }
        else
        {
            functionSlots[functionSlot].GetComponent<FunctionSlot>().SetThumbnail(thumbnail);
        }
    }

    internal void UpdateFunctionThumbnail(int functionSlot)
    {
        if (functionSlot < 0 || functionSlot > functionSlots.Count)
        {
            throw new System.Exception("Selected function slot does not exist in the HUD: " + functionSlot);
        }
        else
        {
            functionSlots[functionSlot].GetComponent<FunctionSlot>().SetThumbnail();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote)) // open/close console
        {
            console.GetComponent<Console>().ToggleConsole();
        }

    } 
}