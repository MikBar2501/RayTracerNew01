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

        public static float GetBaryCentricCoord(Vector3 A, Vector3 B, Vector3 C, Vector3 I)
        {
            Vector3 AB = B - A;
            Vector3 CB = B - C;
            Vector3 AI = I - A;
            Vector3 V = AB - (CB * ((Vector3.Dot(CB, AB)) / (Vector3.Dot(CB, CB))));
            return 1 - (Vector3.Dot(V, AI) / Vector3.Dot(V, AB));
        }

        public override bool HitTest(Ray ray, out Vector3 hitPoint, out Vector3 normal)
        {
            Ray transformedRay = reverseTransform * ray;
            normal = this.normal;
            Vector3 fromCameraToVect1 = vertex0 - transformedRay.origin;
            hitPoint = transformedRay.origin + transformedRay.direction * ((Vector3.Dot(fromCameraToVect1, normal)) / (Vector3.Dot(transformedRay.direction, normal)));


            /*float v = GetBaryCentricCoord(vertex2, vertex1, vertex0, hitPoint);
            if (v > 1 || v < 0)
                return false;

            float w = GetBaryCentricCoord(vertex1, vertex0, vertex2, hitPoint);
            if (w > 1 || w < 0)
                return false;

            float u = GetBaryCentricCoord(vertex0, vertex2, vertex1, hitPoint);
            if (u > 1 || u < 0)
                return false;*/

            /*if(!PointIsInTriangle(vertex0, vertex1, vertex2, hitPoint))
            {
                return false;
            }*/

            Vector3 V0 = vertex1 - vertex0;
            Vector3 V1 = vertex2 - vertex0;
            Vector3 V2 = hitPoint - vertex0;

            float d00 = Vector3.Dot(V0, V0);
            float d01 = Vector3.Dot(V0, V1);
            float d11 = Vector3.Dot(V1, V1);
            float d20 = Vector3.Dot(V2, V0);
            float d21 = Vector3.Dot(V2, V1);
            float d = (d00 * d11) - (d01 * d01);
            float v = ((d11 * d20) - (d01 * d21)) / d;
            float w = ((d00 * d21) - (d01 * d20)) / d;
            float u = 1 - (v + w);

            if (u <= 1 && u >= 0)
            {
                if (v <= 1 && v >= 0)
                {
                    if (w <= 1 && w >= 0)
                    {
                        hitPoint = transform * hitPoint;
                        normal = transform.Inverse().Transpose() * normal;
                        normal = Vector3.Normalize(normal);
                        return true;
                    }
                }
            }


            return false;

            
        }

        bool PointIsInTriangle(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 point)
        {
            Vector3 V0 = vertex1 - vertex0;
            Vector3 V1 = vertex2 - vertex0;
            Vector3 V2 = point - vertex0;

            float d00 = Vector3.Dot(V0, V0);
            float d01 = Vector3.Dot(V0, V1);
            float d11 = Vector3.Dot(V1, V1);
            float d20 = Vector3.Dot(V2, V0);
            float d21 = Vector3.Dot(V2, V1);
            float d = (d00 * d11) - (d01 * d01);
            float v = ((d11 * d20) - (d01 * d21)) / d;
            float w = ((d00 * d21) - (d01 * d20)) / d;
            float u = 1 - (v + w);

            if(u <= 1 && u >= 0)
            {
                if (v <= 1 && v >= 0)
                {
                    if (w <= 1 && w >= 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
