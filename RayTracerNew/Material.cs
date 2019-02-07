using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracerNew
{
    class Material
    {
        public ColorRGB ambient;
        public ColorRGB diffuse;
        public ColorRGB specular;
        public ColorRGB emission;
        public float shininess;

        public Material()
        {
            ambient = ColorRGB.Black;
            diffuse = ColorRGB.Black;
            specular = ColorRGB.Black;
            emission = ColorRGB.Black;
            shininess = 0;
        }

        public Material(Material material)
        {
            ambient = material.ambient;
            diffuse = material.diffuse;
            specular = material.specular;
            emission = material.emission;
            shininess = material.shininess;
        }
    }
}
