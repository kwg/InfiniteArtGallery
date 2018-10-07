using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[System.Serializable]
public class Artwork
{
    private bool debug = ArtGallery.DEBUG_LEVEL > ArtGallery.DEBUG.NONE;

    ArtGallery ag;

    TWEANNGenotype geno;
    TWEANN cppn;
    Texture2D img;
    Color[] pixels;
    Thread cppnProcess;
    bool processingCPPN;  //FIXME PROTOTYPE this is no longer being checked 
    bool needsRedraw;
    const float BIAS = 1f;
    public static int TWO_DIMENSIONAL_HUE_INDEX = 0;
    public static int TWO_DIMENSIONAL_SATURATION_INDEX = 1;
    public static int TWO_DIMENSIONAL_BRIGHTNESS_INDEX = 2;

    //FIXME PROTOTYPE width, height - These are static for testing but we may want to make them change
    int width = 256;
    int height = 256;

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
        ag = ArtGallery.GetArtGallery();
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

    public Texture2D GetImage()
    {
        return img;
    }

    public void ApplyImageProcess()
    {
        img.SetPixels(pixels);
        img.Apply();
        //FIXME PROTOTYPE disabling to build new method
        ag.SaveImage(this);
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
        if (debug) Debug.Log("CPPN Imgage generation started...");

        if (debug) Debug.Log("NETWORK OUTPUT : BEFORE CPPN : Building TWEANN from geno " + geno.ToString());
        cppn = new TWEANN(geno);
        if (debug) Debug.Log("NETWORK OUTPUT : AFTER CPPN  : Building TWEANN from geno " + geno.ToString());

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float scaledX = Scale(x, width);
                float scaledY = Scale(y, height);
                float distCenter = GetDistFromCenter(scaledX, scaledY);
                float[] hsv = ProcessCPPNInput(scaledX, scaledY, GetDistFromCenter(scaledX, scaledY), BIAS);
                // This initial hue is in the range [-1,1] as in the MM-NEAT code
                float initialHue = ActivationFunctions.Activation(FTYPE.PIECEWISE, hsv[TWO_DIMENSIONAL_HUE_INDEX]);
                // However, C Sharp's Colors do not automatically map negative numbers to the proper hue range as in Java, so an additional step is needed
                float finalHue = initialHue < 0 ? initialHue + 1 : initialHue;
                Color colorHSV = Color.HSVToRGB(
                    finalHue,
                    ActivationFunctions.Activation(FTYPE.HLPIECEWISE, hsv[TWO_DIMENSIONAL_SATURATION_INDEX]),
                    Mathf.Abs(ActivationFunctions.Activation(FTYPE.PIECEWISE, hsv[TWO_DIMENSIONAL_BRIGHTNESS_INDEX])),
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

    private float[] ProcessCPPNInput(float scaledX, float scaledY, float distCenter, float bias)
    {
        return cppn.Process(new float[] { scaledX, scaledY, distCenter, bias });
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
