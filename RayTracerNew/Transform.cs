using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RayTracerNew
{
    class Transform
    {
        public Matrix transform;

        public List<Matrix> transList;

        public Transform(Transform trans)
        {
            transform = new Matrix();
            transList = new List<Matrix>();
            foreach (Matrix tran in trans.transList)
            {
                transList.Add(tran);
            }
        }

        public Transform()
        {
            transform = new Matrix();
            transList = new List<Matrix>();
        }

        public Matrix GetMatrix()
        {
            transform = new Matrix();
            for (int i = transList.Count - 1; i >= 0; i--)
            {
                transform = transList[i] * transform;
            }
            return transform;
        }
    }
}
