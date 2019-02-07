using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RayTracerNew
{
    class PointLight : Light
    {
        public PointLight()
        {
            if (constant == 0)
                constant = 1;

            position = new Vector3();
        }

        public Vector3 position;
        public static float constant;
        public static float linear;
        public static float quadratic;

        public override Vector3 GetPosition()
        {
            return position;
        }

        public override Vector3 GetDirection(Vector3 pose)
        {
            return Vector3.Normalize(position - pose);
        }

        public override ColorRGB GetLightColor(Vector3 point)
        {
            float distance = Magnitude(position - point);
            return color * (1 / (constant + linear * distance + distance * distance * quadratic));
        }

        public float Magnitude(Vector3 vector)
        {
            return (float)Math.Sqrt(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2) + Math.Pow(vector.Z, 2));
        }
    }
}
