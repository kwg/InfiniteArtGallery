﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FunctionSlot : MonoBehaviour {

    public GameObject slotBorder;
    public GameObject thumbnailSlot;

    public Sprite defaultThumbnail;
    public Sprite borderSelected;
    public Sprite borderDeselected;

    public void SelectSlot()
    {
        slotBorder.GetComponent<Image>().sprite = borderSelected;
    }

    public void DeselectSlot()
    {
        slotBorder.GetComponent<Image>().sprite = borderDeselected;
    }

    public void SetThumbnail(Sprite sprite)
    {
        thumbnailSlot.GetComponent<Image>().sprite = sprite;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
