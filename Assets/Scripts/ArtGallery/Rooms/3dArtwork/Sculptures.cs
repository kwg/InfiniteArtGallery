using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sculptures : MonoBehaviour {
    public GameObject voxel;
    TWEANNGenotype geno;
    TWEANN cppn;
    const float PRESENCE_THRESHOLD = .1f;
    const float BIAS = 1;

    private void Start()
    {
        //inputs: (x,y,z) outputs: r,g,b and presence
        geno = new TWEANNGenotype(4, 4, 0); // FIXME archetype index 
        ActivationFunctions.ActivateAllFunctions();
        GenerateCPPN();
        float voxelSize = 1;
        createSculpture(new Vector3(5, 5, 5), voxelSize / 2);

    }

    private void GenerateCPPN()
    {
        foreach (NodeGene node in geno.GetNodes())
        {
            node.fTYPE = ActivationFunctions.RandomFTYPE();
        }
    }

    public List<Vector3> createSculpture (Vector3 lenghtOfDimensions, float voxelSize)
    {
        float halfVoxelSize = voxelSize / 2;
        cppn = new TWEANN(geno);
        List<Vector3> voxelCoordinates = new List<Vector3>();
        for (int x = 0; x < lenghtOfDimensions[0]; x++)
        {
            for (int y = 0; y < lenghtOfDimensions[1]; y++)
            {
                for (int z = 0; z < lenghtOfDimensions[2]; z++)
                {
                    float actualX = -(halfVoxelSize * lenghtOfDimensions[0] / 2.0f) + halfVoxelSize + x * halfVoxelSize;
                    float actualY = -(halfVoxelSize * lenghtOfDimensions[1] / 2.0f) + halfVoxelSize + y * halfVoxelSize;
                    float actualZ = -(halfVoxelSize * lenghtOfDimensions[2] / 2.0f) + halfVoxelSize + z * halfVoxelSize;
                    float[] outputs = cppn.Process(new float[] { actualX, actualY, actualZ , BIAS});
                    if (outputs[3] > PRESENCE_THRESHOLD) {
                        GameObject voxelProp = Instantiate(voxel) as GameObject;
                        Renderer rend = voxelProp.gameObject.GetComponent<Renderer>();
                        voxelProp.transform.position = new Vector3(actualX, actualY, actualZ);
                        voxelProp.transform.localScale = new Vector3(halfVoxelSize - .0001f, halfVoxelSize - .0001f, halfVoxelSize - .0001f);
                        rend.material.SetColor("_Color", Color.HSVToRGB(outputs[0], outputs[1], outputs[2]));
                    }
                }
            }
        }
        return voxelCoordinates;
    }

}
