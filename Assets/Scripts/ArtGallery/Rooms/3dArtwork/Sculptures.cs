using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sculptures {
    TWEANNGenotype geno;
    TWEANN cppn;
    float presentThreshold = 0.2f;
    int NUM_VOXELS = 729;
    GameObject sculpture;
    Voxel[] voxels;
    Vector3 size; //must be int

    public class Voxel : MonoBehaviour
    {
        bool present;
        Color color;
        Vector3 positionInSculture; //the (x,y,z) coordinates of this voxel relative to the sculpture, int. 

        public Voxel(float presentValue, float redColor, float greenValue, float blueValue, Vector3 positionInSculture)
        {
            this.present = present;
            this.color = color;
            this.positionInSculture = positionInSculture;
        }

        void setColor(float r, float g, float b)
        {
            color.r = r;
            color.g = g;
            color.b = b;
        }

        void isPresent(float presentValue)
        {
            present = presentThreshold < presentValue;
        }
    }

    Sculptures() : this(new TWEANNGenotype(5, 4, 0))
    {

    }

    Sculptures(TWEANNGenotype geno)
    {
        this.geno = geno;
        sculpture = new GameObject();
    }

    public Voxel[] GenerateSculptureFromCPPN ()
    {
        //should this be here?
        cppn = new TWEANN(geno);
        
        for (int x = 0; x < size[0]; x++)
        {
            for (int y = 0; y < size[1]; y++)
            {
                for (int z = 0; z < size[2]; z++)
                {
                    float scaledX = Scale(x, (int)size[0]);
                    float scaledY = Scale(y, (int)size[1]);
                    float scaledZ = Scale(z, (int)size[2]);
                    float[] voxelCharacteristic = cppn.Process(new float[] { scaledX, scaledY, scaledZ, GetDistFromCenter(scaledX, scaledY, scaledZ), 1});
                    voxels[x * (int)size[1] + y * (int)size[2] + z] = new Voxel(voxelCharacteristic[0], voxelCharacteristic[1], 
                        voxelCharacteristic[2], voxelCharacteristic[3], new Vector3 ( x, y, z ));
                }
            }
        }
        return voxels;
    }

    //duplicate code
    float Scale(int toScale, int maxDimension)
    {
        float result;

        result = ((toScale * 1.0f / (maxDimension - 1)) * 2) - 1;

        return result;
    }

    float GetDistFromCenter(float x, float y, float z)
    {
        return Mathf.Sqrt((x - 0.5f) * (x - 0.5f) + (y - 0.5f) * (y - 0.5f) + (z - 0.5f) * (z - 0.5f));
    }
    //translate

    //rotate

    //scale
}
