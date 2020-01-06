using UnityEngine;

public class Process2D : IGenoProcess
{
    TWEANN cppn;
    const float BIAS = 1f;

    float Zoom = 5f;


    public float[][] Process(TWEANNGenotype _geno, int[] _spatialInputLimits)
    {
        cppn = new TWEANN(_geno);
        int width = _spatialInputLimits[0];
        int height = _spatialInputLimits[1];

        float[][] hsvArr = new float[width * height][];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float scaledX = Scale(x, width);
                float scaledY = Scale(y, height);
                float distCenter = GetDistFromCenter(scaledX, scaledY);

                hsvArr[x + (y * width)] = ProcessCPPNInput(scaledX, scaledY, distCenter, BIAS);
            }
        }

        return hsvArr;
    }

    float Scale(int toScale, int maxDimension)
    {
        float result;

        result = (((toScale * 1f / (maxDimension)) * 2) - 1) * Zoom;

        return result;
    }

    float GetDistFromCenter(float x, float y)
    {
        float result = float.NaN;

        result = Mathf.Sqrt((x * x + y * y)) * Mathf.Sqrt(2);

        return result;
    }

    private float[] ProcessCPPNInput(float scaledX, float scaledY, float distCenter, float bias)
    {
        //HACK FIXME scaledZ and sculpture distances hard coded to 0 - maybe combine all network processing to a utility function that figures all of that out
        return cppn.Process(new float[] { scaledX, scaledY, 0, distCenter, 0, 0, 0, bias });
    }
}
