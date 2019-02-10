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
        public Vector3 vertex0;
        public Vector3 vertex1;
        public Vector3 vertex2;

        public Vector3 normal;

        public Triangle(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2)
        {
            this.vertex0 = vertex0;
            this.vertex1 = vertex1;
            this.vertex2 = vertex2;
            normal = Vector3.Normalize(Vector3.Cross(vertex1 - vertex0, vertex2 - vertex0));
        }

        /*public override bool HitTest(Ray ray, out Vector3 hitPoint, out Vector3 normal)
        {
            Ray transformedRay = reverseTransform * ray;
            normal = this.normal;
            //Vector3 fromCameraToVect1 = vertex0 - ray.origin;
            //hitPoint = ray.origin + ray.direction * ((Vector3.Dot(fromCameraToVect1, normal)) / (Vector3.Dot(ray.direction, normal)));
            hitPoint = GetHitPoint(transformedRay);

            //GetBaryCentricCoord(Vector3 A, Vector3 B, Vector3 C, Vector3 I)
            //float bery1 = GetBaryCentricCoord(vertex2, vertex1, vertex0, hitPoint);
            //float bery2 = GetBaryCentricCoord(vertex1, vertex0, vertex2, hitPoint);
            //float bery3 = GetBaryCentricCoord(vertex0, vertex2, vertex1, hitPoint);

            //Vector3 AB = B - A;
            //Vector3 CB = B - C;
            //Vector3 AI = I - A;
            //Vector3 V = AB - Projection(AB, CB);

            Vector3 V0 = vertex1 - vertex2;
            Vector3 V1 = vertex1 - vertex0;
            Vector3 V2 = hitPoint - vertex2;
            //Vector3 V3 = V0 - Projection(V0, V1);
            Vector3 V3 = V1 * (Vector3.Dot(V1, V0)) / (Vector3.Dot(V1, V1));
            float v = 1 - (Vector3.Dot(V3, V2) / Vector3.Dot(V3, V0));

            Vector3 W0 = vertex0 - vertex1;
            Vector3 W1 = vertex0 - vertex2;
            Vector3 W2 = hitPoint - vertex1;
            Vector3 W3 = W1 * (Vector3.Dot(W1, W0)) / (Vector3.Dot(W1, W1));
            float w = 1 - (Vector3.Dot(W3, W2) / Vector3.Dot(W3, W0));

            Vector3 U0 = vertex2 - vertex0;
            Vector3 U1 = vertex2 - vertex1;
            Vector3 U2 = hitPoint - vertex0;
            Vector3 U3 = U1 * (Vector3.Dot(U1, U0)) / (Vector3.Dot(U1, U1));
            float u = 1 - (Vector3.Dot(U3, U2) / Vector3.Dot(U3, U0));




            if (u*v*w > 1 || u*v*w < 0)
            {
                return false;
            }

            hitPoint = transform * hitPoint;
            normal = transform.Inverse().Transpose() * normal;
            normal = Vector3.Normalize(normal);
            return true;
        }*/

        public static float GetBaryCentricCoord(Vector3 A, Vector3 B, Vector3 C, Vector3 I)
        {
            Vector3 AB = B - A;
            Vector3 CB = B - C;
            Vector3 AI = I - A;
            Vector3 V = AB - Projection(AB, CB);
            return 1 - (Vector3.Dot(V, AI) / Vector3.Dot(V, AB));
        }

        public Vector3 GetHitPoint(Ray ray)
        {
            Vector3 fromCameraToVect1 = vertex0 - ray.origin;
            return ray.origin + ray.direction * ((Vector3.Dot(fromCameraToVect1, normal)) / (Vector3.Dot(ray.direction, normal)));
        }

        public static Vector3 Projection(Vector3 vecTo, Vector3 vecOn)
        {
            return vecOn * ((Vector3.Dot(vecOn, vecTo)) / (Vector3.Dot(vecOn, vecOn)));
        }

        public override bool HitTest(Ray ray, out Vector3 hitPoint, out Vector3 normal)
        {
            Ray transformedRay = reverseTransform * ray;
            normal = this.normal;
            //Vector3 fromCameraToVect1 = vertex0 - ray.origin;
            //hitPoint = ray.origin + ray.direction * ((Vector3.Dot(fromCameraToVect1, normal)) / (Vector3.Dot(ray.direction, normal)));
            hitPoint = GetHitPoint(transformedRay);


            float bery1 = GetBaryCentricCoord(vertex2, vertex1, vertex0, hitPoint);
            if (bery1 > 1 || bery1 < 0)
                return false;

            float bery2 = GetBaryCentricCoord(vertex1, vertex0, vertex2, hitPoint);
            if (bery2 > 1 || bery2 < 0)
                return false;

            float bery3 = GetBaryCentricCoord(vertex0, vertex2, vertex1, hitPoint);
            if (bery3 > 1 || bery3 < 0)
                return false;

            hitPoint = transform * hitPoint;
            normal = transform.Inverse().Transpose() * normal;
            normal = Vector3.Normalize(normal);
            return true;
        }
    }
}
