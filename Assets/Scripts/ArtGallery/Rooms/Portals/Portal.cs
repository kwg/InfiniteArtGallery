using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Physical portals in the room. Controls the decoration of a portal as well as storing the
/// portal's ID and destination portal ID.
/// </summary>
public class Portal : MonoBehaviour {

    public GameObject portalObject;

    // TODO change these to private and use getters and setters since we are no longer using the editor to do this by hand
    public int portalID;
    int destinationID;

    float x, y, z;

    GameObject portalProp;

    private Color displayColor;
    private Texture2D displayImg;  


    /* Public methods */
    /// <summary>
    /// Unity method
    /// </summary>
    public void Start()
    {
        //portalProp = Instantiate(portalObject) as GameObject;
    }

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

    /// <summary>
    /// Tells this portal that the player has chosen it
    /// </summary>
    /// <param name="player">Player that selected the portal</param>
    public void ActivatePortal(Player player)
    {
        ArtGallery pc = FindObjectOfType<ArtGallery>();

        //pc.DoTeleport(player, portalID);
       // pc.DoColorChange();

        //Teleport(player);
    }

    /// <summary>
    /// Decorate portal with a color
    /// </summary>
    /// <param name="newColor">Color to decorate portal with</param>
    public void PaintDoor(Color newColor)
    {
        SetColor(newColor);
    }

    /// <summary>
    /// Decorate portal with a sprite
    /// </summary>
    /// <param name="newSprite">Sprite to decorate portal with</param>
    public void PaintDoor(Texture2D newImg)
    {
        SetImg(newImg);
    }


    /// <summary>
    /// Gets the current color of this portal
    /// </summary>
    /// <returns>Current display color</returns>
    public Color GetColor()
    {
        return displayColor;
    }

    public Texture2D GetImage()
    {
        return displayImg;
    }

    /* Private methods */
    /// <summary>
    /// Set the display color of this portal
    /// </summary>
    /// <param name="newColor">New display color to be used</param>
    private void SetColor(Color newColor)
    {
        displayColor = newColor;
        RefreshDecoration();
    }

    /// <summary>
    /// Set the display sprite for this portal
    /// </summary>
    /// <param name="sprite">New display sprite to be used</param>
    private void SetImg(Texture2D newImg)
    {
        displayImg = newImg;
        RefreshDecoration();
    }

    /// <summary>
    /// Refresh the displayed color and texture in game to match the color and texture specified by the genotype
    /// </summary>
    private void RefreshDecoration()
    {
        Renderer rend = gameObject.GetComponent<Renderer>();
        //rend.material.SetColor("_Color", displayColor);
        rend.material.mainTexture = displayImg;
    }

    public void SetEmmisive(Color color)
    {
        Renderer rend = gameObject.GetComponent<Renderer>();
        rend.material.EnableKeyword("_EMISSION");
        rend.material.SetColor("_EmissionColor", color);
    }
    
}
