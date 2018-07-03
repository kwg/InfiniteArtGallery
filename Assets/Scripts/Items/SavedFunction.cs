using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedFunction : IFunctionItem {

    public string Name { get; set; }

    public Sprite Image { get; set; }

    public FTYPE fTYPE { get; set; }

    public void GenerateThumbnail()
    {
        Texture2D thumb = new Texture2D(32, 32, TextureFormat.ARGB32, false);
        //create a new texture
        Color[] pixels = new Color[thumb.width * thumb.height];
        //fill with black
        for (int c = 0; c < pixels.Length; c++)
        {
            pixels[c] = new Color(0f, 0f, 0f, 1f);
        }
        // plot the function on a line
        for (int x = 0; x < thumb.width; x++)
        {
            // scale from -PI to PI
            float scaledX = Scale(x, thumb.width) * Mathf.PI;
            float plot = ActivationFunctions.Activation(fTYPE, scaledX);
            int mappedPlot;
            //if (plot < -1 || plot > 1)
                mappedPlot = Remap(plot, -Mathf.PI, Mathf.PI, 0, thumb.height - 1);
            //else mappedPlot = Remap(plot, -1, 1, 0, thumb.height - 1);
            Color color = new Color(1f, 1f, 1f, 1f);
            if(ArtGallery.DEBUG_LEVEL >= ArtGallery.DEBUG.VERBOSE) Debug.Log(mappedPlot);
            pixels[x + mappedPlot * thumb.width] = color;
        }

        thumb.SetPixels(pixels);
        thumb.Apply();

        Image = Sprite.Create(thumb, new Rect(0, 0, thumb.width, thumb.height), new Vector2(0.5f, 0.5f));
    }

    float Scale(int toScale, int maxDimension)
    {
        float result;

        result = ((toScale * 1.0f / (maxDimension - 1)) * 2) - 1;

        return result;
    }

    int Remap(float from, float fromMin, float fromMax, float toMin, float toMax)
    {
        if (ArtGallery.DEBUG_LEVEL >= ArtGallery.DEBUG.VERBOSE) Debug.Log("Remapping " + from);
        var fromAbs = from - fromMin;
        var fromMaxAbs = fromMax - fromMin;

        var normal = fromAbs / fromMaxAbs;

        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;

        var to = toAbs + toMin;

        return (int) to;
    }
}
