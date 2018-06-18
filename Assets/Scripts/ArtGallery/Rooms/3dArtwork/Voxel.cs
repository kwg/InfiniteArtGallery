using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel : MonoBehaviour
{
    public Color color;
    public Vector3 positionInSculture; //the (x,y,z) coordinates of this voxel relative to the sculpture, int. 

    Vector4
    public Voxel(Color color, Vector3 positionInSculture)
    {
        this.color = color;
        this.positionInSculture = positionInSculture;
    }

}