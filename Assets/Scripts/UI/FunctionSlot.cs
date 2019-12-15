using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Independent slot inside of the function tray
/// Holds functions and their amount
/// selected slot interacts with function
/// </summary>
public class FunctionSlot : MonoBehaviour {

    public GameObject slotBorder;
    public GameObject thumbnailSlot;
    public Text countDisplay;
    public int Count { get; set; }

    public Sprite defaultThumbnail;
    public Sprite borderSelected;
    public Sprite borderDeselected;

    //public FTYPE FType { get; set; }
    public SavedFunction SavedFunction { get; set; }

    public void SelectSlot()
    {
        slotBorder.GetComponent<Image>().sprite = borderSelected;
    }

    public void DeselectSlot()
    {
        slotBorder.GetComponent<Image>().sprite = borderDeselected;
    }

    public void SetCount(int count)
    {
        Count = count;
        countDisplay.text = Count.ToString();   
    }

    public void SetThumbnail(Sprite sprite)
    {
        thumbnailSlot.GetComponent<Image>().sprite = sprite;
    }

    public void SetThumbnail()
    {
        thumbnailSlot.GetComponent<Image>().sprite = defaultThumbnail;
    }

    // Use this for initialization
    void Start () {
        SetCount(0);
	}
	
	// Update is called once per frame
	void Update () {

    }
}
