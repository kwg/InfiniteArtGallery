using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    private Color color;


    public void SetColor(Color newColor)
    {
        Renderer rend = gameObject.GetComponent<Renderer>();
        color = newColor;
        rend.material.SetColor("_Color", color);
    }

    public Color GetColor()
    {
        return color;
    }

    void Change()
    {
        gameObject.GetComponent<Material>().SetColor("_Color", color);
    }
}