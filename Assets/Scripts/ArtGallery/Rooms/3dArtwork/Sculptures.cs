using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sculptures : MonoBehaviour {
    public GameObject voxel;
    TWEANNGenotype geno;
    TWEANN cppn;
    Vector3 sculptureDimensions;
    float voxelSize;
    const float PRESENCE_THRESHOLD = .1f;
    const float BIAS = 1;
    ArrayList voxelList;

    private void Start()
    {
        //inputs: (x,y,z) outputs: r,g,b and presence
        geno = new TWEANNGenotype(4, 4, 0); // FIXME archetype index 
        sculptureDimensions = new Vector3(5, 5, 5);
        voxelSize = 1;
        ActivationFunctions.ActivateAllFunctions();
        GenerateCPPN();
        createSculpture();

    }

    private void GenerateCPPN()
    {
        foreach (NodeGene node in geno.Nodes)
        {
            node.fTYPE = ActivationFunctions.RandomFTYPE();
        }
    }

    public void createSculpture ()
    {
        float halfVoxelSize = voxelSize / 2;
        cppn = new TWEANN(geno);
        for (int x = 0; x < sculptureDimensions[0]; x++)
        {
            for (int y = 0; y < sculptureDimensions[1]; y++)
            {
                for (int z = 0; z < sculptureDimensions[2]; z++)
                {
                    float actualX = -(halfVoxelSize * sculptureDimensions[0] / 2.0f) + halfVoxelSize + x * halfVoxelSize;
                    float actualY = -(halfVoxelSize * sculptureDimensions[1] / 2.0f) + halfVoxelSize + y * halfVoxelSize;
                    float actualZ = -(halfVoxelSize * sculptureDimensions[2] / 2.0f) + halfVoxelSize + z * halfVoxelSize;
                    float[] outputs = cppn.Process(new float[] { actualX, actualY, actualZ , BIAS});
                    if (outputs[3] > PRESENCE_THRESHOLD) {
                        GameObject voxelProp = Instantiate(voxel) as GameObject;
                        voxelProp.transform.parent = this.transform;
                        Renderer rend = voxelProp.gameObject.GetComponent<Renderer>();
                        voxelProp.transform.position = new Vector3(actualX, actualY, actualZ);
                        voxelProp.transform.localScale = new Vector3(halfVoxelSize - .0001f, halfVoxelSize - .0001f, halfVoxelSize - .0001f);
                        rend.material.SetColor("_Color", Color.HSVToRGB(outputs[0], outputs[1], outputs[2]));
                        //set props name so that we can identify it
                        voxelProp.name = "voxel";
                    }
                }
            }
        }
    }

    public void Mutate()
    {
        geno.Mutate();
    }

}
