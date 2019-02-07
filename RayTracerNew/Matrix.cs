﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RayTracerNew
{
    class Matrix
    {
        public int rows;
        public int columns;
        public float[,] grid;

        public Matrix(Matrix mat)
        {
            rows = mat.rows;
            columns = mat.columns;

            grid = new float[rows, columns];

            for (int x = 0; x < rows; x++)
                for (int y = 0; y < columns; y++)
                {
                    grid[x, y] = mat.grid[x, y];
                }
        }

        public Matrix()
        {
            rows = 4;
            columns = 4;
            grid = new float[4, 4];

            grid[0, 0] = 1;
            grid[0, 1] = 0;
            grid[0, 2] = 0;
            grid[0, 3] = 0;

            grid[1, 0] = 0;
            grid[1, 1] = 1;
            grid[1, 2] = 0;
            grid[1, 3] = 0;

            grid[2, 0] = 0;
            grid[2, 1] = 0;
            grid[2, 2] = 1;
            grid[2, 3] = 0;

            grid[3, 0] = 0;
            grid[3, 1] = 0;
            grid[3, 2] = 0;
            grid[3, 3] = 1;
        }

        public Matrix(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;

            grid = new float[rows, columns];
        }

        public Matrix(
            float g00, float g01, float g02, float g03,
            float g10, float g11, float g12, float g13,
            float g20, float g21, float g22, float g23,
            float g30, float g31, float g32, float g33)
        {
            rows = 4;
            columns = 4;
            grid = new float[4, 4];

            grid[0, 0] = g00;
            grid[0, 1] = g01;
            grid[0, 2] = g02;
            grid[0, 3] = g03;

            grid[1, 0] = g10;
            grid[1, 1] = g11;
            grid[1, 2] = g12;
            grid[1, 3] = g13;

            grid[2, 0] = g20;
            grid[2, 1] = g21;
            grid[2, 2] = g22;
            grid[2, 3] = g23;

            grid[3, 0] = g30;
            grid[3, 1] = g31;
            grid[3, 2] = g32;
            grid[3, 3] = g33;
        }

        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            //Debug.WriteLine("m1 r-" + m1.rows + " c-" + m1.columns);
            //Debug.WriteLine("m2 r-" + m2.rows + " c-" + m2.columns);

            Matrix matrix = new Matrix(m1.rows, m2.columns);

            //Debug.WriteLine("matix r-" + matrix.rows + " c-" + matrix.columns);

            for (int x = 0; x < matrix.rows; x++)
                for (int y = 0; y < matrix.columns; y++)
                {
                    //Debug.WriteLine("x-" + x + " y-" + y);

                    float total = 0;
                    for (int i = 0; i < m1.columns; i++)
                    {
                        //Debug.WriteLine("i-" + i);
                        total += m1.grid[x, i] * m2.grid[i, y];
                    }
                    matrix.grid[x, y] = total;
                }

            return matrix;
        }

        public Vector3 ToVector()
        {
            return new Vector3(grid[0, 0], grid[1, 0], grid[2, 0]);
        }

        public static Vector3 operator *(Matrix m, Vector3 v)
        {
            Matrix vm = new Matrix(4, 1);
            vm.grid[0, 0] = v.X;
            vm.grid[1, 0] = v.Y;
            vm.grid[2, 0] = v.Z;
            vm.grid[3, 0] = 1;

            Matrix matNew = m * vm;
            float w = matNew.grid[3, 0];

            Vector3 vecNew = new Vector3(matNew.grid[0, 0], matNew.grid[1, 0], matNew.grid[2, 0]);

            return vecNew;
        }

        public static Matrix operator *(Matrix m, float v)
        {
            Matrix matrix = new Matrix(m.rows, m.columns);

            for (int x = 0; x < matrix.rows; x++)
                for (int y = 0; y < matrix.columns; y++)
                {
                    matrix.grid[x, y] = m.grid[x, y] * v;
                }

            return matrix;
        }

        public static Ray operator *(Matrix m, Ray r)
        {
            Vector3 pose = m * r.origin;

            Matrix m3 = new Matrix(m);

            for (int x = 0; x < m.rows; x++)
            {
                m3.grid[x, 3] = 0;
            }

            for (int y = 0; y < m.columns; y++)
            {
                m3.grid[3, y] = 0;
            }

            Vector3 dir = Vector3.Normalize(m3 * r.direction);

            return new Ray(pose, dir);
        }

        public static Matrix GetTranslateMatrix(Vector3 tran)
        {
            //tran = tran * 0.125f;

            return new Matrix(
                1, 0, 0, tran.X,
                0, 1, 0, tran.Y,
                0, 0, 1, tran.Z,
                0, 0, 0, 1);
        }

        public static Matrix GetScaleMatrix(Vector3 scale)
        {
            return new Matrix(
                scale.X, 0, 0, 0,
                0, scale.Y, 0, 0,
                0, 0, scale.Z, 0,
                0, 0, 0, 1);
        }

        public static Matrix GetRotationXMatrix(float angle)
        {
            angle = DegreeToRadian(angle);

            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);

            return new Matrix(
                1, 0, 0, 0,
                0, cos, -sin, 0,
                0, sin, cos, 0,
                0, 0, 0, 1);
        }

        public static Matrix GetRotationYMatrix(float angle)
        {
            angle = DegreeToRadian(angle);

            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);

            return new Matrix(
                cos, 0, sin, 0,
                0, 1, 0, 0,
                -sin, 0, cos, 0,
                0, 0, 0, 1);
        }

        public static Matrix GetRotationZMatrix(float angle)
        {
            angle = DegreeToRadian(angle);

            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);

            return new Matrix(
                cos, -sin, 0, 0,
                sin, cos, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1);
        }


        public static float DegreeToRadian(float degree)
        {
            return (float)(degree * Math.PI / (float)180);
        }

        public Matrix Reverse()
        {
            Matrix matrix = new Matrix(rows, columns);

            float a11 = grid[0, 0];
            float a12 = grid[0, 1];
            float a13 = grid[0, 2];
            float a14 = grid[0, 3];

            float a21 = grid[1, 0];
            float a22 = grid[1, 1];
            float a23 = grid[1, 2];
            float a24 = grid[1, 3];

            float a31 = grid[2, 0];
            float a32 = grid[2, 1];
            float a33 = grid[2, 2];
            float a34 = grid[2, 3];

            float a41 = grid[3, 0];
            float a42 = grid[3, 1];
            float a43 = grid[3, 2];
            float a44 = grid[3, 3];

            float det = 0;

            float line = 1;
            for (int y = 0; y < columns; y++)
            {
                line = 1;
                for (int x = 0; x < rows; x++)
                {
                    int localX = x;
                    int localY = x + y;
                    if (localY >= columns)
                        localY -= columns;

                    line *= grid[localX, localY];
                }
                det += line;
            }

            for (int y = 0; y < columns; y++)
            {
                line = 1;
                for (int x = rows - 1; x >= 0; x--)
                {
                    int localX = x;
                    int localY = (rows - x - 1) + y;
                    if (localY >= columns)
                        localY -= columns;

                    line *= grid[localX, localY];
                }
                det -= line;
            }



            float b11 = a22 * a33 * a44 + a23 * a34 * a42 + a24 * a32 * a43 - a22 * a34 * a43 - a23 * a32 * a44 - a24 * a33 * a42;

            float b12 = a12 * a34 * a43 + a13 * a32 * a44 + a14 * a33 * a42 - a12 * a33 * a44 - a13 * a34 * a42 - a14 * a32 * a43;

            float b13 = a12 * a23 * a44 + a13 * a24 * a42 + a14 * a22 * a43 - a12 * a24 * a43 - a13 * a22 * a44 - a14 * a23 * a42;

            float b14 = a12 * a24 * a33 + a13 * a22 * a34 + a14 * a23 * a32 - a12 * a23 * a34 - a13 * a24 * a32 - a14 * a22 * a33;

            float b21 = a21 * a34 * a43 + a23 * a31 * a44 + a24 * a33 * a41 - a21 * a33 * a44 - a23 * a34 * a41 - a24 * a31 * a43;

            float b22 = a11 * a33 * a44 + a13 * a34 * a41 + a14 * a31 * a43 - a11 * a34 * a43 - a13 * a31 * a44 - a14 * a33 * a41;

            float b23 = a11 * a24 * a43 + a13 * a21 * a44 + a14 * a23 * a41 - a11 * a23 * a44 - a13 * a24 * a41 - a14 * a21 * a43;

            float b24 = a11 * a23 * a34 + a13 * a24 * a31 + a14 * a21 * a33 - a11 * a24 * a33 - a13 * a21 * a34 - a14 * a23 * a31;

            float b31 = a21 * a32 * a44 + a22 * a34 * a41 + a24 * a31 * a42 - a21 * a34 * a42 - a22 * a31 * a44 - a24 * a32 * a41;

            float b32 = a11 * a34 * a42 + a12 * a31 * a44 + a14 * a32 * a41 - a11 * a32 * a44 - a12 * a34 * a41 - a14 * a31 * a42;

            float b33 = a11 * a22 * a44 + a12 * a24 * a41 + a14 * a21 * a42 - a11 * a24 * a42 - a12 * a21 * a44 - a14 * a22 * a41;

            float b34 = a11 * a24 * a32 + a12 * a21 * a34 + a14 * a22 * a31 - a11 * a22 * a34 - a12 * a24 * a31 - a14 * a21 * a32;

            float b41 = a21 * a33 * a42 + a22 * a31 * a43 + a23 * a32 * a41 - a21 * a32 * a43 - a22 * a33 * a41 - a23 * a31 * a42;

            float b42 = a11 * a32 * a43 + a12 * a33 * a41 + a13 * a31 * a42 - a11 * a33 * a42 - a12 * a31 * a43 - a13 * a32 * a41;

            float b43 = a11 * a23 * a42 + a12 * a21 * a43 + a13 * a22 * a41 - a11 * a22 * a43 - a12 * a23 * a41 - a13 * a21 * a42;

            float b44 = a11 * a22 * a33 + a12 * a23 * a31 + a13 * a21 * a32 - a11 * a23 * a32 - a12 * a21 * a33 - a13 * a22 * a31;

            //Debug.WriteLine("det-" + det);

            matrix = new Matrix(
                b11, b12, b13, b14,
                b21, b22, b23, b24,
                b31, b32, b33, b34,
                b41, b42, b43, b44) * (1 / det);

            return matrix;
        }

        public Matrix Transpose()
        {
            Matrix matrix = new Matrix(columns, rows);

            for (int x = 0; x < matrix.rows; x++)
                for (int y = 0; y < matrix.columns; y++)
                {
                    matrix.grid[x, y] = grid[y, x];
                }

            return matrix;
        }

        public Matrix Inverse()
        {
            float[] m = new float[16];
            for (int x = 0; x < rows; x++)
                for (int y = 0; y < columns; y++)
                {
                    m[x + 4 * y] = grid[x, y];
                }

            float[] inv = new float[16];
            float det;
            int i;

            inv[0] = m[5] * m[10] * m[15] -
                     m[5] * m[11] * m[14] -
                     m[9] * m[6] * m[15] +
                     m[9] * m[7] * m[14] +
                     m[13] * m[6] * m[11] -
                     m[13] * m[7] * m[10];

            inv[4] = -m[4] * m[10] * m[15] +
                      m[4] * m[11] * m[14] +
                      m[8] * m[6] * m[15] -
                      m[8] * m[7] * m[14] -
                      m[12] * m[6] * m[11] +
                      m[12] * m[7] * m[10];

            inv[8] = m[4] * m[9] * m[15] -
                     m[4] * m[11] * m[13] -
                     m[8] * m[5] * m[15] +
                     m[8] * m[7] * m[13] +
                     m[12] * m[5] * m[11] -
                     m[12] * m[7] * m[9];

            inv[12] = -m[4] * m[9] * m[14] +
                       m[4] * m[10] * m[13] +
                       m[8] * m[5] * m[14] -
                       m[8] * m[6] * m[13] -
                       m[12] * m[5] * m[10] +
                       m[12] * m[6] * m[9];

            inv[1] = -m[1] * m[10] * m[15] +
                      m[1] * m[11] * m[14] +
                      m[9] * m[2] * m[15] -
                      m[9] * m[3] * m[14] -
                      m[13] * m[2] * m[11] +
                      m[13] * m[3] * m[10];

            inv[5] = m[0] * m[10] * m[15] -
                     m[0] * m[11] * m[14] -
                     m[8] * m[2] * m[15] +
                     m[8] * m[3] * m[14] +
                     m[12] * m[2] * m[11] -
                     m[12] * m[3] * m[10];

            inv[9] = -m[0] * m[9] * m[15] +
                      m[0] * m[11] * m[13] +
                      m[8] * m[1] * m[15] -
                      m[8] * m[3] * m[13] -
                      m[12] * m[1] * m[11] +
                      m[12] * m[3] * m[9];

            inv[13] = m[0] * m[9] * m[14] -
                      m[0] * m[10] * m[13] -
                      m[8] * m[1] * m[14] +
                      m[8] * m[2] * m[13] +
                      m[12] * m[1] * m[10] -
                      m[12] * m[2] * m[9];

            inv[2] = m[1] * m[6] * m[15] -
                     m[1] * m[7] * m[14] -
                     m[5] * m[2] * m[15] +
                     m[5] * m[3] * m[14] +
                     m[13] * m[2] * m[7] -
                     m[13] * m[3] * m[6];

            inv[6] = -m[0] * m[6] * m[15] +
                      m[0] * m[7] * m[14] +
                      m[4] * m[2] * m[15] -
                      m[4] * m[3] * m[14] -
                      m[12] * m[2] * m[7] +
                      m[12] * m[3] * m[6];

            inv[10] = m[0] * m[5] * m[15] -
                      m[0] * m[7] * m[13] -
                      m[4] * m[1] * m[15] +
                      m[4] * m[3] * m[13] +
                      m[12] * m[1] * m[7] -
                      m[12] * m[3] * m[5];

            inv[14] = -m[0] * m[5] * m[14] +
                       m[0] * m[6] * m[13] +
                       m[4] * m[1] * m[14] -
                       m[4] * m[2] * m[13] -
                       m[12] * m[1] * m[6] +
                       m[12] * m[2] * m[5];

            inv[3] = -m[1] * m[6] * m[11] +
                      m[1] * m[7] * m[10] +
                      m[5] * m[2] * m[11] -
                      m[5] * m[3] * m[10] -
                      m[9] * m[2] * m[7] +
                      m[9] * m[3] * m[6];

            inv[7] = m[0] * m[6] * m[11] -
                     m[0] * m[7] * m[10] -
                     m[4] * m[2] * m[11] +
                     m[4] * m[3] * m[10] +
                     m[8] * m[2] * m[7] -
                     m[8] * m[3] * m[6];

            inv[11] = -m[0] * m[5] * m[11] +
                       m[0] * m[7] * m[9] +
                       m[4] * m[1] * m[11] -
                       m[4] * m[3] * m[9] -
                       m[8] * m[1] * m[7] +
                       m[8] * m[3] * m[5];

            inv[15] = m[0] * m[5] * m[10] -
                      m[0] * m[6] * m[9] -
                      m[4] * m[1] * m[10] +
                      m[4] * m[2] * m[9] +
                      m[8] * m[1] * m[6] -
                      m[8] * m[2] * m[5];

            det = m[0] * inv[0] + m[1] * inv[4] + m[2] * inv[8] + m[3] * inv[12];

            //if (det == 0)
            //    return false;

            det = 1.0f / det;

            float[] invOut = new float[16];

            for (i = 0; i < 16; i++)
                invOut[i] = inv[i] * det;

            Matrix matOut = new Matrix(columns, rows);

            for (int x = 0; x < rows; x++)
                for (int y = 0; y < columns; y++)
                {
                    matOut.grid[x, y] = invOut[x + 4 * y];
                }

            return matOut;
        }
    }
}
