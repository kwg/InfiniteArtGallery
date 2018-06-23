using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageFromCPPNTest : MonoBehaviour {

    private static readonly int NUM_INPUTS = 4;
    private static readonly int NUM_OUTPUTS = 3;
    private static readonly int TIME_STEPS = 1;


    TWEANNGenotype cppnTest;
    TWEANN cppn;
    float[] inputs, outputs;//float x, y, distFromCenter, bias;
    int width, height;
    Texture2D img;
    Renderer renderer;
    //int newNodeID = 1000;
    bool running = true;
    System.DateTime time;

	void Start ()
    {
        time = System.DateTime.Now;
        width = height = 128;
        renderer = GetComponent<Renderer>();
        img = new Texture2D(width, height, TextureFormat.ARGB32, true);

        ActivationFunctions.ActivateAllFunctions();

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
        //for (int t = 0; t < TIME_STEPS; t++)
        //  {
        img = CreateCPPNImage(width, height);//, t);
            //img = CreateRandomTexture(width, height);
       // }
    }

    void EvolveImage()
    {
        string debugMsg = "Evolving CPPN. ";
        int howtoEvolve = Random.Range(0, 3) + 1;
        debugMsg += "Rolled a " + howtoEvolve + " : ";
        //int howtoEvolve = 1;
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
                debugMsg += "using perturbLinks";
                if (ArtGallery.DEBUG_LEVEL > ArtGallery.DEBUG.NONE) Debug.Log(debugMsg);

                cppnTest.PerturbLinks(Random.Range(0.0f, 1.0f));

                break;
            case 3: // spliceMutation
                debugMsg += "using spliceMutation.";
                if (ArtGallery.DEBUG_LEVEL > ArtGallery.DEBUG.NONE) Debug.Log(debugMsg);

                cppnTest.SpliceMutation();
                break;
            default:
                break;
        }
        img = CreateCPPNImage(width, height);
    }

    void GenerateCPPN()
    {
        foreach (NodeGene node in cppnTest.GetNodes())
        {
            node.fTYPE = ActivationFunctions.RandomFTYPE();
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

    struct CoordinatesAndColor
    {
        public int x, y;
        public Color color;
        public CoordinatesAndColor(int x, int y, Color color)
        {
            this.x = x;
            this.y = y;
            this.color = color;
        }
    }
    Texture2D CreateCPPNImage(int width, int height, int time)
    {
        GenerateCPPN();
        List<CoordinatesAndColor> coordinatesAndColorList = new List<CoordinatesAndColor>();
        List<List<CoordinatesAndColor>> images = new List<List<CoordinatesAndColor>>();
        //Texture2D img = new Texture2D(width, height);
        for (int t = 0; )
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float scaledX = Scale(x, width);
                    float scaledY = Scale(y, height);
                    float[] outputs = cppn.Process(new float[] { scaledX, scaledY, GetDistFromCenter(scaledX, scaledY), 1 }); //time
                    // HSV value restrictions

                    outputs[0] = Mathf.Clamp(outputs[2], 0.0f, 1.0f);
                    outputs[1] = Mathf.Clamp(outputs[2], 0.0f, 1.0f);
                    outputs[2] = Mathf.Abs(Mathf.Clamp(outputs[2], 1.0f, 1.0f));

                    Color color = Color.HSVToRGB(outputs[0], outputs[1], outputs[2]);
                    
                    img.SetPixel(x, y, color);
                   // coordinatesAndColorList.Add(new CoordinatesAndColor(x, y, color));
                }
            }
            img.Apply();

       /* int i = 0;
        System.DateTime time = System.DateTime.Now;
        while (i < coordinatesAndColorList.Count)
        {
            if (System.DateTime.Now.Millisecond > time.Millisecond)
            {
                CoordinatesAndColor thisPixel = coordinatesAndColorList[555];
                Debug.Log("x: " + thisPixel.x);
                Debug.Log("y: " + thisPixel.y);
                Debug.Log("color: " + thisPixel.color);
                img.SetPixel(thisPixel.x, thisPixel.y, thisPixel.color);
                img.Apply();
                time = time.Add(new System.TimeSpan(0, 0, 0, 0, 10));
                i++;
                Debug.Log("in here: " + time);
                break;
            }
        }*/
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
