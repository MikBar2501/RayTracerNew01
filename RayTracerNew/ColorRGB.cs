using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Numerics;

namespace RayTracerNew
{
    class ColorRGB
    {
        public int r;
        public int g;
        public int b;

        public static ColorRGB Black = new ColorRGB(0, 0, 0);

        public ColorRGB(Color color)
        {
            r = color.R;
            g = color.G;
            b = color.B;
        }

        public ColorRGB(int R, int G, int B)
        {
            r = R;
            g = G;
            b = B;
        }

        public Vector3 ChangeToVector()
        {
            return new Vector3((float)r / 255, (float)g / 255, (float)b / 255);
        }

        public Color ChangeToDrawningColor()
        {
            int r = Math.Min(255, this.r);
            int g = Math.Min(255, this.g);
            int b = Math.Min(255, this.b);
            return Color.FromArgb(r, g, b);
        }

        public static ColorRGB operator *(ColorRGB color1, ColorRGB color2)
        {
            Vector3 colorVector1 = color1.ChangeToVector();
            Vector3 colorVector2 = color2.ChangeToVector();

            Vector3 newVector = colorVector1 * colorVector2;

            return ToMyColor(newVector);
        }



        public static ColorRGB operator *(ColorRGB color, float value)
        {
            Vector3 colorVector = color.ChangeToVector();
            Vector3 newVector = colorVector * value;

            return ToMyColor(newVector);
        }

        public static ColorRGB operator /(ColorRGB color, float value)
        {
            Vector3 colorVector = color.ChangeToVector();
            Vector3 newVector = colorVector / value;

            return ToMyColor(newVector);
        }

        public static ColorRGB operator +(ColorRGB color1, ColorRGB color2)
        {
            return new ColorRGB(color1.r + color2.r, color1.g + color2.g, color1.b + color2.b);
        }

        public static ColorRGB ToMyColor(Vector3 vector)
        {
            return new ColorRGB(Math.Min((int)(vector.X * 255), 255), Math.Min((int)(vector.Y * 255), 255), Math.Min((int)(vector.Z * 255), 255));
        }
    }
}
