using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Sculpture : MonoBehaviour {
    
    public GameObject SculturePlatformObject;
    public MeshFilter meshFilter;
    
    TWEANNGenotype geno;
    TWEANNGenotype backupGeno;
    TWEANN cppn;
    
    Vector3 sculptureDimensions;

    const float PRESENCE_THRESHOLD = .1f;

    const float BIAS = 1f;
    bool transparent;
    public static int THREE_DIMENSIONAL_HUE_INDEX = 0;
    public static int THREE_DIMENSIONAL_SATURATION_INDEX = 1;
    public static int THREE_DIMENSIONAL_BRIGHTNESS_INDEX = 2;
    public static int THREE_DIMENSIONAL_VOXEL_INDEX = 3;
    
    GameObject platform;
    
    bool needsUpdated = false;
    public float RotationSpeed = 50f;

    float MaxValue = float.NegativeInfinity;
    float MinValue = float.PositiveInfinity;

    //Thread
    Thread cppnProcess;
    bool processingCPPN;  
    bool needsRedraw;
    Voxel[,,] voxelMap;
    Vector4[] outArr;

    //Mesh
    private List<Vector3> verticies;
    private List<int> triangles;
    private List<Vector2> uvs;
    private List<Color32> colors;
    private int vertexIndex;

    public bool NeedsRedraw()
    {
        return needsRedraw;
    }

    public bool ProcessingCPPN()
    {
        return processingCPPN;
    }

    public bool GetSelected()
    {
        return false;
    }

    public void SetSelected(bool value)
    {
        //selected = value;
        needsUpdated = true;
    }

    public void SetGeno(TWEANNGenotype geno)
    {
        this.geno = geno;
    }


    private void Start()
    {
        cppnProcess = new Thread(new ThreadStart(PopulateVoxelMapFromCPPN));
        meshFilter = GetComponent<MeshFilter>();

        //inputs: (x,y,z) outputs: r,g,b and presence
        geno = new TWEANNGenotype(8, 4, 0); // Use archetype 0 for test chamber

        platform = Instantiate(SculturePlatformObject) as GameObject;
        platform.transform.position = new Vector3(transform.position.x, transform.position.y - 1.25f, transform.position.z);
        GenerateCPPN();
        cppnProcess.Start();
        //TestPopulateVoxelMap();
        //RedrawSculpture();
    }

    public void Refresh()
    {
        cppnProcess = new Thread(new ThreadStart(PopulateVoxelMapFromCPPN));
        cppnProcess.Start();

    }

    private void Update()
    {

        if(needsRedraw && !processingCPPN)
        {
            RedrawSculpture();
        }

        transform.Rotate(Vector3.up * (RotationSpeed * Time.deltaTime));
    }

    /// <summary>
    /// Fill the sculpture space with voxels
    /// </summary>
    private void BuildSculpture()
    {
        voxelMap = new Voxel[VoxelData.SculptureWidth, VoxelData.SculptureWidth, VoxelData.SculptureHeight];

        for (int y = 0; y < VoxelData.SculptureHeight; y++)
        {
            for(int x = 0; x < VoxelData.SculptureWidth; x++)
            {
                for(int z = 0; z < VoxelData.SculptureWidth; z++)
                {
                    voxelMap[x, z, y] = new Voxel();
                }
            }
        }
    }

    /// <summary>
    /// Create a new sculpture with a new geno
    /// </summary>
    public void NewSculpture()
    {
        NewSculpture(new TWEANNGenotype(8, 4, 0));
    }

    /// <summary>
    /// Create a new sculpture from a geno
    /// </summary>
    /// <param name="geno">TWEANNGenotype</param>
    public void NewSculpture(TWEANNGenotype geno)
    {
        //backupGeno = geno.Copy();
        this.geno = geno;
        cppn = new TWEANN(geno);
        if(cppn.Running)
        {
           // geno = backupGeno;
        }
        GenerateCPPN();
        //PopulateVoxelMapFromCPPN();
        TestPopulateVoxelMap();
    }

    private void TestPopulateVoxelMap()
    {
        voxelMap = new Voxel[VoxelData.SculptureWidth, VoxelData.SculptureHeight, VoxelData.SculptureWidth];

        for (int y = 0; y < VoxelData.SculptureHeight; y++)
        {
            for (int x = 0; x < VoxelData.SculptureWidth; x++)
            {
                for (int z = 0; z < VoxelData.SculptureWidth; z++)
                {
                    voxelMap[x, y, z] = new Voxel
                    {
                        Color = UnityEngine.Random.ColorHSV(),
                        IsPresent = true,
                        IsTransparent = false
                    };
                        
                }
            }
        }
    }

    public void SculptureSize(int x, int z, int y)
    {

    }

    private void GenerateCPPN()
    {
        foreach (NodeGene node in geno.Nodes)
        {
            //node.fTYPE = ActivationFunctions.RandomFTYPE();
            node.fTYPE = ActivationFunctions.RandomFTYPE2();
        }
    }

    public bool ToggleTransparency()
    {
        transparent = !transparent;
        return transparent;
    }

    /// <summary>
    /// Change voxels in sculpture based on CPPN outputs
    /// </summary>
    private void PopulateVoxelMapFromCPPN()
    {

        processingCPPN = true;

        cppn = new TWEANN(geno);

        outArr = new Vector4[VoxelData.SculptureWidth * VoxelData.SculptureHeight * VoxelData.SculptureWidth];

        for (int y = 0; y < VoxelData.SculptureHeight; y++)
        {
            for (int x = 0; x < VoxelData.SculptureWidth; x++)
            {
                for (int z = 0; z < VoxelData.SculptureWidth; z++)
                {
                    float actualX = -(0.5f * VoxelData.SculptureWidth / 2.0f) + 0.5f + x * 0.5f;
                    float actualZ = -(0.5f * VoxelData.SculptureWidth / 2.0f) + 0.5f + z * 0.5f;
                    float actualY = -(0.5f * VoxelData.SculptureHeight / 2.0f) + 0.5f + y * 0.5f;
                    float distFromCenter = GetDistFromCenter(actualX, actualZ, actualY);
                    float distFromCenterXZ = GetDistFromCenterXZ(actualX, actualZ);
                    float distfromCenterYZ = GetDistFromCenterYZ(actualZ, actualY);
                    float distfromCenterXY = GetDistFromCenterXY(actualX, actualY);
                    float[] outputs = cppn.Process(new float[] { actualY, actualX, actualZ, distFromCenter, distFromCenterXZ, distfromCenterYZ, distfromCenterXY, BIAS });

                    outArr[x + (VoxelData.SculptureWidth * z) + (VoxelData.SculptureWidth * VoxelData.SculptureWidth  * y)] = new Vector4(
                        outputs[THREE_DIMENSIONAL_HUE_INDEX], 
                        outputs[THREE_DIMENSIONAL_SATURATION_INDEX], 
                        outputs[THREE_DIMENSIONAL_BRIGHTNESS_INDEX], 
                        outputs[THREE_DIMENSIONAL_VOXEL_INDEX]);
                }
            }
        }

        ColorSculpture(outArr);

        processingCPPN = false;
        needsRedraw = true;


    }

    private void ColorSculpture(Vector4[] outArr)
    {
        voxelMap = new Voxel[VoxelData.SculptureWidth, VoxelData.SculptureHeight, VoxelData.SculptureWidth];

        for (int y = 0; y < VoxelData.SculptureHeight; y++)
        {
            for (int x = 0; x < VoxelData.SculptureWidth; x++)
            {
                for (int z = 0; z < VoxelData.SculptureWidth; z++)
                {
                    voxelMap[x, y, z] = new Voxel();

                    float[] o = FixHue(outArr[x + (VoxelData.SculptureWidth * z) + (VoxelData.SculptureWidth * VoxelData.SculptureWidth * y)]);

                    if (o[THREE_DIMENSIONAL_VOXEL_INDEX] > PRESENCE_THRESHOLD)
                    //if (true)
                    {
                        voxelMap[x, y, z].IsPresent = true;
                        /* 
                           Color colorHSV = Color.HSVToRGB(
                             o[THREE_DIMENSIONAL_HUE_INDEX],
                             o[THREE_DIMENSIONAL_SATURATION_INDEX],
                             o[THREE_DIMENSIONAL_BRIGHTNESS_INDEX],
                             true
                             );
                        */
                        Color32 colorHSV = new Color(o[THREE_DIMENSIONAL_HUE_INDEX], o[THREE_DIMENSIONAL_SATURATION_INDEX], o[THREE_DIMENSIONAL_BRIGHTNESS_INDEX]);
                        float alpha = 1f;
                        //if (transparent)
                        if (false)
                        {
                            alpha = ActivationFunctions.Activation(FTYPE.HLPIECEWISE, o[THREE_DIMENSIONAL_VOXEL_INDEX]);
                        }

                        //float alpha = -1.0f;
                        Color32 color = new Color(colorHSV.r, colorHSV.g, colorHSV.b, alpha);
                        voxelMap[x, y, z].Color = colorHSV;
                    }
                    else
                    {
                        // This option will make the voxel turn off (requires matching  = true statement above)
                        //voxelMap[x, z, y].Color = new Color(0f, 0f, 0f, 0f);
                        voxelMap[x, y, z].IsPresent = false;
                        // This option will enable the "glass block" effect
                        //rend.material.SetColor("_Color", new Color(0f, 0f, 0f, 0f));
                    }
                }
            }
        }
    }

    private float[] FixHue(Vector4 outputFromCPPN)
    {
        float[] result = new float[4];
        float range = MaxValue - MinValue;

        result[THREE_DIMENSIONAL_HUE_INDEX] = ((outputFromCPPN[THREE_DIMENSIONAL_HUE_INDEX] - MinValue) / range);
        //result[TWO_DIMENSIONAL_HUE_INDEX] = Mathf.Abs((ActivationFunctions.Activation(FTYPE.PIECEWISE, hsv[TWO_DIMENSIONAL_HUE_INDEX])));

        result[THREE_DIMENSIONAL_SATURATION_INDEX] = ActivationFunctions.Activation(FTYPE.HLPIECEWISE, outputFromCPPN[THREE_DIMENSIONAL_SATURATION_INDEX]);
        result[THREE_DIMENSIONAL_BRIGHTNESS_INDEX] = Mathf.Abs(ActivationFunctions.Activation(FTYPE.PIECEWISE, outputFromCPPN[THREE_DIMENSIONAL_BRIGHTNESS_INDEX]));

        result[THREE_DIMENSIONAL_VOXEL_INDEX] = outputFromCPPN[THREE_DIMENSIONAL_VOXEL_INDEX];
        return result;
    }

    private void RedrawSculpture()
    {
        needsRedraw = false;

        CreateMeshData();
        CreateMesh();



        //ArtGallery ag = ArtGallery.GetArtGallery();
        //FIXME PROTOTYPE disabling to build new method
        //ag.SaveVox(voxelMap);

    }

    private void CreateMeshData()
    {

        verticies = new List<Vector3>();
        triangles = new List<int>();
        uvs = new List<Vector2>();
        colors = new List<Color32>();
        vertexIndex = 0;

        for (int y = 0; y < VoxelData.SculptureHeight; y++)
        {
            for (int x = 0; x < VoxelData.SculptureWidth; x++)
            {
                for (int z = 0; z < VoxelData.SculptureWidth; z++)
                {
                    AddVoxelToSculpture(new Vector3(x, y, z));
                }
            }
        }
    }

    private void AddVoxelToSculpture(Vector3 pos)
    {
        Color voxelColor = voxelMap[(int)pos.x, (int)pos.y, (int)pos.z].Color;
        
        for (int face = 0; face < 6; face++)
        {
            if (voxelMap[(int)pos.x, (int)pos.y, (int)pos.z].IsPresent && !CheckVoxel(pos + VoxelData.faceChecks[face]))
            {
                for (int c = 0; c < 4; c++)
                {
                    Vector3 scaledPos = new Vector3(
                        Scale((int) (pos.x + VoxelData.voxelVerts[VoxelData.voxelTris[face, c]].x), VoxelData.SculptureWidth),
                        Scale((int) (pos.y + VoxelData.voxelVerts[VoxelData.voxelTris[face, c]].y), VoxelData.SculptureWidth),
                        Scale((int) (pos.z + VoxelData.voxelVerts[VoxelData.voxelTris[face, c]].z), VoxelData.SculptureWidth));
                    verticies.Add(scaledPos);
                    colors.Add(voxelColor);

                }



                triangles.Add(vertexIndex);
                triangles.Add(vertexIndex + 1);
                triangles.Add(vertexIndex + 2);
                triangles.Add(vertexIndex + 2);
                triangles.Add(vertexIndex + 1);
                triangles.Add(vertexIndex + 3);

                vertexIndex += 4;
            }
        }
    }

    private bool CheckVoxel(Vector3 pos)
    {
        int x = Mathf.FloorToInt(pos.x);
        int y = Mathf.FloorToInt(pos.y);
        int z = Mathf.FloorToInt(pos.z);

        if (x < 0 || x >= VoxelData.SculptureWidth || y < 0 || y >= VoxelData.SculptureHeight || z < 0 || z >= VoxelData.SculptureWidth)
            return false;
        else
            return voxelMap[x, y, z].IsPresent;
            //return true;
    }

    private void CreateMesh()
    {
        Mesh mesh = new Mesh
        {
            vertices = verticies.ToArray(),
            triangles = triangles.ToArray(),
            uv = uvs.ToArray(),
            colors32 = colors.ToArray()
        };

        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }

    /// <summary>
    /// Mutate genome
    /// </summary>
    public void Mutate()
    {
        geno.Mutate();
    }

    public TWEANNGenotype GetGenotype()
    {
        return geno;
    }

    float GetDistFromCenter(float x, float z, float y)
    {
        float result = float.NaN;

        result = Mathf.Sqrt((x * x + z * z + y * y)) * Mathf.Sqrt(2);

        return result;
    }

    float GetDistFromCenterXY(float x, float y)
    {
        float result = float.NaN;

        result = Mathf.Sqrt((x * x + y * y)) * Mathf.Sqrt(2);

        return result;
    }

    float GetDistFromCenterYZ(float z, float y)
    {
        float result = float.NaN;

        result = Mathf.Sqrt((z * z + y * y)) * Mathf.Sqrt(2);

        return result;
    }

    float GetDistFromCenterXZ(float x, float z)
    {
        float result = float.NaN;

        result = Mathf.Sqrt((x * x + z * z)) * Mathf.Sqrt(2);

        return result;
    }

    float Scale(int toScale, int maxDimension)
    {
        float result;

        result = ((toScale * 1.0f / (maxDimension - 1)) * 2) - 1;

        return result;
    }
}
