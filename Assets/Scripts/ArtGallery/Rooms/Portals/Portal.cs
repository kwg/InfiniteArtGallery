using UnityEngine;


/// <summary>
/// Physical portals in the room. Controls the decoration of a portal as well as storing the
/// portal's ID and destination portal ID.
/// </summary>
public class Portal : MonoBehaviour, IArtworkDisplay {

    public GameObject PortalObject;
    public int portalID;
    int destinationID;
    private Texture2D displayImg;
    Renderer rend;





    /* Public methods */
    /// <summary>
    /// Sets the ID of this portal
    /// </summary>
    /// <param name="portalID">ID to set for this portal</param>
    public void SetPortalID(int portalID)
    {
        this.portalID = portalID;
    }

    /// <summary>
    /// ID of this portal
    /// </summary>
    /// <returns>ID of this portal</returns>
    public int GetPortalID()
    {
        return portalID;
    }

    public void InitializePortal()
    {
        displayImg = new Texture2D(128, 128, TextureFormat.ARGB32, false);

        for(int y = 0; y < 128; y++)
        {
            for(int x = 0; x < 128; x++)
            {
                displayImg.SetPixel(x, y, Color.red); 
            }
        }
        rend = gameObject.GetComponent<Renderer>();

        rend.material.SetTexture("_MainTex", displayImg);
    }

    public void Update()
    {
        rend.material.SetTexture("_MainTex", displayImg);
        transform.Rotate(Vector3.up * (50f * Time.deltaTime));
    }

    /// <summary>
    /// Sets the destiation portal ID for this portal
    /// </summary>
    /// <param name="newDestinationID">ID of portal to exit from</param>
    public void SetDestinationID(int newDestinationID)
    {
        destinationID = newDestinationID;
    }

    /// <summary>
    /// ID of exit portal
    /// </summary>
    /// <returns>ID of portal this portal exits from</returns>
    public int GetDestinationID()
    {
        return destinationID;
    }

    public Texture2D GetImage()
    {
        return displayImg;
    }

    public void UpdateGeneratedArt(Color32[] _adjustedCPPNOutput, int[] _spatialInputLimits)
    {
        Texture2D temp = (Texture2D)rend.material.mainTexture;

        if(portalID == 0) Debug.Log("Starting color for pixel at 64x64 = " +
                temp.GetPixel(64, 64));

        int width = _spatialInputLimits[0];
        int height = _spatialInputLimits[1];

        displayImg = new Texture2D(width, height, TextureFormat.ARGB32, false);

        displayImg.SetPixels32(_adjustedCPPNOutput);

        if (portalID == 0) Debug.Log("refreshing color for pixel at 64x64 = " +
            displayImg.GetPixel(64, 64));


        RefreshDecoration();
    }

    /* Private Methods */
    /// <summary>
    /// Refresh the displayed color and texture in game to match the color and texture specified by the genotype
    /// </summary>
    private void RefreshDecoration()
    {
        rend.material.SetTexture("_MainTex", displayImg);

        Texture2D temp = (Texture2D) rend.material.mainTexture;

        if (portalID == 0) Debug.Log("Refreshed color for pixel at 64x64 = " +
                temp.GetPixel(64, 64));
    }
}
