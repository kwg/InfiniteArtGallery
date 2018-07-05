using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Functions : MonoBehaviour {

    public GameObject HUD;

    private const TRAYS tray = TRAYS.functions;
    private int numberOfFunctionSlots = 5;
    private HUD hud;


    new Camera camera;
    ArtGallery ag;

    float interactionDistance = 50f; // maximum distance to check for raycast collision

    List<FunctionSlot> slots;

    private IFunctionItem[] functions;
    private int ActiveSlot { get; set; }


    // Use this for initialization
    void Start() {
        camera = FindObjectOfType<Camera>();
        ag = FindObjectOfType<ArtGallery>();
        slots = new List<FunctionSlot>();

        hud = HUD.GetComponent<HUD>();
        hud.AddSlots(tray, numberOfFunctionSlots);
        ActiveSlot = 0;
        hud.SelectSlot(tray, ActiveSlot);
        functions = new IFunctionItem[numberOfFunctionSlots];

        ag.player.functions = this;

        /*** TESTING SECTION 
         * Adding functions to the hud ***/
        SavedFunction testFunction0 = new SavedFunction
        {
            fTYPE = FTYPE.ID,
        };
        testFunction0.GenerateThumbnail();
        AddFunction(testFunction0);
        //CycleActiveSlot(1);
        SavedFunction testFunction1 = new SavedFunction
        {
            fTYPE = FTYPE.GAUSS,
        };
        testFunction1.GenerateThumbnail();
        AddFunction(testFunction1);
        //CycleActiveSlot(1);
        SavedFunction testFunction2 = new SavedFunction
        {
            fTYPE = FTYPE.SINE,
        };
        testFunction2.GenerateThumbnail();
        AddFunction(testFunction2);
        //CycleActiveSlot(1);
        //SavedFunction testFunction3 = new SavedFunction
        //{
        //    fTYPE = FTYPE.SAWTOOTH,
        //};
        //testFunction3.GenerateThumbnail();
        //AddFunction(testFunction3);
        //CycleActiveSlot(1);
        //SavedFunction testFunction4 = new SavedFunction
        //{
        //    fTYPE = FTYPE.SQUAREWAVE,
        //};
        //testFunction4.GenerateThumbnail();
        //AddFunction(testFunction4);
        //CycleActiveSlot(1);

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
    }

    public void AddFunction(IFunctionItem function)
    {
        if (functions[ActiveSlot] == null) // Only add an item to the active slot if the slot is empty
        {
            functions[ActiveSlot] = function;
            hud.UpdateFunctionThumbnail(ActiveSlot, function.Image);
            CycleActiveSlot(1);
        }
        else // Overwrite inventory slot
        {
            functions[ActiveSlot] = function;
            hud.UpdateFunctionThumbnail(ActiveSlot, function.Image);
        }
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

        foreach(SavedFunction sf in functions)
        {
            if(sf != null && sf.fTYPE == comapre.fTYPE)
            {
                result = true;
                break;
            }
        }

        return result;
    }
}
