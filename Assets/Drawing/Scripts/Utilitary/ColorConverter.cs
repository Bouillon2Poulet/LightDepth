using UnityEngine;

public static class ColorConverter
{
    public static void RGBToHSL(Color rgb, out float h, out float s, out float l)
    {
        float r = rgb.r;
        float g = rgb.g;
        float b = rgb.b;

        float max = Mathf.Max(r, Mathf.Max(g, b));
        float min = Mathf.Min(r, Mathf.Min(g, b));

        // Calcul de la luminosité (l)
        l = (max + min) / 2f;

        // Si max est égal à min, la couleur est une nuance de gris et la saturation est de 0
        if (Mathf.Approximately(max, min))
        {
            h = s = 0f;
        }
        else
        {
            // Calcul de la saturation (s)
            s = l > 0.5f ? (max - min) / (2.0f - max - min) : (max - min) / (max + min);

            // Calcul de la teinte (h)
            if (max == r)
                h = (g - b) / (max - min) + (g < b ? 6f : 0f);
            else if (max == g)
                h = (b - r) / (max - min) + 2f;
            else
                h = (r - g) / (max - min) + 4f;

            h /= 6f;
        }
    }
}
