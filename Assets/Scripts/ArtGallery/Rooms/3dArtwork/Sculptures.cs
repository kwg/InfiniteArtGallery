using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sculptures : MonoBehaviour {
    public GameObject voxel;

    Vector3 numVoxelsPerDirection; //must be int, number of voxels in the (x,y,z)/(width,height,depth) directions of the sculture
    Vector3 dimensionsOfSculpture; //can be float, dimensions in the (x,y,z)/(width,height,depth) directions of the sculpture
    

    private void Start()
    {
        GameObject voxelProp = Instantiate(voxel) as GameObject;
        voxelProp.transform.position = new Vector3(10, 0, 0);
    }

    public Vector3[] getVoxelCoordinates ()
    {
        Vector3[] voxelCoordinates = new Vector3[numVoxelsPerDirection[0] * numVoxelsPerDirection[1] * numVoxelsPerDirection[2]];
        for (int x = 0; x < numVoxelsPerDirection[0]; x++)
        {
            for (int y = 0; y < numVoxelsPerDirection[1]; y++)
            {
                for (int z = 0; z < numVoxelsPerDirection[2]; z++)
                {
                    float actualX = -(dimensionsOfSculpture[0] * numVoxelsPerDirection[0] / 2.0f) + dimensionsOfSculpture[0] / 2.0f + x * dimensionsOfSculpture[0];
                    float actualY = -(dimensionsOfSculpture[1] * numVoxelsPerDirection[1] / 2.0f) + dimensionsOfSculpture[1] / 2.0f + y * dimensionsOfSculpture[1];
                    float actualZ = -(dimensionsOfSculpture[2] * numVoxelsPerDirection[2] / 2.0f) + dimensionsOfSculpture[2] / 2.0f + z * dimensionsOfSculpture[2];

                }
            }
        }
    }
}
