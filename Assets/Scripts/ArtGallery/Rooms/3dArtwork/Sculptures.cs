using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sculptures : MonoBehaviour {
    public GameObject voxel;
    TWEANNGenotype geno;
    TWEANN cppn;
    const float PRESENCE_THRESHOLD = -1.0f;
        
    private void Start()
    {
        GameObject voxelProp = Instantiate(voxel) as GameObject;
        voxelProp.transform.position = new Vector3(10, 0, 0);
        Vector3 lenghtOfDimensionsRed = new Vector3(5, 5, 5);
        Vector3 lenghtOfDimensionsBlue = new Vector3(5, 5, 5);
        float voxelSize = 10;
        renderVoxels(GetVoxelCoordinates(lenghtOfDimensionsRed, voxelSize / 2), Color.blue, voxelSize);
        renderVoxels(GetVoxelCoordinates(lenghtOfDimensionsBlue, voxelSize / 2), Color.blue, 1);
    }

    public List<Vector3> GetVoxelCoordinates (Vector3 lenghtOfDimension, float voxelSize)
    {
        float halfVoxelSize = voxelSize / 2;
        //cppn = new TWEANN(geno);
        List<Vector3> voxelCoordinates = new List<Vector3>();
        for (int x = 0; x < lenghtOfDimension[0]; x++)
        {
            for (int y = 0; y < lenghtOfDimension[1]; y++)
            {
                for (int z = 0; z < lenghtOfDimension[2]; z++)
                {
                    float actualX = -(halfVoxelSize * lenghtOfDimension[0] / 2.0f) + halfVoxelSize + x * halfVoxelSize;
                    float actualY = -(halfVoxelSize * lenghtOfDimension[1] / 2.0f) + halfVoxelSize + y * halfVoxelSize;
                    float actualZ = -(halfVoxelSize * lenghtOfDimension[2] / 2.0f) + halfVoxelSize + z * halfVoxelSize;
                    voxelCoordinates.Add(new Vector3(actualX, actualY, actualZ));
                   // float[] outputs = cppn.Process(new float[] { actualX, actualY, actualZ });
                    /*if (outputs[3] > PRESENCE_THRESHOLD) {
                        Debug.Log("in here");
                        GameObject voxelProp = Instantiate(voxel) as GameObject;
                        Renderer rend = voxelProp.gameObject.GetComponent<Renderer>();
                        voxelProp.transform.position = new Vector3(actualX, actualY, actualZ);
                        voxelProp.transform.localScale = new Vector3(voxelSize, voxelSize, voxelSize);
                        rend.material.SetColor("_Color", Color.HSVToRGB(outputs[0], outputs[1], outputs[2]));
                    }*/
                    /*GameObject voxelProp = Instantiate(voxel) as GameObject;
                    Renderer rend = voxelProp.gameObject.GetComponent<Renderer>();
                    voxelProp.transform.position = new Vector3(actualX, actualY, actualZ);
                    voxelProp.transform.localScale = new Vector3(voxelSize, voxelSize, voxelSize);
                    rend.material.SetColor("_Color", Color.red);*/
                }
            }
        }
        return voxelCoordinates;
    }

    public void renderVoxel(float[] outputs, Vector3 voxelCoordinates, float voxelSize)
    {
        
    }

    public void renderVoxels(List<Vector3> voxelCoordinates, Color color, float voxelSize)
    {
        foreach (Vector3 voxelCoordinate in voxelCoordinates)
        {
            GameObject voxelProp = Instantiate(voxel) as GameObject;
            Renderer rend = voxelProp.gameObject.GetComponent<Renderer>();
            voxelProp.transform.position = voxelCoordinate;
            voxelProp.transform.localScale = new Vector3(voxelSize - .01f, voxelSize - .01f, voxelSize - .01f);
            rend.material.SetColor("_Color", color);
        }
    }
    
    public void resizeSculpture(Vector3 resizeVec)
    {

    }
}
