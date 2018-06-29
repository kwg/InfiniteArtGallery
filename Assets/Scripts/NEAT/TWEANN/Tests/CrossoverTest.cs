using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossoverTest : MonoBehaviour
{
    public GameObject leftQuad;
    public GameObject rightQuad;


    private static readonly int NUM_INPUTS = 4;
    private static readonly int NUM_OUTPUTS = 3;


    Artwork leftArt, rightArt;
    float[] inputs, outputs;//float x, y, distFromCenter, bias;

    int width, height;
    Texture2D leftImg, rightImg;
    Renderer leftRenderer;
    Renderer rightRenderer;


    bool running = true;

    void Start()
    {
        EvolutionaryHistory.InitializeEvolutionaryHistory();
        EvolutionaryHistory.archetypes[0] = new TWEANNGenotype(4, 3, 0).Nodes;
        leftArt = new Artwork();
        rightArt = new Artwork();
        width = height = 64;
        ActivationFunctions.ActivateAllFunctions();
        leftRenderer = leftQuad.GetComponent<Renderer>();
        rightRenderer = rightQuad.GetComponent<Renderer>();
        leftImg = new Texture2D(width, height, TextureFormat.ARGB32, true);
        rightImg = new Texture2D(width, height, TextureFormat.ARGB32, true);
        BuildArtworks();
    }

    private void BuildArtworks()
    {
        TWEANNGenotype leftGeno = leftArt.GetGenotype().Copy();
        TWEANNGenotype rightGeno = rightArt.GetGenotype().Copy();

        for(int i = 0; i < 50; i++)
        {
            leftGeno.Mutate();
            rightGeno.Mutate();
        }

        leftArt = new Artwork(leftGeno);
        rightArt = new Artwork(rightGeno);
    }

    // Update is called once per frame
    void Update()
    {
        if (leftArt.HasFinishedProcessing())
        {
            leftArt.ApplyImageProcess();
            leftImg = leftArt.GetArtwork();
            leftRenderer.material.mainTexture = leftImg;
            Debug.Log("leftImg applied");
        }
        if (rightArt.HasFinishedProcessing())
        {
            rightArt.ApplyImageProcess();
            rightImg = rightArt.GetArtwork();
            rightRenderer.material.mainTexture = rightImg;
            Debug.Log("rightImg applied");
        }

        if (!PauseMenu.isPaused && Input.GetButtonDown("Fire2"))
        {
            leftImg = new Texture2D(width, height, TextureFormat.ARGB32, true);
            rightImg = new Texture2D(width, height, TextureFormat.ARGB32, true);
            leftArt = new Artwork();
            rightArt = new Artwork();
            BuildArtworks();

        }

        if (!PauseMenu.isPaused && Input.GetButtonDown("Fire1"))
        {
            TWEANNGenotype leftGeno = leftArt.GetGenotype().Copy();
            TWEANNGenotype rightGeno = rightArt.GetGenotype().Copy();


            TWEANNCrossover cr = new TWEANNCrossover(false);
            cr.Crossover(leftGeno, rightGeno);

            leftArt = new Artwork(leftGeno);
            rightArt = new Artwork(rightGeno);

        }
    }
}
