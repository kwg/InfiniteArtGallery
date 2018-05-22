using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "portal")
        {

            Vector3 newDestination = collider.gameObject.GetComponent<Portal>().Destination;

            {
               gameObject.transform.position = newDestination;
            }

        }
    }
}
