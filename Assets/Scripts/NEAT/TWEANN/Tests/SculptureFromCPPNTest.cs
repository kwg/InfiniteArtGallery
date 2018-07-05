using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SculptureFromCPPNTest : MonoBehaviour {
    public GameObject sculptureObject;
    public GameObject VoxelObject;
    private GameObject sculptureProp;
    private Sculpture sculpture;
    private Vector3 center;
    private Vector3 rotSave;
    private float ROTATION_SPEED = 40;
    private float rotationY;

	void Start () {
        sculptureProp = Instantiate(sculptureObject) as GameObject;
        sculptureProp.AddComponent<Sculpture>();
        sculpture = sculptureProp.GetComponent<Sculpture>();
        sculpture.LoadVoxel(VoxelObject);
        sculptureProp.transform.position = new Vector3(0, 1, 0);
        rotSave = sculptureProp.transform.rotation.eulerAngles;
        rotationY = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {

        rotationY = (ROTATION_SPEED * Time.deltaTime) % 180;

        sculptureProp.transform.Rotate(0, rotationY, 0);
        if (!PauseMenu.isPaused && Input.GetButtonDown("Fire2"))
        {
            //Destroy(sculptureProp);
            //sculptureProp = Instantiate(sculptureObject) as GameObject;
            //sculptureProp.AddComponent<Sculpture>();
            //sculptureProp.transform.position = new Vector3(0, 1, 0);
            sculpture.NewSculpture();
        }


        if (!PauseMenu.isPaused && Input.GetButtonDown("Fire1"))
        {
            rotSave = sculptureProp.transform.rotation.eulerAngles;
            sculptureProp.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            DeleteOldVoxels();
            sculpture.Mutate();
            sculpture.CreateSculture();
            sculptureProp.transform.Rotate(rotSave);          
        }

    }

    public void DeleteOldVoxels()
    {
        foreach (GameObject g in FindObjectsOfType<GameObject>())
        {
            if (g.name == "voxel")
            {
                Destroy(g);
            }
        }
    }
}
