using UnityEngine;


/// <summary>
/// Physical portals in the room. Controls the decoration of a portal as well as storing the
/// portal's ID and destination portal ID.
/// </summary>
public class Portal : MonoBehaviour, IUnityGeneticArtwork {

    public GameObject PortalObject;
    //public Material Mat;

    public int PortalID { get; set; }
    public int DestinationID { get; set; }

    private Texture2D _displayImg;
    private Artwork _artwork;
    private MeshRenderer _rend;
    private bool initialized = false;

    /* Public methods */
    public void InitArtDisplay(GeneticArt art)
    {
        _artwork = new Artwork(art);
        _rend = gameObject.GetComponent<MeshRenderer>();
        //_rend.material = GetComponent<Material>();

        // HACK BUG HUNTING - FIX this - pull width and height from central stats
        _displayImg = new Texture2D(128, 128, TextureFormat.ARGB32, false);

        for(int y = 0; y < 128; y++)
        {
            for(int x = 0; x < 128; x++)
            {
                _displayImg.SetPixel(x, y, Color.green); 
            }
        }
        _displayImg.Apply();

        _rend.material.SetTexture("_MainTex", _displayImg);
        initialized = true;
    }

    public void Update()
    {
        if (initialized)
        {
            if (_artwork.NeedsRedraw)
            {
                _displayImg = _artwork.GetTexture();
                RefreshDecoration();
            }
            if (_artwork.Art.Mutated)
            {
                _artwork.UpdateCPPNArt();
                _artwork.Art.Mutated = false;
            }
        }
    }

    public Texture2D GetImage()
    {
        return _displayImg;
    }


    /* IUnityGeneticArtwork */
    public void UpdateGeneratedArt()
    {
        // trigger artwork update
        _artwork.UpdateCPPNArt();
        RefreshDecoration();
    }

    public GeneticArt GetGeneticArt()
    {
        return _artwork.Art;
    }

    public bool SetGeneticArt(GeneticArt newGeneticArt)
    {
        _artwork.Art = newGeneticArt;
        return true;
    }

    /* Private Methods */
    /// <summary>
    /// Refresh the displayed color and texture in game to match the color and texture specified by the genotype
    /// </summary>
    private void RefreshDecoration()
    {
        _rend.material.SetTexture("_MainTex", _displayImg);
    }
}
