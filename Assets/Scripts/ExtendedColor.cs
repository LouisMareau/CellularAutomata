using UnityEngine;

public class ExtendedColor
{
    public static int red;      // Red value of the color
    public static int green;    // Green value of the color
    public static int blue;     // Blue value of the color
    public static int alpha;    // Alpha value of the color 

    public static Color Red = RGB(255,0,0); 
    public static Color Green = RGB(0,255,0);
    public static Color Blue = RGB(0,0,255);

    public static Color RGB (int r, int g, int b)
    {
        red = r;
        green = g;
        blue = b;

        return new Color((float)r/255, (float)g/255, (float)b/255);
    }

    public static Color RGBA (int r, int g, int b, int a)
    {
        red = r;
        green = g;
        blue = b;
        alpha = a;

        return new Color((float)r/255, (float)g/255, (float)b/255, (float)a/100);
    }

    public static Color HEX (string h)
    {
        if (h.Contains("#"))
        {
            // We start (or remove) the '#' to only keep the hexadecimal values
            h = h.Substring(1);
        } 
        
        if (h.Length == 6)
        {
            int h1 = int.Parse(h.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
            int h2 = int.Parse(h.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
            int h3 = int.Parse(h.Substring(4,2), System.Globalization.NumberStyles.HexNumber);

            return RGB(h1, h2, h3);
        }
        else
        {
            Debug.LogError("Wrong hexadecimal value entered. The function has exited with an error.");
            throw new ExitGUIException();
        }
    }
}