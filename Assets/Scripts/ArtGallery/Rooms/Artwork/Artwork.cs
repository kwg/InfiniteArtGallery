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
    bool processing;

    //TODO width, height - These are static for testing but we may want to make them change
    int width = 256;
    int height = 256;

    /// <summary>
    /// Create a new artwork in a room with a new genotype. 
    /// </summary>
    public Artwork() : this(new TWEANNGenotype(4, 3, 0)) { }

    /// <summary>
    /// Create a new artwork in a room with a given genotype
    /// </summary>
    /// <param name="geno">Genotype for this artwrok to use</param>
    public Artwork(TWEANNGenotype geno)
    {
        this.geno = geno; 
        cppnProcess = new Thread ( GenerateImageFromCPPN );
        img = new Texture2D(width, height, TextureFormat.ARGB32, false);
        pixels = new Color[width * height];
        processing = true;
        cppnProcess.Start();

    }

    public bool HasFinishedProcessing()
    {
        return processing && !cppnProcess.IsAlive;
    }

    public void ApplyImageProcess()
    {
        img.SetPixels(pixels);
        img.Apply();
        processing = false;
    }

    private void GenerateCPPN()
    {
        foreach (NodeGene node in geno.GetNodes())
        {
            node.fTYPE = ActivationFunctions.RandomFTYPE();
        }
    }

    public void GenerateImageFromCPPN()
    {
        cppn = new TWEANN(geno);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float scaledX = Scale(x, width);
                float scaledY = Scale(y, height);
                float distCenter = GetDistFromCenter(scaledX, scaledY);
                float[] hsv = ProcessCPPNInput(scaledX, scaledY, GetDistFromCenter(scaledX, scaledY), 1);
                Color colorRGB = new Color(Mathf.InverseLerp(0.0f, 1.0f, hsv[1]), Mathf.InverseLerp(0.0f, 1.0f, hsv[1]), Mathf.InverseLerp(0.0f, 1.0f, hsv[2]));
                Color colorHSV = Color.HSVToRGB(colorRGB.r, colorRGB.g, colorRGB.b);
                pixels[x + y * width] = colorHSV;
                //pixels[x + y * width] = Color.HSVToRGB(hsv[0] % 1.0f, hsv[1] % 1.0f, hsv[2] % 1.0f);

                //img.SetPixel(x, y, color);
            }
        }

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

    public void SetGenotypePortal(TWEANNGenotype geno)
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
