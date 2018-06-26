using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SculptureFromCPPNTest : MonoBehaviour {
    public GameObject sculpture;
    private GameObject sculptureProp;
    private Vector3 center;
    private float ROTATION_SPEED = 40;

	void Start () {
        sculptureProp = Instantiate(sculpture) as GameObject;
        sculptureProp.AddComponent<Sculptures>();
        sculptureProp.transform.position = new Vector3(0, 1, 0);
    }
	
	// Update is called once per frame
	void Update () {
        sculptureProp.transform.Rotate(0, ROTATION_SPEED * Time.deltaTime, 0);
        if (!PauseMenu.isPaused && Input.GetButtonDown("Fire2"))
        {
            GameObject.Destroy(sculptureProp);
            sculptureProp = Instantiate(sculpture) as GameObject;
            sculptureProp.AddComponent<Sculptures>();
            sculptureProp.transform.position = new Vector3(0, 1, 0);
        }


        if (!PauseMenu.isPaused && Input.GetButtonDown("Fire1"))
        {
            DeleteOldVoxels();
            sculptureProp.GetComponent<Sculptures>().Invoke("Mutate", 0);
            sculptureProp.GetComponent<Sculptures>().Invoke("createSculpture", 0);
            
        }

    }

    public void DeleteOldVoxels()
    {
        foreach (GameObject g in FindObjectsOfType<GameObject>())
        {
            if (g.name == "voxel")
            {
                System.Console.WriteLine("found a voxel!");
                GameObject.Destroy(g);
            }
        }
    }
}
