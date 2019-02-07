using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RayTracerNew
{
    class Light
    {
        public ColorRGB color;
        public bool visible;

        public virtual Vector3 GetDirection(Vector3 position)
        {
            return new Vector3();
        }

        public virtual ColorRGB GetLightColor(Vector3 point)
        {
            return color;
        }

        public virtual Vector3 GetPosition()
        {
            return new Vector3();
        }
    }
}
