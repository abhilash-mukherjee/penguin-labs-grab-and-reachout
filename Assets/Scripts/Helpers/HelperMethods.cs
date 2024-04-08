using UnityEngine;

public static class HelperMethods
{
    public static Color ParseHexStringToColor(string colorString)
    {
        // Ensure the color string starts with '#' for Unity's TryParseHtmlString
        if (!colorString.StartsWith("#"))
        {
            colorString = "#" + colorString;
        }

        Color color;
        if (ColorUtility.TryParseHtmlString(colorString, out color))
        {
            // If the string is a valid hex color, return the Color object
            return color;
        }
        else
        {
            // If the string is not a valid hex color, return white as a default or error color
            Debug.LogWarning("Invalid color string: " + colorString + ". Returning white.");
            return Color.white;
        }
    }

    public static Material[] GetMaterialArrayFromColors(Color[] colors)
    {


        Material[] materials = new Material[colors.Length];

        for(int i = 0; i < colors.Length; i++)
        {
            Shader urpShader = Shader.Find("Universal Render Pipeline/Lit");
            if (urpShader == null)
            {
                Debug.LogError("URP Lit shader not found. Make sure you're using URP.");
                return new Material[0];
            }
            Material material = new Material(urpShader);
            material.color = colors[i];
            materials[i] = material;
        }

       return materials;
    }
}