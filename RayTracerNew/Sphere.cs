using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RayTracerNew
{
    class Sphere : GeometricObject
    {
        public Vector3 center;
        public float radius;

        public Sphere(Vector3 c, float r)
        {
            center = c;
            radius = r;
        }

        public override bool HitTest(Ray ray, out Vector3 hitPoint, out Vector3 normal)
        {
            Ray transformedRay = reverseTransform * ray;
            Vector3 distance = transformedRay.origin - center;
            float a = Vector3.Dot(transformedRay.direction, transformedRay.direction);
            float b = Vector3.Dot(transformedRay.direction * 2, distance);
            float c = Vector3.Dot(distance, distance) - (radius * radius);
            float delta = b * b - 4 * a * c;
            if (delta <= 0)
            {
                hitPoint = new Vector3();
                normal = new Vector3();
                return false;
            }
            else
            {
                float deltaSqrt = (float)Math.Sqrt(delta);
                float x1 = (-b - deltaSqrt) / (2 * a);
                float x2 = (-b + deltaSqrt) / (2 * a);

                float x = x1;
                if (x1 > 0 && x2 > 0)
                    x = x1 > x2 ? x2 : x1;

                if ((x1 < 0 && x2 > 0) || (x1 > 0 && x2 < 0))
                    x = x1 < x2 ? x2 : x1;


                hitPoint = transformedRay.origin + transformedRay.direction * x;
                normal = Vector3.Normalize((hitPoint - center));

                hitPoint = transform * hitPoint;
                normal = Vector3.Normalize(transform.Inverse().Transpose() * normal);
                //normal = Vector3.Normalize(normal);
                //hitPoint = transform.Reverse() * hitPoint;
                //if (Vector3.Dot(hitPoint - center, hitPoint - center) - radius * radius != 0)
                //    return false;
                return true;
            }
        }
    }
}
