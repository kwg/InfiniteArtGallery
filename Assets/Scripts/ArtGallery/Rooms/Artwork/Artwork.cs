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

    float Zoom = 5;
    float MaxValue = float.NegativeInfinity;
    float MinValue = float.PositiveInfinity;

    //FIXME PROTOTYPE width, height - These are static for testing but we may want to make them change
    int width = 128;
    int height = 128;

    /// <summary>
    /// Default empty constructor
    /// </summary>
    public Artwork() : this(new TWEANNGenotype(8, 4, 0)) { }

    /// <summary>
    /// Create a new artwork in a room with a new genotype. 
    /// </summary>
    public Artwork(int archetypeIndex) : this(new TWEANNGenotype(8, 4, archetypeIndex)) { }

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
        needsRedraw = false;
        img.SetPixels(pixels);
        img.Apply();
        //FIXME PROTOTYPE disabling to build new method
        ag.SaveImage(this);
    }

    public void Refresh()
    {
        cppnProcess = new Thread(new ThreadStart(GenerateImageFromCPPN));
        cppnProcess.Start();
        MaxValue = float.NegativeInfinity;
        MinValue = float.PositiveInfinity;

    }

    private void GenerateImageFromCPPN()
    {
        processingCPPN = true;
        if (debug) Debug.Log("CPPN Imgage generation started...");

        if (debug) Debug.Log("NETWORK OUTPUT : BEFORE CPPN : Building TWEANN from geno " + geno.ToString());
        cppn = new TWEANN(geno);
        if (debug) Debug.Log("NETWORK OUTPUT : AFTER CPPN  : Building TWEANN from geno " + geno.ToString());

        Vector3[] hsvArr = new Vector3[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float scaledX = Scale(x, width);
                float scaledY = Scale(y, height);

                float distCenter = GetDistFromCenter(scaledX, scaledY);
                float[] hsv = ProcessCPPNInput(scaledX, scaledY, distCenter, BIAS);
                // This initial hue is in the range [-1,1] as in the MM-NEAT code
                // However, C Sharp's Colors do not automatically map negative numbers to the proper hue range as in Java, so an additional step is needed
                /*Color colorHSV = Color.HSVToRGB(
                    hsv[TWO_DIMENSIONAL_HUE_INDEX],
                    hsv[TWO_DIMENSIONAL_SATURATION_INDEX],
                    hsv[TWO_DIMENSIONAL_BRIGHTNESS_INDEX],
                    false);
                */
                //Debug.Log(hsv[0] + ", " + hsv[1] + ", " + hsv[2]);
                Color colorHSV = Color.HSVToRGB(
                    Mathf.Abs(ActivationFunctions.Activation(FTYPE.HLPIECEWISE, ActivationFunctions.Activation(FTYPE.SINE, hsv[TWO_DIMENSIONAL_HUE_INDEX]))),
                    Mathf.Abs(ActivationFunctions.Activation(FTYPE.HLPIECEWISE, hsv[TWO_DIMENSIONAL_SATURATION_INDEX])),
                    Mathf.Abs(ActivationFunctions.Activation(FTYPE.PIECEWISE, hsv[TWO_DIMENSIONAL_BRIGHTNESS_INDEX])),
                    true
                    );
                

                //MaxValue = Mathf.Max(MaxValue, hsv[TWO_DIMENSIONAL_HUE_INDEX]);
                //MinValue = Mathf.Min(MinValue, hsv[TWO_DIMENSIONAL_HUE_INDEX]);


                //Debug.Log(finalColor[0] + ", " + finalColor[1] + ", " + finalColor[2]);

                pixels[x + y * width] = new Color(hsv[0], hsv[1], hsv[2]); // HACK switched to RGB for testing. This should be an option. Need to map a LOT of outputs for seperate effects
                //hsvArr[x + y * width] = new Vector3(hsv[TWO_DIMENSIONAL_HUE_INDEX], hsv[TWO_DIMENSIONAL_SATURATION_INDEX], hsv[TWO_DIMENSIONAL_BRIGHTNESS_INDEX]);
            }
        }
        //for (int i = 0; i < hsvArr.Length; i++)
        //{
        //    float[] v = FormatHSVMod(new float[] { hsvArr[i][TWO_DIMENSIONAL_HUE_INDEX], hsvArr[i][TWO_DIMENSIONAL_SATURATION_INDEX], hsvArr[i][TWO_DIMENSIONAL_BRIGHTNESS_INDEX] });
        //    pixels[i] = Color.HSVToRGB(v[TWO_DIMENSIONAL_HUE_INDEX], v[TWO_DIMENSIONAL_SATURATION_INDEX], v[TWO_DIMENSIONAL_BRIGHTNESS_INDEX], true);
       //}
        //Debug.Log("MaxValue: " + MaxValue + ", MinValue: " + MinValue);

        processingCPPN = false;
        needsRedraw = true;
        //img.SetPixels(pixels);
        //img.Apply();
        if (ArtGallery.DEBUG_LEVEL > ArtGallery.DEBUG.NONE) Debug.Log("CPPN Imgage generation complete");
    }

    private float[] FormatHSV(float[] hsv)
    {
        float[] result = new float[hsv.Length];
        float range = MaxValue - MinValue;

        result[TWO_DIMENSIONAL_HUE_INDEX] = ((hsv[TWO_DIMENSIONAL_HUE_INDEX] - MinValue) / range);
        //result[TWO_DIMENSIONAL_HUE_INDEX] = Mathf.Abs((ActivationFunctions.Activation(FTYPE.PIECEWISE, hsv[TWO_DIMENSIONAL_HUE_INDEX])));

        result[TWO_DIMENSIONAL_SATURATION_INDEX] = ActivationFunctions.Activation(FTYPE.HLPIECEWISE, hsv[TWO_DIMENSIONAL_SATURATION_INDEX]);
        result[TWO_DIMENSIONAL_BRIGHTNESS_INDEX] = Mathf.Abs(ActivationFunctions.Activation(FTYPE.PIECEWISE, hsv[TWO_DIMENSIONAL_BRIGHTNESS_INDEX]));

        return result;
    }

    private float[] FormatHSVMod(float[] hsv)
    {
        float[] result = new float[hsv.Length];
        float range = MaxValue - MinValue;

        result[TWO_DIMENSIONAL_HUE_INDEX] = ((hsv[TWO_DIMENSIONAL_HUE_INDEX] - MinValue) % 1);
        //result[TWO_DIMENSIONAL_HUE_INDEX] = Mathf.Abs((ActivationFunctions.Activation(FTYPE.PIECEWISE, hsv[TWO_DIMENSIONAL_HUE_INDEX])));

        result[TWO_DIMENSIONAL_SATURATION_INDEX] = ActivationFunctions.Activation(FTYPE.HLPIECEWISE, hsv[TWO_DIMENSIONAL_SATURATION_INDEX]);
        result[TWO_DIMENSIONAL_BRIGHTNESS_INDEX] = Mathf.Abs(ActivationFunctions.Activation(FTYPE.PIECEWISE, hsv[TWO_DIMENSIONAL_BRIGHTNESS_INDEX]));

        return result;
    }

    private float[] ProcessCPPNInput(float scaledX, float scaledY, float distCenter, float bias)
    {
        //HACK FIXME scaledZ and sculpture distances hard coded to 0 - maybe combine all network processing to a utility function that figures all of that out
        return cppn.Process(new float[] { scaledX, scaledY, 0, distCenter, 0, 0, 0, bias });
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
