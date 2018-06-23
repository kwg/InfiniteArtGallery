using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {

    public GameObject thumbnailSlot;
    private Image thumbnail;
    private Sprite defaultThumbnail;

    public void ChangeBorder(Sprite sprite)
    {
        gameObject.GetComponent<Image>().sprite = sprite;
    }

    public void ChangeThumbnail(Sprite sprite)
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
