using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sculpturePlatform : MonoBehaviour {

    Renderer rend;


    // Use this for initialization
    void Start () {

        rend = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetColor(Color newColor)
    {
        rend.material.color = newColor;
    }
}
