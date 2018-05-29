using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Portal : MonoBehaviour
{
    public int portalID;
    public int destinationID;

    private Vector3 destination;

    // FIXME Disable this to start moving geno to population controller 
    private GenotypePortal<Color> geno;

    private Population population;

    private Color displayColor;
    private Sprite displaySprite;  // TODO Next step will be to get images on portals


    private List<GenotypePortal<Color>> GeneticHistory;

    /// <summary>
    /// Unity method
    /// </summary>
    public void Start()
    {

    }

    /// <summary>
    /// Initialization method since Unity constructors must be empty
    /// </summary>
    /// <param name="population">Population controller</param>
    /// <param name="genotypePortal"></param>
    public void InitializePortal(Population population, GenotypePortal<Color> genotypePortal)
    {
        this.population = population;
        geno = genotypePortal;
        GeneticHistory = new List<GenotypePortal<Color>>();


        geno.RandomizeRGB();
        SetColor(geno.GetColor());
        GeneticHistory.Add(geno);
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
       population.TriggerBreeding(this);
       Teleport(player);
    }

    public void PaintDoor<T>(T newPaint)
    {
        PaintDoor(newPaint);
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
    /// <returns></returns>
    public Color GetColor()
    {
        return geno.GetColor();
    }

    /// <summary>
    /// Set the display color of this portal
    /// </summary>
    /// <param name="newColor">New display color to be used</param>
    public void SetColor(Color newColor)
    {
        //geno.SetColor(newColor);
        displayColor = newColor;
        UpdateColor(newColor);
    }

    /// <summary>
    /// Set the display sprite for this portal
    /// </summary>
    /// <param name="sprite">New display sprite to be used</param>
    private void SetSprite(Sprite newSprite)
    {
        displaySprite = newSprite;
        //UpdateSprite(newSprite
    }

    /// <summary>
    /// Get the genotype
    /// </summary>
    /// <returns>Genotype of this portal</returns>
    public GenotypePortal<Color> GetGenotypePortal()
    {
        return geno;
    } 

    /// <summary>
    /// Adds a new genotype
    /// </summary>
    /// <param name="newGeno">Genotyp to add</param>
    public void AddGenotype(GenotypePortal<Color> newGeno)
    {
        //currentGenerationID++;
        geno = newGeno;
        GeneticHistory.Add(geno);
        UpdateColor();
    }

    /// <summary>
    /// Return to an older genotype
    /// </summary>
    /// <param name="generationID">Historic genotype use</param>
    public void SetGenotype(int generationID)
    {
        GeneticHistory.RemoveRange(generationID + 1, GeneticHistory.Count - generationID - 1);
        geno = GeneticHistory[generationID];
        UpdateColor();
    }

    public int GetCurrentGenerationID()
    {
        return GeneticHistory.Count - 1;
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
    private void UpdateColor()
    {
        displayColor = geno.GetColor();
        Renderer rend = gameObject.GetComponent<Renderer>();
        rend.material.SetColor("_Color", displayColor);
    }

    /// <summary>
    /// Refresh the displayed color in game to match the color specified by the genotype
    /// </summary>
    private void UpdateColor(Color color)
    {
        Renderer rend = gameObject.GetComponent<Renderer>();
        rend.material.SetColor("_Color", color);
    }

}
