using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class OptionsMenu : MonoBehaviour {

    public Toggle toggle;

    public static bool isInverted = true;


    // Use this for initialization
    void Start () {
        toggle = GetComponent<Toggle>();

        toggle.onValueChanged.AddListener(delegate { ToggleInvertMouse(); });
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ToggleInvertMouse()
    {
        isInverted = isInverted ? false : true;
        Debug.Log("Invert value = " + isInverted);
    }

    public bool IsInverted()
    {
        return isInverted;
    }
}
