using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageFromCPPNTest : MonoBehaviour {

    private static readonly int NUM_INPUTS = 4;
    private static readonly int NUM_OUTPUTS = 3;


    TWEANNGenotype cppnTest;
    double[] inputs, outputs;//double x, y, distFromCenter, bias;
    int width, height;
    float quadWidth, quadHeight;
    Texture2D img;
    int newNodeID = 1000;

	void Start ()
    {
        SpriteRenderer sp = gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
        quadWidth = gameObject.transform.localScale.x;
        quadHeight = gameObject.transform.localScale.z;


        cppnTest = new TWEANNGenotype(NUM_INPUTS, NUM_OUTPUTS, 0);

        foreach (NodeGene node in cppnTest.GetNodes())
        {
            node.fType = RandomFTYPE();
        }

        //for (int i = 0; i < 5; i++)
        //{
        //    cppnTest.SpliceNode(RandomFTYPE(), newNodeID++, cppnTest.GetNodes()[RandomInput()].GetInnovation(),
        //        cppnTest.GetNodes()[RandomOut()].GetInnovation(), Random.value * Random.Range(-1, 1), Random.value * Random.Range(-1, 1), newNodeID++, newNodeID++);
        //}

        TWEANN cppn = new TWEANN(cppnTest);
        width = height = 50;
        img = new Texture2D(width, height);

        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                double[] rgb = cppn.Process(new double[] { x, y, GetDistFromCenter(x, y), 1 });
                //Debug.Log("SPAM! x:" + x + ", y:" + y + ", distFromCenter:" + GetDistFromCenter(x, y) + "");
                //Debug.Log("ColorRGB - r:" +  rgb[0] + " g:" + rgb[1] + " b:" + rgb[2]);
                Color color = new Color((float)rgb[0], (float)rgb[1], (float)rgb[2], .5f);
                img.SetPixel(x, y, color);
                img.Apply();
            }
        }

        

        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = img;
        //renderer.material.mainTexture = CreateRandomTexture(width, height);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    double GetDistFromCenter(int x, int y)
    {
        double result = double.NaN;
        double centerX = width / 2;
        double centerY = height / 2;

        result = Mathf.Sqrt((float)((centerX - x)*(centerX - x) + (centerY - y)*(centerY - y)));

        return result;
    }

    Texture2D CreateRandomTexture(int width, int height)
    {
        Texture2D result = new Texture2D(width, height);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                result.SetPixel(x, y, new Color(Random.value, Random.value, Random.value, 1));
            }
        }

        result.Apply();
        return result;
    }

    float Clamp(double x)
    {
        float result = 0.0f;

        // TODO may need this later


        return result;
    }

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
