using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Sculpture : IProcessable {
    
    public GameObject SculturePlatformObject;
    public MeshFilter meshFilter;

    public bool NeedsRedraw { get; private set; }
    public bool IsInitialized { get; private set; }
    public GeneticArt Art { get; set; }
    public CoordinateSpace SpatialInputLimits { get; private set; }


    private Mesh mesh;
    private IColorChange colorChanger;
    private float[][] cppnOutput;

    //Config
    const float PRESENCE_THRESHOLD = .1f;
    const float BIAS = 1f;
    bool transparent;

    //Vox
    private Voxel[,,] voxelMap;
 
    //Mesh
    private List<Vector3> verticies;
    private List<int> triangles;
    private List<Vector2> uvs;
    private List<Color32> colors;
    private int vertexIndex;

    public Sculpture(GeneticArt art) 
    {
        IsInitialized = false;
        Art = art;
        Init();
    }

    private void Init()
    {
        SpatialInputLimits = new CoordinateSpace(VoxelData.SculptureWidth, VoxelData.SculptureHeight, VoxelData.SculptureWidth);
        colorChanger = new ColorSpaceStandardRGB();
        BuildSculpture();
        //TestPopulateVoxelMap();

        UpdateCPPNArt();
        //RedrawSculpture();
        IsInitialized = true;
    }

    public void UpdateCPPNArt()
    {
        //cppnProcess = new Thread(new ThreadStart(PopulateVoxelMapFromCPPN));
        //cppnProcess.Start();
        cppnOutput = Process();
        ApplyToVoxelMap();

        RedrawSculpture();

    }

    private void ApplyToVoxelMap()
    {
        Color32[] processedColor = colorChanger.AdjustColor(cppnOutput);
        for (int y = 0; y < VoxelData.SculptureHeight; y++)
        {
            for (int x = 0; x < VoxelData.SculptureWidth; x++)
            {
                for (int z = 0; z < VoxelData.SculptureWidth; z++)
                {
                    int index = x + (VoxelData.SculptureWidth * z) + (VoxelData.SculptureWidth * VoxelData.SculptureWidth * y);
                    if (cppnOutput[index][3] > PRESENCE_THRESHOLD)
                    {
                        voxelMap[x, y, z].Color = processedColor[index];
                        voxelMap[x, y, z].IsPresent = true;
                    }

                }
            }
        }
    }

    /// <summary>
    /// Fill the sculpture space with voxels
    /// </summary>
    private void BuildSculpture()
    {
        voxelMap = new Voxel[VoxelData.SculptureWidth, VoxelData.SculptureHeight, VoxelData.SculptureWidth];

        for (int y = 0; y < VoxelData.SculptureHeight; y++)
        {
            for(int x = 0; x < VoxelData.SculptureWidth; x++)
            {
                for(int z = 0; z < VoxelData.SculptureWidth; z++)
                {
                    voxelMap[x, y, z] = new Voxel();
                }
            }
        }
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

    public bool ToggleTransparency()
    {
        transparent = !transparent;
        return transparent;
    }

    /// <summary>
    /// Change voxels in sculpture based on CPPN outputs
    /// </summary>
    private float[][] Process()
    {
        TWEANN cppn = new TWEANN(Art.GetGenotype());
        float[][] hsvArr = new float[VoxelData.SculptureWidth * VoxelData.SculptureHeight * VoxelData.SculptureWidth][];

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

                    hsvArr[x + (VoxelData.SculptureWidth * z) + (VoxelData.SculptureWidth * VoxelData.SculptureWidth * y)] = cppn.Process(new float[] { 
                        actualY, 
                        actualX, 
                        actualZ, 
                        distFromCenter, 
                        distFromCenterXZ, 
                        distfromCenterYZ, 
                        distfromCenterXY, 
                        BIAS 
                    });
                }
            }
        }

        //NeedsRedraw = true;
        return hsvArr;
    }

    private void RedrawSculpture()
    {
        NeedsRedraw = false;

        CreateMeshData();
        CreateMesh();
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
        mesh = new Mesh
        {
            vertices = verticies.ToArray(),
            triangles = triangles.ToArray(),
            uv = uvs.ToArray(),
            colors32 = colors.ToArray()
        };

        mesh.RecalculateNormals();

        //meshFilter.mesh = mesh;
        NeedsRedraw = true;
    }

    public Mesh GetMesh()
    {
        return mesh;
    }

    /* Utils */
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
