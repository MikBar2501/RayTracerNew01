using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RayTracerNew
{
    class Lighting
    {
        public List<Lights> lights;

        public Lighting()
        {
            lights = new List<Lights>();
        }

        public ColorRGB PointColor(Vector3 cameraPose, Vector3 point, Material material, Vector3 normal, bool forceVisible = false)
        {
            ColorRGB ambient = material.ambient;
            ColorRGB emission = material.emission;

            ColorRGB outColor = ambient + emission;
            foreach (Lights light in lights)
            {
                int visible = 0;
                if (light.isVisible || forceVisible)
                    visible = 1;

                Vector3 lightDirection = light.GetDirection(point);
                Vector3 toCamera = Vector3.Normalize(cameraPose - point);
                Vector3 halfVector = Vector3.Normalize(lightDirection + toCamera);
                ColorRGB lightColor = light.GetLightColor(point);

                ColorRGB diffuse = material.diffuse * (Math.Max(0, Vector3.Dot(lightDirection, normal)));
                ColorRGB specular = material.specular * (float)Math.Pow(Math.Max(0, Vector3.Dot(halfVector, normal)), material.shininess);
                if (forceVisible)
                    specular /= 2;

                outColor += lightColor* visible *(diffuse + specular);
            }
            return outColor;
        }
    }
}
