using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sculptures {
    TWEANNGenotype geno;
    TWEANN cppn;
    float presentThreshold = 0.2f;
    int NUM_VOXELS = 729;
    Voxel[] voxels;

    public class Voxel : MonoBehaviour
    {
        bool present;
        Color color;
  
        Vector3 size;
        Vector3 positionInSculture; //the (x,y,z) coordinates of this voxel relative to the sculpture, int. 

        void setColor(float r, float g, float b)
        {
            color.r = r;
            color.g = g;
            color.b = b;
        }

        void isPresent(float presentValue)
        {
            present = presentThreshold.CompareTo(presentValue) > 0 ? true: false;
        }
    }

    Sculptures() : this(new TWEANNGenotype(3, 4, 0))
    {

    }

    Sculptures(TWEANNGenotype geno)
    {
        this.geno = geno;
        voxels = new Voxel[NUM_VOXELS];
    }

    /*
     * The direction and distance to translate Voxel from its current position, (x,y,z)
     */
    void Translate(Vector3 movementVector)
    {
        position[0] += movementVector[0];
        position[1] += movementVector[1];
        position[2] += movementVector[2];
    }

    Rotate(Vector3 eulerAngles)


        //scale
}
