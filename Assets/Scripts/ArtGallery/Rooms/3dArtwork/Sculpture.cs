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
    const int SCULP_Z = 5;
    const int SCULP_Y = 10;
    const float BIAS = 1f;
    const float NUDGE = 0f;
    public static int THREE_DIMENSIONAL_VOXEL_INDEX = 0;
    public static int THREE_DIMENSIONAL_HUE_INDEX = 1;
    public static int THREE_DIMENSIONAL_SATURATION_INDEX = 2;
    public static int THREE_DIMENSIONAL_BRIGHTNESS_INDEX = 3;
    GameObject[,,] vox;

    private void Start()
    {
        //inputs: (x,y,z) outputs: r,g,b and presence
        geno = new TWEANNGenotype(5, 4, 0); // Use archetype 0 for test chamber
        vox = new GameObject[SCULP_X, SCULP_Z, SCULP_Y];

        voxelSize = 0.5f;
        ActivationFunctions.ActivateAllFunctions();
        PregenSculpture();
        GenerateCPPN();
        CreateSculture();
    }

    /// <summary>
    /// Fill the sculpture space with voxels
    /// </summary>
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
                    float actualY = transform.position.z + (-(halfVoxelSize * SCULP_Z / 2.0f) + halfVoxelSize + z * halfVoxelSize);
                    float actualZ = transform.position.y + (-(halfVoxelSize * SCULP_Y / 2.0f) + halfVoxelSize + y * halfVoxelSize);
                    voxel.transform.SetParent(transform);
                    voxel.transform.position = new Vector3(actualX, actualZ, actualY);
                    voxel.transform.localScale = new Vector3(halfVoxelSize - NUDGE, halfVoxelSize - NUDGE, halfVoxelSize - NUDGE);

                    vox[x, z, y] = voxel;
                }
            }
        }
    }

    /// <summary>
    /// Reset the sculpture
    /// </summary>
    public void NewSculpture()
    {
        geno = new TWEANNGenotype(4, 4, 0); 
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

    /// <summary>
    /// Change voxels in sculpture based on CPPN outputs
    /// </summary>
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
                    float actualZ = -(halfVoxelSize * SCULP_Z / 2.0f) + halfVoxelSize + z * halfVoxelSize;
                    float actualY = -(halfVoxelSize * SCULP_Y / 2.0f) + halfVoxelSize + y * halfVoxelSize;
                    float distFromCenter = GetDistFromCenter(actualX, actualZ, actualY);
                    float[] outputs = cppn.Process(new float[] { actualX, actualY, actualZ, distFromCenter, BIAS});
                    if (outputs[THREE_DIMENSIONAL_BRIGHTNESS_INDEX] > PRESENCE_THRESHOLD) {
                        float initialHue = ActivationFunctions.Activation(FTYPE.PIECEWISE, outputs[THREE_DIMENSIONAL_VOXEL_INDEX]);
                        float finalHue = initialHue < 0 ? initialHue + 1 : initialHue;
                        Color colorHSV = Color.HSVToRGB(
                            finalHue,
                            ActivationFunctions.Activation(FTYPE.HLPIECEWISE, outputs[THREE_DIMENSIONAL_HUE_INDEX]),
                            Mathf.Abs(ActivationFunctions.Activation(FTYPE.PIECEWISE, outputs[THREE_DIMENSIONAL_SATURATION_INDEX])),
                            true
                            );
                        rend.enabled = true;
                        float alpha = ActivationFunctions.Activation(FTYPE.HLPIECEWISE, outputs[THREE_DIMENSIONAL_BRIGHTNESS_INDEX]);
                        //float alpha = -1.0f;
                        Color color = new Color(colorHSV.r, colorHSV.g, colorHSV.b, alpha);
                        rend.material.SetColor("_Color", color);
                    }
                    else
                    {
                        // This option will make the voxel turn off (requires matching  = true statement above)
                        rend.enabled = false; 
                        // This option will enable the "glass block" effect
                        //rend.material.SetColor("_Color", new Color(0f, 0f, 0f, 0f));
                    }
                }
            }
        }
    }

    /// <summary>
    /// Mutate genome
    /// </summary>
    public void Mutate()
    {
        geno.Mutate();
    }

    /// <summary>
    /// Tell sculpture what object we are using as a voxel
    /// </summary>
    /// <param name="Voxel">Unity prefab we are using as a voxel</param>
    public void LoadVoxel(GameObject Voxel)
    {
        VoxelObject = Voxel;
    }

    float GetDistFromCenter(float x, float z, float y)
    {
        float result = float.NaN;

        result = Mathf.Sqrt((x * x + z * z + y * y)) * Mathf.Sqrt(2);

        return result;
    }

    float Scale(int toScale, int maxDimension)
    {
        float result;

        result = ((toScale * 1.0f / (maxDimension - 1)) * 2) - 1;

        return result;
    }
}
