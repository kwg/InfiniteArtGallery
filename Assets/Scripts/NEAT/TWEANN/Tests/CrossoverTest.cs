using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossoverTest : MonoBehaviour
{
    //public GameObject leftQuad;
    //public GameObject rightQuad;


    //private static readonly int NUM_INPUTS = 4;
    //private static readonly int NUM_OUTPUTS = 3;
    //// active functions
    //List<FTYPE> activeFunctions;
    //// collectedFunctions
    //List<FTYPE> collectedFunctions;

    //Artwork leftArt, rightArt;
    //TWEANNGenotype leftGeno;
    //TWEANNGenotype rightGeno;
    //float[] inputs, outputs;//float x, y, distFromCenter, bias;

    //int width, height;
    //Texture2D leftImg, rightImg;
    //Renderer leftRenderer;
    //Renderer rightRenderer;


    //bool running = true;

    //void Start()
    //{
    //    EvolutionaryHistory.InitializeEvolutionaryHistory();
    //    EvolutionaryHistory.archetypes[0] = new TWEANNGenotype(4, 3, 0).Nodes;
    //    width = height = 64;
    //    //ActivationFunctions.ActivateAllFunctions();
    //    collectedFunctions = new List<FTYPE> { FTYPE.ID, FTYPE.TANH, FTYPE.SQUAREWAVE, FTYPE.GAUSS, FTYPE.SINE };
    //    activeFunctions = new List<FTYPE> { FTYPE.ID, FTYPE.GAUSS, FTYPE.SINE };
    //    ActivationFunctions.ActivateFunction(activeFunctions);

    //    leftRenderer = leftQuad.GetComponent<Renderer>();
    //    rightRenderer = rightQuad.GetComponent<Renderer>();
    //    leftImg = new Texture2D(width, height, TextureFormat.ARGB32, true);
    //    rightImg = new Texture2D(width, height, TextureFormat.ARGB32, true);
    //    BuildArtworks();
    //}

    //private void MutateArtworks()
    //{
    //    leftGeno = new TWEANNGenotype(NUM_INPUTS, NUM_OUTPUTS, 0);
    //    rightGeno = new TWEANNGenotype(NUM_INPUTS, NUM_OUTPUTS, 0);

    //    for (int i = 0; i < 10; i++)
    //    {
    //        leftGeno.Mutate();
    //        rightGeno.Mutate();
    //    }
    //}

    //private void BuildArtworks()
    //{
    //    MutateArtworks();
    //    leftArt = new Artwork(leftGeno);
    //    rightArt = new Artwork(rightGeno);
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (leftArt.NeedsRedraw())
    //    {
    //        leftArt.ApplyImageProcess();
    //        leftImg = leftArt.GetArtwork();
    //        leftRenderer.material.mainTexture = leftImg;
    //        Debug.Log("leftImg applied");
    //    }
    //    if (rightArt.NeedsRedraw())
    //    {
    //        rightArt.ApplyImageProcess();
    //        rightImg = rightArt.GetArtwork();
    //        rightRenderer.material.mainTexture = rightImg;
    //        Debug.Log("rightImg applied");
    //    }

    //    if (!PauseMenu.isPaused && Input.GetButtonDown("Fire2"))
    //    {
    //        leftImg = new Texture2D(width, height, TextureFormat.ARGB32, true);
    //        rightImg = new Texture2D(width, height, TextureFormat.ARGB32, true);
    //        BuildArtworks();
    //        //leftArt = new Artwork(leftGeno);
    //        //rightArt = new Artwork(rightGeno);

    //    }

    //    if (!PauseMenu.isPaused && Input.GetButtonDown("Fire1"))
    //    {
    //        leftGeno = new TWEANNGenotype(leftArt.GetGenotype().Copy());
    //        rightGeno = new TWEANNGenotype(rightArt.GetGenotype().Copy());


    //        TWEANNCrossover cr = new TWEANNCrossover(false);
    //        cr.Crossover(leftGeno, rightGeno);
    //        leftImg = new Texture2D(width, height, TextureFormat.ARGB32, true);
    //        rightImg = new Texture2D(width, height, TextureFormat.ARGB32, true);
    //        leftArt = new Artwork(leftGeno);
    //        rightArt = new Artwork(rightGeno);

    //    }
    //}
}
