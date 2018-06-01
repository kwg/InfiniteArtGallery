using System.Collections;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    private Color color;
    private float lerpFactor = 0.7f;

    /// <summary>
    /// Initialization
    /// <para>Sets color reference to color of the material as set by the editor</para>
    /// </summary>
    private void Start()
    {
        //color = gameObject.GetComponent<Renderer>().material.color;
    }

    /// <summary>
    /// Modify and update color of the first material attached to this gameObject
    /// </summary>
    /// <param name="newColor">New color</param>
    public void SetColor(Color newColor)
    {
        color = newColor;
        UpdateColor();
    }

    /// <summary>
    /// Returns current color of the first material attached to this gameObject
    /// </summary>
    /// <returns>Color of material</returns>
    public Color GetCurrentColor()
    {
        return color;
    }

    /// <summary>
    /// Evolves portal colors based on user selection. 
    /// <para>The selected portal is assigned a new random color. 
    /// The exit portal is assigned a random color from any portal except the selected portal. All remaining portals
    /// have their current color modified by the selected portal color.</para>
    /// </summary>
    /// <param name="entryPortal">Reference to the selected portal</param>
    public void EvolveColor(Portal entryPortal)
    {
        ArrayList oldColors = new ArrayList();
        Color selectedColor = gameObject.GetComponent<Renderer>().material.color;

        /* Create arraylist of all colors in use by portals other than the selected portal */
        foreach (Portal portal in FindObjectsOfType<Portal>())
        {
            if(entryPortal != portal)
            {
                oldColors.Add(portal.GetComponent<ColorChanger>().color);
            }
        }

        /* Modify colors of all portals in the scene */
        foreach (Portal portal in FindObjectsOfType<Portal>())
        {
            if(portal.GetPortalID() != entryPortal.GetDestinationID())
            {
                Color oldColor = portal.GetComponent<ColorChanger>().GetCurrentColor();
                Color newColor = Color.Lerp(selectedColor, oldColor, lerpFactor);
                portal.GetComponent<ColorChanger>().SetColor(newColor);
            }
            else
            {
                Color newColor = (Color) oldColors[Random.Range(0, oldColors.Count - 1)];
                portal.GetComponent<ColorChanger>().SetColor(newColor);
            }
        }

        /* Change selected portal to a random color */
        entryPortal.GetComponent<ColorChanger>().SetColor(new Color(Random.value, Random.value, Random.value));
    }

    /// <summary>
    /// Update material color of the first material attached to this gameObject
    /// </summary>
    public void UpdateColor()
    {
        Renderer rend = gameObject.GetComponent<Renderer>();
        rend.material.SetColor("_Color", color);
    }
}