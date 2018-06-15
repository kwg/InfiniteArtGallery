﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artwork {

    TWEANNGenotype geno;
    TWEANN cppn;
    Texture2D img;

    //TODO width, height - These are static for testing but we may want to make them change
    int width = 128;
    int height = 128;

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
        img = new Texture2D(width, height, TextureFormat.ARGB32, true);
        img = GenerateImageFromCPPN();

    }

    private void GenerateCPPN()
    {
        foreach (NodeGene node in geno.GetNodes())
        {
            node.fTYPE = ActivationFunctions.RandomFTYPE();
        }
    }

    public Texture2D GenerateImageFromCPPN()
    {
        cppn = new TWEANN(geno);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float scaledX = Scale(x, width);
                float scaledY = Scale(y, height);
                float[] hsv = cppn.Process(new float[] { scaledX, scaledY, GetDistFromCenter(scaledX, scaledY), 1 });
                Color color = Color.HSVToRGB(hsv[0], hsv[1], hsv[2]);

                img.SetPixel(x, y, color);
            }
        }

        img.Apply();

        if (ArtGallery.DEBUG_LEVEL > ArtGallery.DEBUG.NONE) Debug.Log("CPPN Imgage generation complete");

        return img;
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
