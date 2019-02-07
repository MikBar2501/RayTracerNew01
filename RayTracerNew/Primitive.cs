using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracerNew
{
    class GeometricObject
    {
        public Material material;
        public Matrix transform;
        public Matrix reverseTransform;

        public virtual bool HitTest(Ray ray, out Vector3 hitPoint, out Vector3 normal)
        {
            hitPoint = new Vector3();
            normal = new Vector3();
            return false;
        }
    }
}
