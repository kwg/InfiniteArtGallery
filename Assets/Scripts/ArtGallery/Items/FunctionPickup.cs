using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionPickup : MonoBehaviour {

    public SavedFunction Function { get; set; }

    Renderer rend;

	// Use this for initialization
	void Start () {
        rend = FindObjectOfType<Renderer>();
	}

    // Update is called once per frame
    void Update()
    {
        rend.material.mainTexture = Function.Image.texture;


    }

    public GameObject GetPickup()
    {
        return gameObject;
    }
}
