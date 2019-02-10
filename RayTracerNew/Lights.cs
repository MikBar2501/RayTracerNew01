using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RayTracerNew
{
    class Lights
    {
        public ColorRGB lightColor;
        public Vector3 dirPose;
        public bool isVisible;
        bool isPoint;

        public static float constant;
        public static float linear;
        public static float quadratic;

        public Lights()
        {
            lightColor = new ColorRGB(255,255,255);
            dirPose = new Vector3();
            isVisible = true;
            isPoint = true;
        }

        public Lights(ColorRGB lightColor, Vector3 dirPose, bool isPoint)
        {
            this.lightColor = lightColor;
            this.dirPose = dirPose;
            this.isPoint = isPoint;
        }

        public Vector3 GetDirection(Vector3 pose)
        {
            if (isPoint)
            {
                return Vector3.Normalize(dirPose - pose);
            } else
            {
                return dirPose;
            }
        }

        public ColorRGB GetLightColor(Vector3 point)
        {
            if (isPoint)
            {
                float distance = (float)Math.Sqrt(Math.Pow((dirPose - point).X, 2) + Math.Pow((dirPose - point).Y, 2) + Math.Pow((dirPose - point).Z, 2));
                return lightColor * (1 / (constant + linear * distance + (float)Math.Pow(distance, 2) * quadratic));
            }
            else
            {
                return lightColor;
            }
        }
         
        public Vector3 GetPosition()
        {
            if (isPoint)
            {
                return dirPose;
            }
            else
            {
                return (dirPose * -1000);
            }
        }

        public void setPoint(bool isPoint)
        {
            this.isPoint = isPoint;
        }
    }
}
