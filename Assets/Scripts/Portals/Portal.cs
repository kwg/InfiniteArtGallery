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
    int portalID;
    int destinationID;

    float x, y, z;

    GameObject portalProp;

    private Color displayColor;
    private Sprite displaySprite;  // TODO Next step will be to get images on portals


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
    public void PaintDoor(Sprite newSprite)
    {
        SetSprite(newSprite);
    }


    /// <summary>
    /// Gets the current color of this portal
    /// </summary>
    /// <returns>Current display color</returns>
    public Color GetColor()
    {
        return displayColor;
    }


    /* Private methods */
    /// <summary>
    /// Set the display color of this portal
    /// </summary>
    /// <param name="newColor">New display color to be used</param>
    private void SetColor(Color newColor)
    {
        displayColor = newColor;
        RefreshColor();
    }

    /// <summary>
    /// This assigns a random color to the portal 
    /// </summary>
    /// <param name="entryPortal">In reference to the portal that was selected</param>
    private void RandomColor(Portal entryPortal)
    {
        foreach (Portal portal in FindObjectsOfType<Portal>())
        {
            if (portal.GetPortalID() != entryPortal.GetDestinationID())
            {
                /* Change selected portal to a random color */
                entryPortal.GetComponent<ColorChanger>().SetColor(new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value));
            }
        }
    }


    /// <summary>
    /// Set the display sprite for this portal
    /// </summary>
    /// <param name="sprite">New display sprite to be used</param>
    private void SetSprite(Sprite newSprite)
    {
        displaySprite = newSprite;
        RefreshSprite();
    }

    /// <summary>
    /// Refresh the displayed color in game to match the color specified by the genotype
    /// </summary>
    private void RefreshColor()
    {
        Renderer rend = gameObject.GetComponent<Renderer>();
        rend.material.SetColor("_Color", displayColor);
    }

    /// <summary>
    /// Refresh the displayed color in game to match the color specified by the genotype
    /// </summary>
    private void RefreshSprite()
    {
        //TODO Add refresh method for sprites
    }
}
