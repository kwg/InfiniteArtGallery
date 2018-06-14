using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artwork {

    int doorID;

    TWEANNGenotype geno;
    TWEANN cppn;
    Texture2D img;
    int width = 64;
    int height = 64;

    /// <summary>
    /// Create a new door in a room with a new CPPN. 
    /// </summary>
    /// <param name="pc">Reference to the portal controller that can spawn and decorate portals in the scene</param>
    public Artwork() 
    {
        geno = new TWEANNGenotype(4, 3, 0); // FIXME archetype index 
        GenerateCPPN();
        img = new Texture2D(width, height, TextureFormat.ARGB32, true);
        img = GenerateImageFromCPPN();

    }

    public Artwork(TWEANNGenotype geno)
    {
        this.geno = geno; 
        img = new Texture2D(width, height, TextureFormat.ARGB32, true);
        img = GenerateImageFromCPPN();

    }

    private void GenerateCPPN()
    {

        //geno = new TWEANNGenotype(4, 3, 0);
        foreach (NodeGene node in geno.GetNodes())
        {
            node.fTYPE = ActivationFunctions.RandomFTYPE();
        }
        //for (int i = 0; i < 5; i++)
        //{
        //    int newNodeInnovation = newNodeID++;
        //    int toLinkInnovation = newNodeID++;
        //    int fromLinkInnovation = newNodeID++;

        //    geno.SpliceNode(ActivationFunctions.RandomFTYPE(), newNodeInnovation++, geno.GetNodes()[RandomInput()].GetInnovation(),
        //        geno.GetNodes()[RandomOut()].GetInnovation(), Random.Range(-1f, 1f), Random.Range(-1f, 1f), toLinkInnovation, fromLinkInnovation);
        //}

    }

    public Texture2D GenerateImageFromCPPN()
    {
        cppn = new TWEANN(geno);

        //Texture2D img = new Texture2D(width, height);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float scaledX = Scale(x, width);
                float scaledY = Scale(y, height);
                float[] hsv = cppn.Process(new float[] { scaledX, scaledY, GetDistFromCenter(scaledX, scaledY), 1 });
                //Debug.Log("SPAM! x:" + x + ", y:" + y + ", distFromCenter:" + GetDistFromCenter(x, y) + "");
                //Debug.Log("SPAM! scaledX:" + scaledX + ", scaledY:" + scaledY + ", distFromCenter:" + GetDistFromCenter(scaledX, scaledY) + "");
                //Debug.Log("ColorHSV - h:" +  hsv[0] + " s:" + hsv[1] + " v:" + hsv[2]);
                Color color = Color.HSVToRGB(hsv[0], hsv[1], hsv[2]);

                img.SetPixel(x, y, color);
                img.Apply();
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
