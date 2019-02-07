using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Numerics;

namespace RayTracerNew
{
    class Camera
    {
        public int imageWidth;
        public int imageHeight;

        public Vector3 position;
        Vector3 lookDir;

        Vector3 center;

        float lookAngleX;
        float lookAngleY;

        Vector3 u, v, w;

        public void SetSize(int width, int height)
        {
            this.imageWidth = width;
            this.imageHeight = height + 1;
            center = new Vector3(width / 2, height / 2, 0);
        }

        public void SetTransform(Vector3 position, Vector3 LookAtPoint, Vector3 up, float lookAngleY)
        {
            this.position = position;
            lookDir = LookAtPoint - position;
            //this.lookAngleY = Raytracer.DegreeToRadian(lookAngleY);
            this.lookAngleY = (float)(lookAngleY * Math.PI / (float)180);
            Debug.WriteLine("Look angle y " + this.lookAngleY);
            lookAngleX = 2 * (float)Math.Atan((float)imageWidth / imageHeight * Math.Tan(this.lookAngleY / 2));
            Debug.WriteLine("Look angle x " + lookAngleX);

            w = Vector3.Normalize(position - LookAtPoint);
            u = Vector3.Normalize(Vector3.Cross(up, w));
            v = Vector3.Cross(w, u);
        }



        Vector3 GetLookDirForAngle(float alpha, float beta)
        {
            return Vector3.Normalize(u * alpha + v * beta - w);
        }

        public Camera()
        {
            this.imageWidth = 500;
            this.imageHeight = 500;
            this.position = new Vector3();
            lookDir = new Vector3(0, 0, 1);
            center = new Vector3(imageWidth / 2, imageHeight / 2, 0);
        }

        public Camera(int width, int height, Vector3 position, Vector3 LookAtPoint)
        {
            this.imageWidth = width;
            this.imageHeight = height;
            this.position = position;
            lookDir = LookAtPoint - position;
            center = new Vector3(width / 2, height / 2, 0);
        }

        public Ray GetRay(int x, int y)
        {
            float alpha = (x - imageWidth / 2.0f) / (imageWidth / 2.0f) * (float)Math.Tan(lookAngleX / 2.0f);
            float beta = (y - imageHeight / 2.0f) / (imageHeight / 2.0f) * (float)Math.Tan(lookAngleY / 2.0f);

            Vector3 curLookDir = GetLookDirForAngle(alpha, beta);

            return new Ray(position, curLookDir);
        }
    }
}
