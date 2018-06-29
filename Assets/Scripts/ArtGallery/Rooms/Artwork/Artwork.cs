using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Artwork
{ 

    TWEANNGenotype geno;
    TWEANN cppn;
    Texture2D img;
    Color[] pixels;
    Thread cppnProcess;
    bool processingCPPN;
    bool needsRedraw;

    //TODO width, height - These are static for testing but we may want to make them change
    int width = 64;
    int height = 64;

    /// <summary>
    /// Default empty constructor
    /// </summary>
    public Artwork() : this(new TWEANNGenotype(4, 3, 0)) { }

    /// <summary>
    /// Create a new artwork in a room with a new genotype. 
    /// </summary>
    public Artwork(int archetypeIndex) : this(new TWEANNGenotype(4, 3, archetypeIndex)) { }

    /// <summary>
    /// Create a new artwork in a room with a given genotype
    /// </summary>
    /// <param name="geno">Genotype for this artwrok to use</param>
    public Artwork(TWEANNGenotype geno)
    {
        needsRedraw = false;
        processingCPPN = false;
        this.geno = geno; 
        cppnProcess = new Thread ( new ThreadStart (GenerateImageFromCPPN) );
        img = new Texture2D(width, height, TextureFormat.ARGB32, false);
        pixels = new Color[width * height];
        //GenerateImageFromCPPN();  // non threaded version of generation
        cppnProcess.Start();
    }

    public bool NeedsRedraw()
    {
        return needsRedraw;
    }

    public void ApplyImageProcess()
    {
        img.SetPixels(pixels);
        img.Apply();
        needsRedraw = false;
    }

    public void Refresh()
    {
        cppnProcess = new Thread(new ThreadStart(GenerateImageFromCPPN));
        cppnProcess.Start();

    }

    public void GenerateImageFromCPPN()
    {
        processingCPPN = true;
        if (ArtGallery.DEBUG_LEVEL > ArtGallery.DEBUG.NONE) Debug.Log("CPPN Imgage generation started...");

        if (ArtGallery.DEBUG_LEVEL > ArtGallery.DEBUG.NONE) Debug.Log("NETWORK OUTPUT : BEFORE CPPN : Building TWEANN from geno " + geno.ToString());
        cppn = new TWEANN(geno);
        if (ArtGallery.DEBUG_LEVEL > ArtGallery.DEBUG.NONE) Debug.Log("NETWORK OUTPUT : AFTER CPPN  : Building TWEANN from geno " + geno.ToString());

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float scaledX = Scale(x, width);
                float scaledY = Scale(y, height);
                float distCenter = GetDistFromCenter(scaledX, scaledY);
                float[] hsv = ProcessCPPNInput(scaledX, scaledY, GetDistFromCenter(scaledX, scaledY), 1);
                // This initial hue is in the range [-1,1] as in the MM-NEAT code
                float initialHue = ActivationFunctions.Activation(FTYPE.PIECEWISE, hsv[0]);
                // However, C Sharp's Colors do not automatically map negative numbers to the proper hue range as in Java, so an additional step is needed
                float finalHue = initialHue < 0 ? initialHue + 1 : initialHue;
                Color colorHSV = Color.HSVToRGB(
                    finalHue,
                    ActivationFunctions.Activation(FTYPE.HLPIECEWISE, hsv[1]),
                    Mathf.Abs(ActivationFunctions.Activation(FTYPE.PIECEWISE, hsv[2])),
                    true
                    );
                pixels[x + y * width] = colorHSV;
            }
        }
        processingCPPN = false;
        needsRedraw = true;
        //img.SetPixels(pixels);
        //img.Apply();

        if (ArtGallery.DEBUG_LEVEL > ArtGallery.DEBUG.NONE) Debug.Log("CPPN Imgage generation complete");
    }

    private float[] ProcessCPPNInput(float scaledX, float scaledY, float distCenter, int bias)
    {
        return cppn.Process(new float[] { scaledX, scaledY, distCenter, 1 });
    }

    float Scale(int toScale, int maxDimension)
    {
        float result;

        result = ((toScale * 1.0f / (maxDimension - 1)) * 2) - 1;

        return result;
    }

    float GetDistFromCenter(float x, float y)
    {
        float result = float.NaN;

        result = Mathf.Sqrt((x * x + y * y)) * Mathf.Sqrt(2);

        return result;
    }

    public Texture2D GetArtwork()
    {
        return img;

    }

    public void SetGenotype(TWEANNGenotype geno)
    {

        this.geno = geno;
    }

    public TWEANNGenotype GetGenotype()
    {
        return geno;
    }

    int RandomInput()
    {
        return Random.Range(0, 4);
    }

    int RandomOut()
    {
        return Random.Range(0, 3);
    }
}
