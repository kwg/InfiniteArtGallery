using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleInvertMouse : MonoBehaviour {

    public bool isInverted = false;

    public void ToggleInvertY()
    {

    }

    private void changeMouseSettings()
    {
        Input.GetAxis("Mouse Y");
    }
}
