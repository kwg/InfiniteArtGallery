using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageFromCPPNTest : MonoBehaviour {

    private static readonly int NUM_INPUTS = 4;
    private static readonly int NUM_OUTPUTS = 3;


    TWEANNGenotype cppnTest;
    TWEANN cppn;
    double[] inputs, outputs;//double x, y, distFromCenter, bias;
    int width, height;
    Texture2D img;
    Renderer renderer;
    int newNodeID = 1000;
    bool running = true;

	void Start ()
    {
        width = height = 64;
        renderer = GetComponent<Renderer>();
        img = new Texture2D(width, height, TextureFormat.ARGB32, true);

        GenerateCPPN();
        DoImage();
        renderer.material.mainTexture = img;

    }
	
	// Update is called once per frame
	void Update () {
        if (!PauseMenu.isPaused && Input.GetButtonDown("Fire1"))
        {
            DoImage();
            renderer.material.mainTexture = img;
        }
    }

    void DoImage()
    {
        img = CreateRandomCPPNImage(width, height);
        //img = CreateRandomTexture(width, height);

    }

    void GenerateCPPN()
    {
        cppnTest = new TWEANNGenotype(NUM_INPUTS, NUM_OUTPUTS, 0);
        foreach (NodeGene node in cppnTest.GetNodes())
        {
            node.fType = RandomFTYPE();
        }
        for (int i = 0; i < 5; i++)
        {
            int newNodeInnovation = newNodeID++;
            int toLinkInnovation = newNodeID++;
            int fromLinkInnovation = newNodeID++;

            cppnTest.SpliceNode(RandomFTYPE(), newNodeInnovation++, cppnTest.GetNodes()[RandomInput()].GetInnovation(),
                cppnTest.GetNodes()[RandomOut()].GetInnovation(), Random.value * Random.Range(-1, 1), Random.value * Random.Range(-1, 1), toLinkInnovation, fromLinkInnovation);
        }

        cppn = new TWEANN(cppnTest);

    }

    double GetDistFromCenter(double x, double y)
    {
        double result = double.NaN;
       
        result = System.Math.Sqrt((x*x + y*y)) * System.Math.Sqrt(2);

        return result;
    }

    Texture2D CreateRandomTexture(int width, int height)
    {

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                img.SetPixel(x, y, new Color(Random.value, Random.value, Random.value, 1));
            }
        }

        img.Apply();
        return img;
    }

    Texture2D CreateRandomCPPNImage(int width, int height)
    {
        GenerateCPPN();

        //Texture2D img = new Texture2D(width, height);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                double scaledX = Scale(x, width);
                double scaledY = Scale(y, height);
                double[] hsv = cppn.Process(new double[] { scaledX, scaledY, GetDistFromCenter(scaledX, scaledY), 1 });
                //Debug.Log("SPAM! x:" + x + ", y:" + y + ", distFromCenter:" + GetDistFromCenter(x, y) + "");
                //Debug.Log("SPAM! scaledX:" + scaledX + ", scaledY:" + scaledY + ", distFromCenter:" + GetDistFromCenter(scaledX, scaledY) + "");
                //Debug.Log("ColorHSV - h:" +  hsv[0] + " s:" + hsv[1] + " v:" + hsv[2]);
                Color color = Color.HSVToRGB((float)hsv[0], (float)hsv[1], (float)hsv[2]);
                
                img.SetPixel(x, y, color);
                img.Apply();
            }
        }

        img.Apply();
        return img;
    }

    double Scale(int toScale, int maxDimension)
    {
        double result;

        result = ((toScale * 1.0 / (maxDimension - 1)) * 2) - 1;

        return result;
    }


    // FIXME DELETE THIS
    FTYPE RandomFTYPE()
    {
        FTYPE result = FTYPE.ID;

        int rnd = Random.Range(1, 6);
        switch (rnd)
        {
            case 1:
                result = FTYPE.TANH;
                break;
            case 2:
                result = FTYPE.SIGMOID;
                break;
            case 3:
                result = FTYPE.SINE;
                break;
            case 4:
                result = FTYPE.COS;
                break;
            case 5:
                result = FTYPE.GAUSS;
                break;
            case 6:
                result = FTYPE.ID;
                break;
            default:
                break;

        }

        return result;
    }

    int RandomInput()
    {
        return Random.Range(0, NUM_INPUTS);
    }

    int RandomOut()
    {
        return Random.Range(0, NUM_OUTPUTS);
    }
}
