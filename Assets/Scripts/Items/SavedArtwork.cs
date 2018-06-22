using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedArtwork : IInventoryItem {

    public string Name
    {
        get
        {
            throw new System.NotImplementedException();
        }
    }

    public Sprite Image { get; set; }
    public TWEANNGenotype Geno { get; set; }

    public void OnPickup()
    {
        //throw new System.NotImplementedException();
    }


    // Use this for initialization
    void Start () {
	    	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
