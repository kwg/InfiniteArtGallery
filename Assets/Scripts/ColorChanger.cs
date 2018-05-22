using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    private Color color;
    private float lerpFactor = 0.9f;

    private void Start()
    {
        color = gameObject.GetComponent<Renderer>().material.color;
    }

    public void SetColor(Color newColor)
    {
        color = newColor;
    }

    public Color GetCurrentColor()
    {
        return color;
    }

    public void EvolveColor(Color selectedColor)
    {
        foreach(Portal portal in FindObjectsOfType<Portal>())
        {
            Color oldColor = portal.GetComponent<ColorChanger>().GetCurrentColor();
            Color newColor = Color.Lerp(selectedColor, oldColor, lerpFactor);
            portal.GetComponent<ColorChanger>().SetColor(newColor);
        }
    }

    void Update()
    {
        //        gameObject.GetComponent<Material>().SetColor("_Color", color);
        Renderer rend = gameObject.GetComponent<Renderer>();
        rend.material.SetColor("_Color", color);
    }
}