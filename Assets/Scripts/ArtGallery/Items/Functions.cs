using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Handles the interaction between the player and the function bar
/// </summary>
public class Functions : MonoBehaviour {

    public GameObject HUD;


    private const TRAYS tray = TRAYS.functions;
    private HUD hud;
    List<FTYPE> availableFunctions;
    List<GameObject> functionSlots;
    int numberOfFunctionSlots;


    //new Camera camera;
    ArtGallery ag;

    float interactionDistance = 50f; // maximum distance to check for raycast collision

    private FunctionSlot[] slots;

    //private SavedFunction[] functions;
    private int ActiveSlot { get; set; }

    private void Awake()
    {

    }

    // Use this for initialization
    void Start() {
        //camera = FindObjectOfType<Camera>();
        ag = ArtGallery.GetArtGallery();
        //ag = FindObjectOfType<ArtGallery>();
        //availableFunctions = ag.GetAvailableActivationFunctions();
        numberOfFunctionSlots = ActivationFunctions.GetActiveFunctionCount();
        Debug.Log("Number of function slots = " + numberOfFunctionSlots);
        hud = HUD.GetComponent<HUD>();
        functionSlots = hud.AddSlots(tray, numberOfFunctionSlots);
        ActiveSlot = 0;
        hud.SelectSlot(tray, ActiveSlot);
        slots = new FunctionSlot[numberOfFunctionSlots];
        //functions = new SavedFunction[numberOfFunctionSlots];

        foreach(GameObject go in functionSlots)
        {
            slots[ActiveSlot] = go.GetComponent<FunctionSlot>();
            CycleActiveSlot(1);
        }

        ag.player.functions = this;

        foreach (FTYPE f in ActivationFunctions.GetFunctionList())
        {
            Debug.Log("Building tray for " + f.ToString());
            SavedFunction sf = new SavedFunction
            {
                fTYPE = f
            };
            sf.GenerateThumbnail();
            BuildFunctionTray(sf);
        }

    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            CycleActiveSlot(-1);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            CycleActiveSlot(1);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropFunction();
        }

        foreach(FunctionSlot slot in slots)
        {

        }
    }

    public void DropFunction()
    {
        if(slots[ActiveSlot].Count > 0)
        {
            ActivationFunctions.DeactivateFunction(slots[ActiveSlot].SavedFunction.fTYPE);
            slots[ActiveSlot].Count -= 1;
            //hud.UpdateFunctionThumbnail(ActiveSlot);
        }
    }

    void BuildFunctionTray(SavedFunction function)
    {
        slots[ActiveSlot].SavedFunction = function;
        hud.UpdateFunctionThumbnail(ActiveSlot, function.Image);
        //ag.ActivateFunction(slots[ActiveSlot].FType);
        CycleActiveSlot(1);
    }

    public void AddFunction(IFunctionItem function)
    {
        //Debug.Log("Function picked up");
        foreach(FunctionSlot slot in slots)
        {

            if (slot.SavedFunction.fTYPE == function.fTYPE)
            {
                //Debug.Log("Found it");
                ActivationFunctions.ActivateFunction(function.fTYPE);
                slot.SetCount(slot.Count + 1);
            }
        }

        /*
        if (functions[ActiveSlot] == null) // Only add an item to the active slot if the slot is empty
        {
            functions[ActiveSlot] = function;
            
            hud.UpdateFunctionThumbnail(ActiveSlot, function.Image);
            ag.ActivateFunction(functions[ActiveSlot].fTYPE);
            CycleActiveSlot(1);
        }
        else // Overwrite inventory slot
        {
            ag.DeactivateFunction(functions[ActiveSlot].fTYPE);
            functions[ActiveSlot] = function;
            hud.UpdateFunctionThumbnail(ActiveSlot, function.Image);
            ag.ActivateFunction(functions[ActiveSlot].fTYPE);
        }
        */
    }


    public void ChangeActiveSlot(int newSlot)
    {
        ActiveSlot = newSlot;
    }

    public void CycleActiveSlot(int delta)
    {
        if (delta == -1 || delta == 1) // ensure delta is a single +/- int
        {
            int oldSlot = ActiveSlot;
            ActiveSlot = ActiveSlot + delta;
            if (ActiveSlot < 0) ActiveSlot = numberOfFunctionSlots - 1;
            if (ActiveSlot >= numberOfFunctionSlots) ActiveSlot = 0;
            hud.SelectSlot(tray, ActiveSlot);
        }
        else
        {
            throw new ArgumentException("Delta was not +/- 1");
        }
    }

    public bool HasFunction(SavedFunction comapre)
    {
        bool result = false;

        foreach(FunctionSlot fs in slots )
        {
            if(fs.SavedFunction != null && fs.SavedFunction.fTYPE == comapre.fTYPE)
            {
                result = true;
                break;
            }
        }

        return false;
    }
}
