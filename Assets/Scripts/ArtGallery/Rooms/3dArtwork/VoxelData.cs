using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VoxelData
{
    public static readonly int SculptureWidth = 16;
    public static readonly int SculptureHeight = 32;

    public static readonly Vector3[] voxelVerts = new Vector3[8] {
        new Vector3(0.0f, 0.0f, 0.0f), // 0
        new Vector3(1.0f, 0.0f, 0.0f), // 1
        new Vector3(1.0f, 1.0f, 0.0f), // 2
        new Vector3(0.0f, 1.0f, 0.0f), // 3
        new Vector3(0.0f, 0.0f, 1.0f), // 4
        new Vector3(1.0f, 0.0f, 1.0f), // 5
        new Vector3(1.0f, 1.0f, 1.0f), // 6
        new Vector3(0.0f, 1.0f, 1.0f)  // 7
    };

    public static readonly Vector3[] faceChecks = new Vector3[6]
    {
        new Vector3(0.0f, 0.0f, -1.0f),
        new Vector3(0.0f, 0.0f, 1.0f),
        new Vector3(0.0f, 1.0f, 0.0f),
        new Vector3(0.0f, -1.0f, 0.0f),
        new Vector3(-1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f)
    };

    public static readonly int[,] voxelTris = new int[6, 4]
    {
        // 0, 1, 2, 2, 1, 3
        { 0, 3, 1, 2}, // back face
        { 5, 6, 4, 7}, // front face
        { 3, 7, 2, 6}, // top face
        { 1, 5, 0, 4}, // bottom face
        { 4, 7, 0, 3}, // left face
        { 1, 2, 5, 6}  // right face

    };

    public static readonly Vector2[] voxelUvs = new Vector2[4] {

        new Vector2(0.0f, 0.0f),
        new Vector2(0.0f, 1.0f),
        new Vector2(1.0f, 0.0f),
        new Vector2(1.0f, 1.0f)

    };


}
