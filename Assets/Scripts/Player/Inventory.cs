using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    Texture2D[] savedArt;
    public GameObject hud0;
    public GameObject hud1;
    public GameObject hud2;
    public GameObject hud3;
    public GameObject hud4;
    GameObject[] hudImages;
	
	void Start() {
        hudImages = new GameObject[] { hud0, hud1, hud2, hud3, hud4 };
        savedArt = new Texture2D[hudImages.Length];
    }
	
    public void AddArt(int position, Texture2D art)
    {
        if(position < 0 || position > savedArt.Length - 1)
        {
            throw new System.ArgumentOutOfRangeException("Attempted to add artwork to inventory at position " + position + ", but was out of range.");
        }
        else
        {
            savedArt[position] = art;
            hudImages[position].GetComponent<Image>().sprite = Sprite.Create(savedArt[position], new Rect(0, 0, savedArt[position].width, savedArt[position].height), new Vector2(0.5f, 0.5f));
            Debug.Log("Image saved at position " + position);
        }
    }
}
