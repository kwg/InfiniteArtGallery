using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Sculpture : GeneticArt {
    
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

    private const int STANDARD_RGB = 0; 
    private const int STANDARD_HSV = 1;
    private const int MINMAXED_RGB = 2;
    private const int MINMAXED_HSV = 3;
    private const int DIRECT_RGB = 4;
    private const int DIRECT_HSV = 5;


    GameObject platform;
    
    bool needsUpdated = false;
    public float RotationSpeed = 50f;

    float MaxHueValue = float.NegativeInfinity;
    float MinHueValue = float.PositiveInfinity;
    float MaxSaturationValue = float.NegativeInfinity;
    float MinSaturationValue = float.PositiveInfinity;
    float MaxBrightnessValue = float.NegativeInfinity;
    float MinBrightnessValue = float.PositiveInfinity;

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

    public Sculpture() : base(new TWEANNGenotype(8, 4, 0), new int[] { 5, 5, 5 }, null)
    {

    }

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


    //private void Start()
    //{
    //    cppnProcess = new Thread(new ThreadStart(PopulateVoxelMapFromCPPN));
    //    meshFilter = GetComponent<MeshFilter>();

    //    //inputs: (x,y,z) outputs: r,g,b and presence
    //    geno = new TWEANNGenotype(8, 4, 0); // Use archetype 0 for test chamber

    //    platform = Instantiate(SculturePlatformObject) as GameObject;
    //    platform.transform.position = new Vector3(transform.position.x, transform.position.y - 1.25f, transform.position.z);
    //    GenerateCPPN();
    //    cppnProcess.Start();
    //    //TestPopulateVoxelMap();
    //    //RedrawSculpture();
    //}

    public void Refresh()
    {
        cppnProcess = new Thread(new ThreadStart(PopulateVoxelMapFromCPPN));
        cppnProcess.Start();

    }

    //private void Update()
    //{

    //    if(needsRedraw && !processingCPPN)
    //    {
    //        RedrawSculpture();
    //    }

    //    transform.Rotate(Vector3.up * (RotationSpeed * Time.deltaTime));
    //}

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
                    
                    float[] outputs = cppn.Process(new float[] { 
                        actualY, 
                        actualX, 
                        actualZ, 
                        distFromCenter, 
                        distFromCenterXZ, 
                        distfromCenterYZ, 
                        distfromCenterXY, 
                        BIAS 
                    });

                    MinMax(new Vector4(outputs[0], outputs[1], outputs[2], outputs[3]));

                    outArr[x + (VoxelData.SculptureWidth * z) + (VoxelData.SculptureWidth * VoxelData.SculptureWidth * y)] = new Vector4(
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

    void MinMax(Vector4 output)
    {
        if (output.x < MinHueValue)
            MinHueValue = output.x;
        else if (output.x > MaxHueValue)
            MaxHueValue = output.x;
        if (output.y < MinSaturationValue)
            MinSaturationValue = output.y;
        else if (output.y > MaxSaturationValue)
            MinSaturationValue = output.y; 
        if (output.z < MinBrightnessValue)
            MinBrightnessValue = output.z;
        else if (output.z > MaxBrightnessValue)
            MinBrightnessValue = output.z;
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

                    Vector4 output = outArr[x + (VoxelData.SculptureWidth * z) + (VoxelData.SculptureWidth * VoxelData.SculptureWidth * y)];
                    Color32 color = ColorAdjustment(output, STANDARD_RGB);

                    if (output.w > PRESENCE_THRESHOLD)
                    {
                        voxelMap[x, y, z].IsPresent = true;
                        voxelMap[x, y, z].Color = color;
                    }
                    else
                    { 
                        voxelMap[x, y, z].IsPresent = false;
                    }
                }
            }
        }
    }

    private Color32 ColorAdjustment(Vector4 output, int selection)
    {
        switch (selection)
        {
            case STANDARD_RGB:
                return FixHueSTANDARD_RGB(output);
            case STANDARD_HSV:
                return FixHueSTANDARD_HSV(output);
            case MINMAXED_RGB:
                return FixHueMINMAXED_RGB(output);
            case MINMAXED_HSV:
                return FixHueMINMAXED_HSV(output);
            case DIRECT_RGB:
                return FixHueDIRECT_RGB(output);
            case DIRECT_HSV:
                return FixHueDIRECT_HSV(output);
            default:
                return new Color32();
        }
    }

    private Color32 FixHueSTANDARD_RGB(Vector4 output)
    {
        Color32 result =  new Color(
        output[THREE_DIMENSIONAL_HUE_INDEX],
        ActivationFunctions.Activation(FTYPE.HLPIECEWISE, output[THREE_DIMENSIONAL_SATURATION_INDEX]),
        Mathf.Abs(ActivationFunctions.Activation(FTYPE.PIECEWISE, output[THREE_DIMENSIONAL_BRIGHTNESS_INDEX])));

        return result;
    }



    private Color32 FixHueSTANDARD_HSV(Vector4 output)
    {
        Color32 result = Color.HSVToRGB(
            Mathf.Abs(ActivationFunctions.Activation(FTYPE.PIECEWISE, output[THREE_DIMENSIONAL_HUE_INDEX])),
            Mathf.Abs(ActivationFunctions.Activation(FTYPE.HLPIECEWISE, output[THREE_DIMENSIONAL_SATURATION_INDEX])),
            Mathf.Abs(ActivationFunctions.Activation(FTYPE.PIECEWISE, output[THREE_DIMENSIONAL_BRIGHTNESS_INDEX])),
            false
        );

        return result;
    }

    private Color32 FixHueMINMAXED_RGB(Vector4 output)
    {
        float range = MaxHueValue - MinHueValue;

        Color32 result = new Color(
        (output[THREE_DIMENSIONAL_HUE_INDEX] - MinHueValue) / range,
        ActivationFunctions.Activation(FTYPE.HLPIECEWISE, output[THREE_DIMENSIONAL_SATURATION_INDEX]),
        Mathf.Abs(ActivationFunctions.Activation(FTYPE.PIECEWISE, output[THREE_DIMENSIONAL_BRIGHTNESS_INDEX])));

        return result;
    }

    private Color32 FixHueMINMAXED_HSV(Vector4 output)
    {
        float range = MaxHueValue - MinHueValue;

        Color32 result = Color.HSVToRGB(
            Mathf.Abs((ActivationFunctions.Activation(FTYPE.PIECEWISE, (output[THREE_DIMENSIONAL_HUE_INDEX] - MinHueValue)) / range)),
            ActivationFunctions.Activation(FTYPE.HLPIECEWISE, output[THREE_DIMENSIONAL_SATURATION_INDEX]),
            Mathf.Abs(ActivationFunctions.Activation(FTYPE.PIECEWISE, output[THREE_DIMENSIONAL_BRIGHTNESS_INDEX])));
        return result;
    }

    private Color32 FixHueDIRECT_RGB(Vector4 output)
    {
        float range = MaxHueValue - MinHueValue;

        Color32 result = Color.HSVToRGB(
            (output[THREE_DIMENSIONAL_HUE_INDEX] - MinHueValue) / range,
            ActivationFunctions.Activation(FTYPE.HLPIECEWISE, output[THREE_DIMENSIONAL_SATURATION_INDEX]),
            Mathf.Abs(ActivationFunctions.Activation(FTYPE.PIECEWISE, output[THREE_DIMENSIONAL_BRIGHTNESS_INDEX])));

        return result;
    }

    private Color32 FixHueDIRECT_HSV(Vector4 output)
    {
        float range = MaxHueValue - MinHueValue;

        Color32 result = Color.HSVToRGB(
            (output[THREE_DIMENSIONAL_HUE_INDEX] - MinHueValue) / range,
            output[THREE_DIMENSIONAL_SATURATION_INDEX],
            output[THREE_DIMENSIONAL_BRIGHTNESS_INDEX]);

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
                    Vector3 offset = VoxelData.voxelVerts[VoxelData.voxelTris[face, c]];
                    int widthScale = VoxelData.SculptureWidth;
                    float adjustment = 1f / widthScale;

                    Vector3 scaledPos = new Vector3(
                        adjustment * (pos.x + offset.x - (0.5f * widthScale) - adjustment),
                        (adjustment) * (pos.y + offset.y - (0.5f * widthScale)),
                        adjustment * (pos.z + offset.z - (0.5f * widthScale) - adjustment));
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
        else if (voxelMap[x, y, z].IsTransparent)
            return false;
        else
            return voxelMap[x, y, z].IsPresent;
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
