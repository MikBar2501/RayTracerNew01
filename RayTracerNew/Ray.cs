using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RayTracerNew
{
    class Ray
    {
        public Vector3 origin;
        public Vector3 direction;
        public Ray(Vector3 o, Vector3 d)
        {
            origin = o;
            direction = d;
        }
    }
}
