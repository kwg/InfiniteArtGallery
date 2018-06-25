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

    public List<Vector3> createSculpture (Vector3 lengthOfDimensions, float voxelSize)
    {
        float halfVoxelSize = voxelSize / 2;
        cppn = new TWEANN(geno);
        List<Vector3> voxelCoordinates = new List<Vector3>();
        for (int x = 0; x < lengthOfDimensions[0]; x++)
        {
            for (int y = 0; y < lengthOfDimensions[1]; y++)
            {
                for (int z = 0; z < lengthOfDimensions[2]; z++)
                {
                    float actualX = -(halfVoxelSize * lengthOfDimensions[0] / 2.0f) + halfVoxelSize + x * halfVoxelSize;
                    float actualY = -(halfVoxelSize * lengthOfDimensions[1] / 2.0f) + halfVoxelSize + y * halfVoxelSize;
                    float actualZ = -(halfVoxelSize * lengthOfDimensions[2] / 2.0f) + halfVoxelSize + z * halfVoxelSize;
                    float[] outputs = cppn.Process(new float[] { actualX, actualY, actualZ , BIAS});
                    if (outputs[3] > PRESENCE_THRESHOLD) {
                        GameObject voxelProp = Instantiate(voxel) as GameObject;
                        voxelProp.transform.parent = this.transform;
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

    public void Mutate()
    {
        geno.Mutate();
    }

}
