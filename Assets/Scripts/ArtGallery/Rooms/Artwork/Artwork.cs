using UnityEngine;

public class Artwork : GeneticArt
{
    private bool debug = ArtGallery.DEBUG_LEVEL > ArtGallery.DEBUG.NONE;
    protected Texture2D texture;

    //ArtGallery ag;

    //FIXME PROTOTYPE width, height - These are static for testing but we may want to make them change
    static int width = 128;
    static int height = 128;


    public Texture2D GetTexture()
    {
        NeedsRedraw = false;
        return texture;
    }


    /// <summary>
    /// Default empty constructor
    /// </summary>
    public Artwork() : this(new TWEANNGenotype(8, 4, 0)) { }

    /// <summary>
    /// Create a new artwork in a room with a new genotype. 
    /// </summary>
    public Artwork(int archetypeIndex) : this(new TWEANNGenotype(8, 4, archetypeIndex)) { }

    /// <summary>
    /// Create a new artwork in a room with a given genotype
    /// </summary>
    /// <param name="geno">Genotype for this artwork to use</param>
    public Artwork(TWEANNGenotype geno) : base(geno, new int[] { width, height, 0 }, new Process2D())
    {
        NeedsRedraw = false;
    }

    override protected void UpdateCPPNArt()
    {
        int width = spatialInputLimits[0];
        int height = spatialInputLimits[1];

        texture = new Texture2D(width, height, TextureFormat.ARGB32, false);

        texture.SetPixels32(adjustedCPPNOutput);
        texture.Apply();

        Debug.Log("Texture updated and ready for painting on portal!");

        NeedsRedraw = true;
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
