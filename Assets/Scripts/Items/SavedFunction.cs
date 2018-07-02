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
        Color[] pixels = new Color[thumb.width * thumb.height];
        for(int c = 0; c < pixels.Length; c++)
        {
            pixels[c] = new Color(0f, 0f, 0f, 1f);
        }


        //for(int i = 0; i < thumb.width; i++)
        //{
        //    pixels[i + (thumb.height / 2) * thumb.width] = new Color(1f, 1f, 1f, 1f);
        //}
        //(thumb.height / 2) * thumb.width
        for (int x = 0; x < thumb.width; x++)
        {
            float scaledX = Scale(x, thumb.width);
            float plot = ActivationFunctions.Activation(fTYPE, scaledX * Mathf.PI);
            Color color = new Color(1f, 1f, 1f, 1f);
            Debug.Log((int) ((plot / Mathf.PI * (thumb.height / 2))));
            pixels[x + (int) ((plot * ((thumb.height + 1) / 2)) + ((thumb.height  + 1) / 2)) * thumb.width] = color;
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
}
