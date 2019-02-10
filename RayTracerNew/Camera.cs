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
        Vector3 lookDirection;
        Vector3 centerPoint;

        float fovX;
        float fovY;

        Vector3 u;
        Vector3 v;
        Vector3 w;

        public Camera()
        {
            this.imageWidth = 512;
            this.imageHeight = 512;
            this.position = new Vector3();
            lookDirection = new Vector3(0, 0, 1);
            centerPoint = new Vector3(imageWidth / 2, imageHeight / 2, 0);
        }

        public void SetSize(int imageWidth, int imageHeight)
        {
            this.imageWidth = imageWidth;
            this.imageHeight = imageHeight + 1;
            centerPoint = new Vector3(imageWidth / 2, imageHeight / 2, 0);
        }

        public void SetTransform(Vector3 position, Vector3 lookPoint, Vector3 up, float fovY)
        {
            this.position = position;
            lookDirection = lookPoint - position;
            this.fovY = (float)(fovY * Math.PI / (float)180);
            fovX = 2 * (float)Math.Atan((float)imageWidth / imageHeight * Math.Tan(this.fovY / 2));

            w = Vector3.Normalize(position - lookPoint);
            u = Vector3.Normalize(Vector3.Cross(up, w));
            v = Vector3.Cross(w, u);
        }

        

        public Camera(int imageWidth, int imageHeight, Vector3 position, Vector3 lookPoint)
        {
            this.imageWidth = imageWidth;
            this.imageHeight = imageHeight;
            this.position = position;
            lookDirection = lookPoint - position;
            centerPoint = new Vector3(imageWidth / 2, imageHeight / 2, 0);
        }

        public Ray GetRay(int x, int y)
        {
            float alpha = (x - imageWidth / 2.0f) / (imageWidth / 2.0f) * (float)Math.Tan(fovX / 2.0f);
            float beta = (y - imageHeight / 2.0f) / (imageHeight / 2.0f) * (float)Math.Tan(fovY / 2.0f);

            Vector3 newLookDirection = Vector3.Normalize(u * alpha + v * beta - w);

            return new Ray(position, newLookDirection);
        }
    }
}
