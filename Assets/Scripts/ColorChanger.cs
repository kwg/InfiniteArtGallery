using System.Collections;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    private Color color;
    private float lerpFactor = 0.7f;

    private void Start()
    {
        color = gameObject.GetComponent<Renderer>().material.color;
    }

    public void SetColor(Color newColor)
    {
        color = newColor;
        UpdateColor();
    }

    public Color GetCurrentColor()
    {
        return color;
    }

    public void EvolveColor(Portal entryPortal)
    {
        ArrayList oldColors = new ArrayList();
        Color selectedColor = gameObject.GetComponent<Renderer>().material.color;

        foreach (Portal portal in FindObjectsOfType<Portal>())
        {
            if(entryPortal != portal)
            {
                oldColors.Add(portal.GetComponent<ColorChanger>().color);
            }
        }

        foreach (Portal portal in FindObjectsOfType<Portal>())
        {
            if(portal.portalID != entryPortal.destinationID)
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
    }

    void UpdateColor()
    {
        //        gameObject.GetComponent<Material>().SetColor("_Color", color);
        Renderer rend = gameObject.GetComponent<Renderer>();
        rend.material.SetColor("_Color", color);
    }
}