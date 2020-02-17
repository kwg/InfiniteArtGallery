using System;
using System.Threading;
using UnityEngine;

public class Artwork : IProcessable
{

    public bool NeedsRedraw { get; private set; }
    public bool IsInitialized { get; private set; }
    public GeneticArt Art { get; set; }
    public CoordinateSpace SpatialInputLimits { get; private set; }


    private Texture2D texture;
    private IColorChange colorChanger;
    private float[][] cppnOutput;

    private Thread thread;
    private bool threaded = true;

    //ArtGallery ag;

    //FIXME PROTOTYPE width, height - These are static for testing but we may want to make them change
    private static int width = 128;
    private static int height = 128;
    private static float BIAS = 1f;
    private static float ZOOM = 5f;

    public Texture2D GetTexture()
    {
        texture = new Texture2D(width, height, TextureFormat.ARGB32, false);

        texture.SetPixels32(colorChanger.AdjustColor(cppnOutput));
        texture.Apply();
        NeedsRedraw = false;
        return texture;
    }

    [Obsolete("This should only be used for testing purposes.")]
    /// <summary>
    /// Create a new artwork in a room with a new genetic art.
    /// </summary>
    public Artwork() : this(new GeneticArt()) { }

    /// <summary>
    /// Create a new artwork in a room with a given genotype
    /// </summary>
    public Artwork(GeneticArt Art) 
    {
        this.Art = Art;
        IsInitialized = false;

        Init();
    }

    private void Init()
    {
        NeedsRedraw = false;
        SpatialInputLimits = new CoordinateSpace(width, height);
        texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        colorChanger = new ColorSpaceStandardRGB();
        UpdateCPPNArt();

        IsInitialized = true;
    }

    public void UpdateCPPNArt()
    {
        if (threaded)
        {
            thread = new Thread(new ThreadStart(CPPNProcess));
            thread.Start();
        }
        else
        {
            CPPNProcess();
        }
    }

    private void CPPNProcess()
    {
            cppnOutput = Process();
            NeedsRedraw = true;

    }

    private float[][] Process()
    {
        TWEANN cppn = new TWEANN(Art.GetGenotype());

        float[][] hsvArr = new float[width * height][];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float scaledX = Scale(x, width);
                float scaledY = Scale(y, height);
                float distCenter = GetDistFromCenter(scaledX, scaledY);

                hsvArr[x + (y * width)] = cppn.Process(new float[] { scaledX, scaledY, 0, distCenter, 0, 0, 0, BIAS });
            }
        }

        return hsvArr;
    }

    private float Scale(int toScale, int maxDimension)
    {
        float result;

        result = (((toScale * 1f / (maxDimension)) * 2) - 1) * ZOOM;

        return result;
    }

    private float GetDistFromCenter(float x, float y)
    {
        float result = float.NaN;

        result = Mathf.Sqrt((x * x + y * y)) * Mathf.Sqrt(2);

        return result;
    }
}
