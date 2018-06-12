using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageFromCPPNTest : MonoBehaviour {

    private static readonly int NUM_INPUTS = 4;
    private static readonly int NUM_OUTPUTS = 3;


    TWEANNGenotype cppnTest;
    TWEANN cppn;
    float[] inputs, outputs;//float x, y, distFromCenter, bias;
    int width, height;
    Texture2D img;
    Renderer renderer;
    //int newNodeID = 1000;
    bool running = true;

	void Start ()
    {
        width = height = 128;
        renderer = GetComponent<Renderer>();
        img = new Texture2D(width, height, TextureFormat.ARGB32, true);

        cppnTest = new TWEANNGenotype(NUM_INPUTS, NUM_OUTPUTS, 0);
        GenerateCPPN();
        DoImage();
        renderer.material.mainTexture = img;

    }
	
	// Update is called once per frame
	void Update () {
        if (!PauseMenu.isPaused && Input.GetButtonDown("Fire2"))
        {
            cppnTest = new TWEANNGenotype(NUM_INPUTS, NUM_OUTPUTS, 0);
            GenerateCPPN();
            DoImage();
            renderer.material.mainTexture = img;
        }

        if (!PauseMenu.isPaused && Input.GetButtonDown("Fire1"))
        {
            EvolveImage();
            renderer.material.mainTexture = img;
        }
    }

    void DoImage()
    {

        img = CreateCPPNImage(width, height);
        //img = CreateRandomTexture(width, height);

    }

    void EvolveImage()
    {
        string debugMsg = "Evolving CPPN ";
        //int howtoEvolve = Random.Range(2, 4);
        int howtoEvolve = 1;
        switch (howtoEvolve)
        {
            case 0:
                // there is no 0 because we always want evolution on the test
                break;
            case 1: // linkMutation
                debugMsg += "using linkMutation.";
                if (ArtGallery.DEBUG_LEVEL > ArtGallery.DEBUG.NONE) Debug.Log(debugMsg);

                cppnTest.LinkMutation();
                break;
            case 2: // perturbLink
                int link = Random.Range(0, cppnTest.GetLinks().Count - 1);
                float delta = RandomGenerator.NextGaussian() * 0.001f;
                debugMsg += "using perturbLink on link " + link + " with a delta of " + delta;
                if (ArtGallery.DEBUG_LEVEL > ArtGallery.DEBUG.NONE) Debug.Log(debugMsg);

                cppnTest.PerturbLink(link, delta);

                break;
            case 3: // spliceMutation
                debugMsg += "using spliceMutation.";
                if (ArtGallery.DEBUG_LEVEL > ArtGallery.DEBUG.NONE) Debug.Log(debugMsg);

                cppnTest.SpliceMutation();
                break;
            default:
                break;
        }
        if (ArtGallery.DEBUG_LEVEL > ArtGallery.DEBUG.NONE) Debug.Log(debugMsg);
        img = CreateCPPNImage(width, height);
    }

    void GenerateCPPN()
    {
        foreach (NodeGene node in cppnTest.GetNodes())
        {
            node.fType = ActivationFunctions.RandomFTYPE();
        }
        //for (int i = 0; i < 5; i++)
        //{
        //    long newNodeInnovation = EvolutionaryHistory.NextInnovationID();
        //    long toLinkInnovation = EvolutionaryHistory.NextInnovationID();
        //    long fromLinkInnovation = EvolutionaryHistory.NextInnovationID();

        //    cppnTest.SpliceNode(ActivationFunctions.RandomFTYPE(), EvolutionaryHistory.NextInnovationID(), cppnTest.GetNodes()[RandomInput()].GetInnovation(),
        //        cppnTest.GetNodes()[RandomOut()].GetInnovation(), Random.value * Random.Range(-1, 1), Random.value * Random.Range(-1, 1), toLinkInnovation, fromLinkInnovation);
        //}

        cppn = new TWEANN(cppnTest);

    }

    float GetDistFromCenter(float x, float y)
    {
        float result = float.NaN;
       
        result = Mathf.Sqrt((x*x + y*y)) * Mathf.Sqrt(2);

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

    Texture2D CreateCPPNImage(int width, int height)
    {
        GenerateCPPN();

        //Texture2D img = new Texture2D(width, height);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float scaledX = Scale(x, width);
                float scaledY = Scale(y, height);
                float[] hsv = cppn.Process(new float[] { scaledX, scaledY, GetDistFromCenter(scaledX, scaledY), 1 });

                // HSV value restrictions

                hsv[0] = Mathf.Clamp(hsv[2], 0.0f, 1.0f);
                hsv[1] = Mathf.Clamp(hsv[2], 0.0f, 1.0f);
                hsv[2] = Mathf.Abs(Mathf.Clamp(hsv[2], 1.0f, 1.0f));

                Color color = Color.HSVToRGB(hsv[0], hsv[1], hsv[2]);
                
                img.SetPixel(x, y, color);
            }
        }

        img.Apply();
        return img;
    }

    float Scale(int toScale, int maxDimension)
    {
        float result;

        result = ((toScale * 1.0f / (maxDimension - 1)) * 2) - 1;

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
