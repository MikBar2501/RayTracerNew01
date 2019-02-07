using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RayTracerNew
{
    class Triangle : GeometricObject
    {
        public Vector3 vert1;
        public Vector3 vert2;
        public Vector3 vert3;

        public Vector3 normal;

        public Triangle(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            vert1 = v1;
            vert2 = v2;
            vert3 = v3;
        }

        public override bool HitTest(Ray ray, out Vector3 hitPoint, out Vector3 normal)
        {
            Ray transformedRay = reverseTransform * ray;

            hitPoint = GetHitPoint(transformedRay);
            normal = this.normal;
            //hitPoint = transform.Reverse() * hitPoint;

            float bery1 = GetBaryCentricCoord(vert3, vert2, vert1, hitPoint);
            if (bery1 > 1 || bery1 < 0)
                return false;

            float bery2 = GetBaryCentricCoord(vert2, vert1, vert3, hitPoint);
            if (bery2 > 1 || bery2 < 0)
                return false;

            float bery3 = GetBaryCentricCoord(vert1, vert3, vert2, hitPoint);
            if (bery3 > 1 || bery3 < 0)
                return false;

            hitPoint = transform * hitPoint;
            normal = transform.Inverse().Transpose() * normal;
            normal = Vector3.Normalize(normal);
            return true;
        }

        public static float GetBaryCentricCoord(Vector3 A, Vector3 B, Vector3 C, Vector3 I)
        {
            Vector3 AB = B - A;
            Vector3 CB = B - C;
            Vector3 AI = I - A;
            Vector3 V = AB - Projection(AB, CB);
            return 1 - (Vector3.Dot(V, AI) / Vector3.Dot(V, AB));
        }

        public Vector3 Normal()
        {
            return Vector3.Normalize(Vector3.Cross(vert2 - vert1, vert3 - vert1));
        }

        public Vector3 GetHitPoint(Ray ray)
        {
            Vector3 fromCameraToVect1 = vert1 - ray.origin;
            return ray.origin + ray.direction
                * ((Vector3.Dot(fromCameraToVect1, normal))
                / (Vector3.Dot(ray.direction, normal)));
        }

        public static Vector3 Projection(Vector3 vecTo, Vector3 vecOn)
        {
            return vecOn * ((Vector3.Dot(vecOn, vecTo)) / (Vector3.Dot(vecOn, vecOn)));
        }
    }
}
