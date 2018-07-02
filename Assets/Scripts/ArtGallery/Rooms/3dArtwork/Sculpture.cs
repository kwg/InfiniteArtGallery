using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sculpture : MonoBehaviour {
    public GameObject VoxelObject;
    TWEANNGenotype geno;
    TWEANN cppn;
    Vector3 sculptureDimensions;
    float voxelSize;
    const float PRESENCE_THRESHOLD = .1f;
    const int SCULP_X = 5;
    const int SCULP_Y = 10;
    const int SCULP_Z = 5;
    const float BIAS = 1;
    GameObject[,,] vox;

    private void Start()
    {
        //inputs: (x,y,z) outputs: r,g,b and presence
        geno = new TWEANNGenotype(4, 4, 0); // FIXME archetype index 
        vox = new GameObject[SCULP_X, SCULP_Z, SCULP_Y];


        sculptureDimensions = new Vector3(5, 5, 5);
        transform.SetPositionAndRotation(new Vector3((SCULP_X * voxelSize) / 2f, (SCULP_Y * voxelSize) / 2f, (SCULP_Z * voxelSize) / 2f), Quaternion.identity);

        voxelSize = 1;
        ActivationFunctions.ActivateAllFunctions();
        GenerateCPPN();
        PregenSculpture();
        CreateSculture();
    }

    private void PregenSculpture()
    {
        float halfVoxelSize = voxelSize / 2;

        for (int x = 0; x < SCULP_X; x++)
        {
            for(int z = 0; z < SCULP_Z; z++)
            {
                for(int y = 0; y < SCULP_Y; y++)
                {
                    GameObject voxel = Instantiate(VoxelObject) as GameObject;
                    // set this vox position
                    float actualX = transform.position.x + (-(halfVoxelSize * SCULP_X / 2.0f) + halfVoxelSize + x * halfVoxelSize);
                    float actualY = transform.position.y + (-(halfVoxelSize * SCULP_Z / 2.0f) + halfVoxelSize + y * halfVoxelSize);
                    float actualZ = transform.position.z + (-(halfVoxelSize * SCULP_Y / 2.0f) + halfVoxelSize + z * halfVoxelSize);
                    voxel.transform.SetParent(transform);
                    voxel.transform.position = new Vector3(actualX, actualY, actualZ);
                    voxel.transform.localScale = new Vector3(halfVoxelSize - .0001f, halfVoxelSize - .0001f, halfVoxelSize - .0001f);

                    vox[x, z, y] = voxel;
                }
            }
        }
    }

    public void NewSculpture()
    {
        geno = new TWEANNGenotype(4, 4, 0); // FIXME archetype index 
        GenerateCPPN();
        CreateSculture();

    }

    private void GenerateCPPN()
    {
        foreach (NodeGene node in geno.Nodes)
        {
            node.fTYPE = ActivationFunctions.RandomFTYPE();
        }
    }

    public void CreateSculture ()
    {
        float halfVoxelSize = voxelSize / 2;
        cppn = new TWEANN(geno);
        for (int x = 0; x < SCULP_X; x++)
        {
            for (int z = 0; z < SCULP_Z; z++)
            {
                for (int y = 0; y < SCULP_Y; y++)
                {
                    GameObject voxelProp = vox[x, z, y];
                    Renderer rend = voxelProp.gameObject.GetComponent<Renderer>();
                    float actualX = -(halfVoxelSize * SCULP_X / 2.0f) + halfVoxelSize + x * halfVoxelSize;
                    float actualZ = -(halfVoxelSize * SCULP_Z / 2.0f) + halfVoxelSize + y * halfVoxelSize;
                    float actualY = -(halfVoxelSize * SCULP_Y / 2.0f) + halfVoxelSize + z * halfVoxelSize;
                    float[] outputs = cppn.Process(new float[] { actualX, actualY, actualZ , BIAS});
                    if (outputs[3] > PRESENCE_THRESHOLD) {
                        float initialHue = ActivationFunctions.Activation(FTYPE.PIECEWISE, outputs[0]);
                        float finalHue = initialHue < 0 ? initialHue + 1 : initialHue;
                        Color colorHSV = Color.HSVToRGB(
                            finalHue,
                            ActivationFunctions.Activation(FTYPE.HLPIECEWISE, outputs[1]),
                            Mathf.Abs(ActivationFunctions.Activation(FTYPE.PIECEWISE, outputs[2])),
                            true
                            );
                        rend.enabled = true;
                        Color color = new Color(colorHSV.r, colorHSV.g, colorHSV.b, outputs[3]);
                        rend.material.SetColor("_Color", color);
                    }
                    else
                    {
                        // This option will make the voxel turn off (requires matching  = true statement above)
                        rend.enabled = false; 
                        // This option will enable the "glass block" effect
                        rend.material.SetColor("_Color", new Color(0f, 0f, 0f, 0f));
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
