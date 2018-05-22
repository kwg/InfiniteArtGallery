using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "portal")
        {
            //            Color newColor = new Color(Random.value, Random.value, Random.value);
            //            collider.gameObject.GetComponent<ColorChanger>().SetColor(newColor);
            ColorChanger colChanger = collider.gameObject.GetComponent<ColorChanger>();
            Color selectedColor = colChanger.GetCurrentColor();
            colChanger.EvolveColor(selectedColor);

            Vector3 newDestination = collider.gameObject.GetComponent<Portal>().Destination;

            {
               gameObject.transform.position = newDestination;
            }

        }
    }
}
