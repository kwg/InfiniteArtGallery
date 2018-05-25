using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenotypePortal <T> : IGenotype<T> {

    /* List of all parents of this genotype */
    List<long> Parents;

    /* Unique ID assigned by EvolutionaryHistory */
    long ID;

    /* Mutation limits */
    private float mutationLimit = 0.2f;

    /* Traits? */
    float colorHue, colorSaturation, colorValue;
    float red, green, blue;
    Color color;

    /* TODO use NewInstance() */
    /// <summary>
    /// Initialization method since constructors have to be empty
    /// </summary>
    /// <param name="id">long Unique id assigned by EvolutionaryHistory</param>
    public void InitializeGenotype(long id)
    {
        ID = id;
        RandomizeRGB();
    }

    /// <summary>
    /// Assign random values to R, G, B (Random color)
    /// </summary>
    public void RandomizeRGB()
    {
        SetRGB(Random.value, Random.value, Random.value);
        color = new Color(red, green, blue);
    }

    /// <summary>
    /// Set the RGB values
    /// </summary>
    /// <param name="r"></param>
    /// <param name="g"></param>
    /// <param name="b"></param>
    public void SetRGB(float r, float g, float b)
    {
        SetRed(r);
        SetGreen(g);
        SetBlue(b);
        color = new Color(r, g, b);

    }

    /// <summary>
    /// Set the HSV values
    /// </summary>
    /// <param name="hue"></param>
    /// <param name="saturation"></param>
    /// <param name="value"></param>
    public void SetHSV(float hue, float saturation, float value)
    {
        SetColorHue(hue);
        SetColorSaturation(saturation);
        SetColorValue(value);
        color = Color.HSVToRGB(colorHue, colorSaturation, colorValue);

    }

    /// <summary>
    /// Change this color trait
    /// </summary>
    /// <param name="newcolor">New color to use</param>
    public void SetColor(Color newcolor)
    {
        color = newcolor;
    }

    /// <summary>
    /// Utility method to set red value
    /// </summary>
    /// <param name="r">float Value of red</param>
    public void SetRed(float r)
    {
        red = r;
        color = new Color(r, green, blue);

    }

    /// <summary>
    /// Utility method to set green value
    /// </summary>
    /// <param name="g">float Value of green</param>
    public void SetGreen(float g)
    {
        green = g;
        color = new Color(red, g, blue);

    }

    /// <summary>
    /// Utility method to set blue value
    /// </summary>
    /// <param name="b">float Value of blue</param>
    public void SetBlue(float b)
    {
        blue = b;
        color = new Color(red, green, b);

    }

    /// <summary>
    /// Utility method to set hue value (HSV)
    /// </summary>
    /// <param name="hue">float Value of hue (HSV)</param>
    public void SetColorHue(float hue)
    {
        colorHue = hue;
        color = Color.HSVToRGB(colorHue, colorSaturation, colorValue);

    }

    /// <summary>
    /// Utility method to set saturation value (HSV)
    /// </summary>
    /// <param name="saturation">float Value of saturation (HSV)</param>
    public void SetColorSaturation(float saturation)
    {
        colorSaturation = saturation;
        color = Color.HSVToRGB(colorHue, colorSaturation, colorValue);

    }

    /// <summary>
    /// Utility method to set value (HSV)
    /// </summary>
    /// <param name="value">float Value of value (HSV)</param>
    public void SetColorValue(float value)
    {
        colorValue = value;
        color = Color.HSVToRGB(colorHue, colorSaturation, colorValue);

    }

    /// <summary>
    /// Get values for red, green, and blue returned as a Color
    /// </summary>
    /// <returns>Color from RGB values</returns>
    public Color GetColor()
    {
        return color;
    }

    /// <summary>
    /// Adds a parent ID to the ArrayList of parents
    /// </summary>
    /// <param name="id">ID of parent</param>
    public void AddParentID(long id)
    {
        Parents.Add(id);
    }

    /// <summary>
    /// Get a list of all parent IDs of this genotype
    /// </summary>
    /// <returns>Parent ID</returns>
    public List<long> GetParentIDs()
    {
        return Parents;
    }

    /// <summary>
    /// Get a copy of this genotype
    /// </summary>
    /// <returns>IGenotype copy of this genotype</returns>
    public IGenotype<T> Copy()
    {
        IGenotype<T> copyOfGenotype = new GenotypePortal<T>();
        copyOfGenotype = this;
        return copyOfGenotype;
    }

    /// <summary>
    /// Cross this genotype with a given genotype and return the child
    /// </summary>
    /// <param name="g">IGenotype Genotype to be crossed with this genotype</param>
    /// <returns>IGenotype child of the crossover</returns>
    public IGenotype<T> Crossover(IGenotype<T> breederGenotype)
    {
        GenotypePortal<T> crossedGenotype = new GenotypePortal<T>();

        GenotypePortal<T> gp = (GenotypePortal<T>) breederGenotype;

        float rLerp, gLerp, bLerp;

        // TODO This leads to some wild changes - clamp this down to a smaller range
        rLerp = Random.value * mutationLimit;
        gLerp = Random.value * mutationLimit;
        bLerp = Random.value * mutationLimit;

        float newR, newG, newB;
        Color gpColor = gp.GetColor();

        newR = Mathf.Lerp(red, gpColor.r, rLerp);
        newG = Mathf.Lerp(green, gpColor.g, gLerp);
        newB = Mathf.Lerp(blue, gpColor.b, bLerp);

        crossedGenotype.SetRGB(newR, newG, newB);

        return crossedGenotype;
    }

    /// <summary>
    /// Get the id of this genotype
    /// </summary>
    /// <returns></returns>
    public long GetID()
    {
        return ID;
    }

    /// <summary>
    /// Mutate this genotype
    /// </summary>
    public void Mutate()
    {
        //throw new System.NotImplementedException();
    }

    /// <summary>
    /// Create a new instance of this genotype
    /// </summary>
    /// <returns></returns>
    public IGenotype<T> NewInstance()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public T GetPhenotype()
    {
        return default(T);
    }

}
