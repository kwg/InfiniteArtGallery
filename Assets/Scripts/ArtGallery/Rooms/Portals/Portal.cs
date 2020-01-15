using UnityEngine;


/// <summary>
/// Physical portals in the room. Controls the decoration of a portal as well as storing the
/// portal's ID and destination portal ID.
/// </summary>
public class Portal : MonoBehaviour, IUnityGeneticArtwork {

    public GameObject PortalObject;
    public int PortalID;
    public Material Mat;

    private int _destinationID;
    private Texture2D _displayImg;
    private Artwork _artwork;
    private MeshRenderer _rend;




    /* Public methods */
    /// <summary>
    /// Sets the ID of this portal
    /// </summary>
    /// <param name="portalID">ID to set for this portal</param>
    public void SetPortalID(int portalID)
    {
        this.PortalID = portalID;
    }

    /// <summary>
    /// ID of this portal
    /// </summary>
    /// <returns>ID of this portal</returns>
    public int GetPortalID()
    {
        return PortalID;
    }

    public void InitializePortal(Artwork artwork)
    {
        _artwork = artwork;
        _rend = gameObject.GetComponent<MeshRenderer>();
        _rend.material = Mat;

        // HACK BUG HUNTING - remove this
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
    }

    public void Update()
    {
        if (_artwork.NeedsRedraw)
        {
            Color prevColor = _displayImg.GetPixel(64, 64);
            Debug.Log("Found updated texture for portalID " + PortalID);
            _displayImg = _artwork.GetTexture();
            if(_displayImg.GetPixel(64, 64) != prevColor)
            {
                Debug.Log("displayImg changed");
            }
            RefreshDecoration();
        }
        //rend.material.SetTexture("_MainTex", displayImg);
        //transform.Rotate(Vector3.up * (50f * Time.deltaTime));
    }

    /// <summary>
    /// Sets the destiation portal ID for this portal
    /// </summary>
    /// <param name="newDestinationID">ID of portal to exit from</param>
    public void SetDestinationID(int newDestinationID)
    {
        _destinationID = newDestinationID;
    }

    /// <summary>
    /// ID of exit portal
    /// </summary>
    /// <returns>ID of portal this portal exits from</returns>
    public int GetDestinationID()
    {
        return _destinationID;
    }

    public Texture2D GetImage()
    {
        return _displayImg;
    }


    /* IUnityGeneticArtwork */
    public void UpdateGeneratedArt()
    {
        // trigger artwork update
        RefreshDecoration();
    }

    public GeneticArt GetGeneticArt()
    {
        return _artwork;
    }

    public bool SetGeneticArt(GeneticArt newGeneticArt)
    {
        _artwork = (Artwork) newGeneticArt;
        return true;
    }

    /* Private Methods */
    /// <summary>
    /// Refresh the displayed color and texture in game to match the color and texture specified by the genotype
    /// </summary>
    private void RefreshDecoration()
    {
        _rend.material.SetTexture("_MainTex", _displayImg);

        //Texture2D temp = (Texture2D) rend.material.mainTexture;

        //if (portalID == 0) Debug.Log("Refreshed color for pixel at 64x64 = " + temp.GetPixel(64, 64));
    }


}
