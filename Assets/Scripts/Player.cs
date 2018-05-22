using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "portal")
        {
            collider.gameObject.GetComponent<Portal>().SelectPortal(this);
        }
    }
}
