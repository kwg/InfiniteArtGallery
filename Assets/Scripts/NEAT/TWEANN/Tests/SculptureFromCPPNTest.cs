using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SculptureFromCPPNTest : MonoBehaviour {
    public GameObject sculpture;
    private GameObject sculptureProp;
    private Vector3 center;
    private Vector3 rotSave;
    private float ROTATION_SPEED = 4;
    private float rotationY;

	void Start () {
        sculptureProp = Instantiate(sculpture) as GameObject;
        sculptureProp.AddComponent<Sculptures>();
        sculptureProp.transform.position = new Vector3(0, 1, 0);
        rotSave = sculptureProp.transform.rotation.eulerAngles;
        rotationY = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {

        rotationY = sculptureProp.transform.rotation.eulerAngles.y + (ROTATION_SPEED * Time.deltaTime);

        sculptureProp.transform.Rotate(0, rotationY, 0);
        if (!PauseMenu.isPaused && Input.GetButtonDown("Fire2"))
        {
            GameObject.Destroy(sculptureProp);
            sculptureProp = Instantiate(sculpture) as GameObject;
            sculptureProp.AddComponent<Sculptures>();
            sculptureProp.transform.position = new Vector3(0, 1, 0);
        }


        if (!PauseMenu.isPaused && Input.GetButtonDown("Fire1"))
        {
            rotSave = sculptureProp.transform.rotation.eulerAngles;
            sculptureProp.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            DeleteOldVoxels();
            sculptureProp.GetComponent<Sculptures>().Invoke("Mutate", 0);
            sculptureProp.GetComponent<Sculptures>().Invoke("createSculpture", 0);
            sculptureProp.transform.Rotate(rotSave);          
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
