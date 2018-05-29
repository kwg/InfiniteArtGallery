using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Portal : MonoBehaviour
{
    public int portalID;
    public int destinationID;

    private Vector3 destination;

    private Color displayColor;
    private Sprite displaySprite;  // TODO Next step will be to get images on portals


    /// <summary>
    /// Unity method
    /// </summary>
    public void Start()
    {
    }

    public void SetPortalID(int portalID)
    {
        this.portalID = portalID;
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
    /// Tells this portal that the player has chosen it
    /// </summary>
    /// <param name="player">Player that selected the portal</param>
    public void ActivatePortal(Player player)
    {
        PopulationController pc = FindObjectOfType<PopulationController>();

        pc.DoTeleport(player, portalID);
        pc.DoColorChange();

        //Teleport(player);
    }

    public void PaintDoor(Color newColor)
    {
        SetColor(newColor);
    }

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

    /// <summary>
    /// Set the display color of this portal
    /// </summary>
    /// <param name="newColor">New display color to be used</param>
    public void SetColor(Color newColor)
    {
        displayColor = newColor;
        RefreshColor();
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
    /// Teleports the player from this portal to the listed destination id
    /// </summary>
    /// <param name="player">Player to be teleported</param>
    private void Teleport(Player player)
    {
        /* Find destination portal and get destination position */
        foreach (Portal portal in FindObjectsOfType<Portal>())
        {
            if (portal.portalID == destinationID)
            {
                destination = portal.gameObject.transform.position; // set destination to exit portal position
            }
        }

        /* Bump player to just outside of the portal collision box based on the location of the portal 
         * relative to the center */
        if (destination.x < 0)
        {
            destination.x += 0.25f;
        }
        else
        {
            destination.x -= 0.25f;
        }

        if (destination.z < 0)
        {
            destination.z += 0.25f;
        }
        else
        {
            destination.z -= 0.25f;
        }

        destination.y -= 1.6f; // Fix exit height for player (player is 1.8 tall, portal is 5, center of portal is 2.5, center of player is 0.9. 2.5 - 0.9 = 1.6)

        player.transform.position = destination;
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
