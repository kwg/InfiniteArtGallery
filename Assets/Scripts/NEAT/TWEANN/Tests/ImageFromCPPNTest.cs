using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// FIXME Now reverse this test chamber to use the Artwork class instead of hard coding in here
/// <summary>
/// Class that was used as the development platform for Artwork
/// </summary>
public class ImageFromCPPNTest : MonoBehaviour
{

    //private static readonly int NUM_INPUTS = 4;
    //private static readonly int NUM_OUTPUTS = 3;

    //OutputText textbox;
    //TWEANNGenotype cppnTest;
    //TWEANN cppn;
    //Artwork art;
    //float[] inputs, outputs;//float x, y, distFromCenter, bias;

    //int width, height;
    //Texture2D img;
    //Renderer renderer;
    ////int newNodeID = 1000;

    //bool running = true;

    //const float BIAS = 1f;
    //public static int TWO_DIMENSIONAL_HUE_INDEX = 0;
    //public static int TWO_DIMENSIONAL_SATURATION_INDEX = 1;
    //public static int TWO_DIMENSIONAL_BRIGHTNESS_INDEX = 2;

    //void Start()
    //{
    //    EvolutionaryHistory.InitializeEvolutionaryHistory();
    //    EvolutionaryHistory.archetypes[0] = new TWEANNGenotype(4, 3, 0).Nodes;
    //    textbox = GetComponent<OutputText>();

    //    art = new Artwork();

    //    ActivationFunctions.ActivateAllFunctions();
    //    //ActivationFunctions.ActivateFunction(new List<FTYPE> { FTYPE.SAWTOOTH });
    //    width = height = 256;
    //    renderer = GetComponent<Renderer>();
    //    img = new Texture2D(width, height, TextureFormat.ARGB32, true);

    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if(art != null && art.NeedsRedraw())
    //    {
    //        art.ApplyImageProcess();
    //        img = art.GetArtwork();
    //        renderer.material.mainTexture = img;
    //        Debug.Log("Image applied");
    //    }

    //    if (!PauseMenu.isPaused && Input.GetButtonDown("Fire2"))
    //    {
    //        img = new Texture2D(width, height, TextureFormat.ARGB32, true);
    //        art = new Artwork();
    //        textbox.Text("New image and genome");
    //    }

    //    if (!PauseMenu.isPaused && Input.GetButtonDown("Fire1"))
    //    {
    //        TWEANNGenotype geno = art.GetGenotype();
    //        geno.Mutate();
    //        textbox.Text("Mutating...");
    //        art = new Artwork(geno);
            
    //    }
    //}

    //void DoImage()
    //{

    //    img = CreateCPPNImage(width, height);
    //    //img = CreateRandomTexture(width, height);

    //}

    //void EvolveImage()
    //{
    //    string debugMsg = "Evolving CPPN. ";
    //    int howtoEvolve = Random.Range(0, 3) + 1;
    //    debugMsg += "Rolled a " + howtoEvolve + " : ";
    //    //int howtoEvolve = 1;
    //    switch (howtoEvolve)
    //    {
    //        case 0:
    //            // there is no 0 because we always want evolution on the test
    //            break;
    //        case 1: // linkMutation
    //            debugMsg += "using linkMutation.";
    //            if (ArtGallery.DEBUG_LEVEL > ArtGallery.DEBUG.NONE) Debug.Log(debugMsg);

    //            cppnTest.LinkMutation();
    //            break;
    //        case 2: // perturbLink
    //            debugMsg += "using perturbLinks";
    //            if (ArtGallery.DEBUG_LEVEL > ArtGallery.DEBUG.NONE) Debug.Log(debugMsg);

    //            cppnTest.PerturbLinks(Random.Range(0.0f, 1.0f));

    //            break;
    //        case 3: // spliceMutation
    //            debugMsg += "using spliceMutation.";
    //            if (ArtGallery.DEBUG_LEVEL > ArtGallery.DEBUG.NONE) Debug.Log(debugMsg);

    //            cppnTest.SpliceMutation();
    //            break;
    //        default:
    //            break;
    //    }
    //    img = CreateCPPNImage(width, height);
    //}

    //void GenerateCPPN()
    //{
    //    foreach (NodeGene node in cppnTest.Nodes)
    //    {
    //        node.fTYPE = ActivationFunctions.RandomFTYPE();
    //    }
    //    cppn = new TWEANN(cppnTest);
    //}

    //float GetDistFromCenter(float x, float y)
    //{
    //    float result = float.NaN;

    //    result = Mathf.Sqrt((x * x + y * y)) * Mathf.Sqrt(2);

    //    return result;
    //}

    //Texture2D CreateRandomTexture(int width, int height)
    //{

    //    for (int y = 0; y < height; y++)
    //    {
    //        for (int x = 0; x < width; x++)
    //        {
    //            img.SetPixel(x, y, new Color(Random.value, Random.value, Random.value, 1));
    //        }
    //    }

    //    img.Apply();
    //    return img;
    //}

    //Texture2D CreateCPPNImage(int width, int height)
    //{
    //    GenerateCPPN();

    //    //Texture2D img = new Texture2D(width, height);

    //    for (int y = 0; y < height; y++)
    //    {
    //        for (int x = 0; x < width; x++)
    //        {
    //            float scaledX = Scale(x, width);
    //            float scaledY = Scale(y, height);
    //            float distCenter = GetDistFromCenter(scaledX, scaledY);
    //            float[] hsv = ProcessCPPNInput(scaledX, scaledY, GetDistFromCenter(scaledX, scaledY), BIAS);
    //            // This initial hue is in the range [-1,1] as in the MM-NEAT code
    //            float initialHue = ActivationFunctions.Activation(FTYPE.PIECEWISE, hsv[TWO_DIMENSIONAL_HUE_INDEX]);
    //            // However, C Sharp's Colors do not automatically map negative numbers to the proper hue range as in Java, so an additional step is needed
    //            float finalHue = initialHue < 0 ? initialHue + 1 : initialHue;
    //            Color colorHSV = Color.HSVToRGB(
    //                finalHue,
    //                ActivationFunctions.Activation(FTYPE.HLPIECEWISE, hsv[TWO_DIMENSIONAL_SATURATION_INDEX]),
    //                Mathf.Abs(ActivationFunctions.Activation(FTYPE.PIECEWISE, hsv[TWO_DIMENSIONAL_BRIGHTNESS_INDEX])),
    //                true
    //                );


    //            img.SetPixel(x, y, colorHSV);
    //        }
    //    }

    //    img.Apply();
    //    return img;
    //}

    //private float[] ProcessCPPNInput(float scaledX, float scaledY, float distCenter, float bias)
    //{
    //    return cppn.Process(new float[] { scaledX, scaledY, distCenter, bias });
    //}

    //float Scale(int toScale, int maxDimension)
    //{
    //    float result;

    //    result = ((toScale * 1.0f / (maxDimension - 1)) * 2) - 1;

    //    return result;
    //}

    //int RandomInput()
    //{
    //    return Random.Range(0, NUM_INPUTS);
    //}

    //int RandomOut()
    //{
    //    return Random.Range(0, NUM_OUTPUTS);
    //}
}
