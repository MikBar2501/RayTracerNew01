﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Numerics;


public enum Command
{
    maxverts,
    vertex,
    tri,
    camera,
    size,
    sphere,
    ambient,
    popTransform,
    pushTransform,
    scale,
    translate,
    rotate,
    directional,
    point,
    attenuation,
    diffuse,
    specular,
    shininess,
    emission,
    maxDepth,
    output
}

namespace RayTracerNew
{
    class FileReader
    {
        Command curCommand;

        Vector3[] vectors;
        int iterator = 0;

        Transform transform;
        List<Transform> transformStack;

        Material material;

        public FileReader()
        {
            transform = new Transform();
            transformStack = new List<Transform>();
            material = new Material();
        }

        void Push()
        {
            //DrawMatrix(transform.GetMatrix());
            transformStack.Add(new Transform(transform));
            transform = new Transform(transform);
            //transform.Set();
        }

       /* public void DrawMatrix(Matrix matrix)
        {
            for (int x = 0; x < matrix.rows; x++)
            {
                string m = "";
                for (int y = 0; y < matrix.columns; y++)
                {
                    m += matrix.grid[x, y] + "|";
                }
                Debug.WriteLine(m);
            }
        }*/

        bool Pop()
        {
            if (transformStack.Count == 0)
                return false;

            int top = transformStack.Count - 1;

            transform = new Transform(transformStack[top]);
            transformStack.RemoveAt(top);

            //DrawMatrix(transform.GetMatrix());

            return true;
        }

        public void Read(string path)
        {
            string[] lines = File.ReadAllLines(path, Encoding.UTF8);
            foreach (string line in lines)
            {
                Debug.WriteLine(line);

                if (line == "" || line[0] == '#')
                    continue;

                string lineTemp = line + ' ';
                string command = "";

                int lineIterator = 0;

                while (lineIterator < lineTemp.Length && lineTemp[lineIterator] != ' ')
                {
                    command += lineTemp[lineIterator];
                    lineIterator++;
                }

                lineIterator++; 

                switch (command) 
                {
                    case "maxverts":
                        curCommand = Command.maxverts;
                        break;

                    case "vertex":
                        curCommand = Command.vertex;
                        break;

                    case "tri":
                        curCommand = Command.tri;
                        break;

                    case "camera":
                        curCommand = Command.camera;
                        break;

                    case "size":
                        curCommand = Command.size;
                        break;

                    case "sphere":
                        curCommand = Command.sphere;
                        break;

                    case "ambient":
                        curCommand = Command.ambient;
                        break;

                    case "pushTransform":
                        curCommand = Command.pushTransform;
                        break;

                    case "popTransform":
                        curCommand = Command.popTransform;
                        break;

                    case "translate":
                        curCommand = Command.translate;
                        break;

                    case "scale":
                        curCommand = Command.scale;
                        break;

                    case "rotate":
                        curCommand = Command.rotate;
                        break;

                    case "directional":
                        curCommand = Command.directional;
                        break;

                    case "point":
                        curCommand = Command.point;
                        break;

                    case "attenuation":
                        curCommand = Command.attenuation;
                        break;

                    case "diffuse":
                        curCommand = Command.diffuse;
                        break;

                    case "specular":
                        curCommand = Command.specular;
                        break;

                    case "emission":
                        curCommand = Command.emission;
                        break;

                    case "shininess":
                        curCommand = Command.shininess;
                        break;

                    case "maxdepth":
                        curCommand = Command.maxDepth;
                        break;

                    case "output":
                        string pathArg = "";
                        while (lineIterator < lineTemp.Length)
                        {
                            if (lineTemp[lineIterator] == ' ')
                            {
                                if (pathArg == "")
                                {
                                    lineIterator++;
                                    continue;
                                }

                                Raytracer.savePath = pathArg;
                                lineIterator++;
                                continue;
                            }
                            pathArg += lineTemp[lineIterator];
                            lineIterator++;
                        }

                        Raytracer.savePath = pathArg;
                        continue;

                    default:
                        continue;
                }

               // Debug.WriteLine("Command " + command.ToString());

                List<float> args = new List<float>();

                string arg = "";

                while (lineIterator < lineTemp.Length)
                {
                    if (lineTemp[lineIterator] == ' ')
                    {
                        if (arg == "")
                        {
                            lineIterator++;
                            continue;
                        }

                        //obsluz arg
                        float argF = ConvertToFloat(arg);
                        //Debug.WriteLine("Arg " + argF);
                        args.Add(argF);

                        arg = "";
                        lineIterator++;
                        continue;
                    }
                    arg += lineTemp[lineIterator];
                    lineIterator++;
                }

                if (HandleCommand(curCommand, args))
                    Debug.WriteLine("Success");
                else
                    Debug.WriteLine("Fail");
            }
        }

        float ConvertToFloat(string s)
        {
            float value = 0.0f;
            float mod = 1.0f;
            int sign = 1;
            string numPar = "";
            bool afterDot = false;

            foreach (char ch in s)
            {
                if (ch == '+')
                    continue;

                if (ch == '-')
                {
                    sign = -1;
                    continue;
                }

                if (ch == '.')
                {
                    if (numPar != "")
                    {
                        value += int.Parse(numPar) * sign;
                        numPar = "";
                    }
                    afterDot = true;
                }
                else
                {
                    numPar += ch;
                    if (afterDot)
                        mod *= 0.1f;
                }
            }

            if (numPar != "")
            {
                value += int.Parse(numPar) * mod * sign;
            }

            return value;
        }

        bool HandleCommand(Command command, List<float> args)
        {
            switch (command)
            {
                case Command.maxverts:
                    if (args.Count == 0 || args[0] <= 0)
                        return false;
                    vectors = new Vector3[(int)args[0]];
                    iterator = 0;
                    break;

                case Command.vertex:
                    if (iterator >= vectors.Length || args.Count < 3)
                        return false;

                    Vector3 vec = new Vector3(args[0], args[1], args[2]);
                    vectors[iterator] = vec;

                    iterator++;
                    break;

                case Command.tri:
                    if (args.Count < 3)
                        return false;

                    foreach (float arg in args)
                    {
                        if (arg < 0 || (int)arg >= vectors.Length)
                            return false;
                    }

                    Triangle triangle = new Triangle(vectors[(int)args[0]], vectors[(int)args[1]], vectors[(int)args[2]]);
                    triangle.transform = transform.GetMatrix();
                    triangle.reverseTransform = triangle.transform.Inverse();
                    //triangle.normal = triangle.Normal();
                    triangle.material = new Material(material);
                    Raytracer.main.AddPrimitive(triangle);
                    break;

                case Command.sphere:
                    if (args.Count < 4)
                        return false;

                    Sphere sphere = new Sphere(new Vector3(args[0], args[1], args[2]), args[3]);
                    sphere.transform = transform.GetMatrix();
                    sphere.reverseTransform = sphere.transform.Inverse();
                    sphere.material = new Material(material);
                    Debug.WriteLine("sphere");
                    //DrawMatrix(sphere.transform);
                    Raytracer.main.AddPrimitive(sphere);
                    break;

                case Command.camera:
                    if (args.Count < 10)
                        return false;

                    Raytracer.main.camera.SetTransform(
                        new Vector3(args[0], args[1], args[2]),
                        new Vector3(args[3], args[4], args[5]),
                        new Vector3(args[6], args[7], args[8]),
                        args[9]);
                    break;

                case Command.size:
                    if (args.Count < 2)
                        return false;

                    Raytracer.main.camera.SetSize((int)args[0], (int)args[1]);
                    break;

                case Command.ambient:
                    if (args.Count < 3)
                        return false;

                    material.ambient = new ColorRGB((int)(args[0] * 255), (int)(args[1] * 255), (int)(args[2] * 255));
                    break;

                case Command.pushTransform:
                    Push();
                    break;

                case Command.popTransform:
                    if (!Pop())
                        return false;
                    break;

                case Command.translate:
                    if (args.Count < 3)
                        return false;

  
                    transform.transList.Add(Matrix.GetTranslateMatrix(new Vector3(args[0], args[1], args[2])));

                    //DrawMatrix(transform);


                    break;

                case Command.scale:
                    if (args.Count < 3)
                        return false;
                    
                    transform.transList.Add(Matrix.GetScaleMatrix(new Vector3(args[0], args[1], args[2])));

                    //DrawMatrix(transform);
                    break;

                case Command.rotate:
                    if (args.Count < 4)
                        return false;

                    if (args[0] != 0.0f)
                    {
                        transform.transList.Add(Matrix.GetRotationXMatrix(args[0] * args[3]));
                    }
                    if (args[1] != 0.0f)
                    {

                        transform.transList.Add(Matrix.GetRotationYMatrix(args[1] * args[3]));
                    }

                    if (args[2] != 0.0f)
                    {
                       
                        transform.transList.Add(Matrix.GetRotationZMatrix(args[2] * args[3]));
                    }
                    //DrawMatrix(transform);
                    break;

                case Command.directional:
                    if (args.Count < 6)
                        return false;

                    Debug.WriteLine("DIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIR");

                    //DirectionalLight light = new DirectionalLight();
                    //light.color = new ColorRGB((int)(args[3] * 255), (int)(args[4] * 255), (int)(args[5] * 255));
                    //light.direction = Vector3.Normalize(new Vector3(args[0], args[1], args[2]));

                    ColorRGB lightColor = new ColorRGB((int)(args[3] * 255), (int)(args[4] * 255), (int)(args[5] * 255));
                    Vector3 dirPose = Vector3.Normalize(new Vector3(args[0], args[1], args[2]));
                    
                    Lights light = new Lights(lightColor, dirPose, false);

                    Raytracer.main.lighting.lights.Add(light);
                    break;

                case Command.point:
                    if (args.Count < 6)
                        return false;

                    Debug.WriteLine("POOOOOOOOOOOOOOOOOOOOINT");

                    //PointLight lightP = new PointLight();
                    //lightP.color = new ColorRGB((int)(args[3] * 255), (int)(args[4] * 255), (int)(args[5] * 255));
                    //lightP.position = new Vector3(args[0], args[1], args[2]);


                    ColorRGB lightColorP = new ColorRGB((int)(args[3] * 255), (int)(args[4] * 255), (int)(args[5] * 255));
                    Vector3 dirPoseP = new Vector3(args[0], args[1], args[2]);
                    Lights lightP = new Lights(lightColorP,dirPoseP, true);
                    Raytracer.main.lighting.lights.Add(lightP);

                    Debug.WriteLine(lightP.lightColor.ToString());
                    break;

                case Command.attenuation:
                    if (args.Count < 3)
                        return false;

                    //PointLight.constant = args[0];
                    //PointLight.linear = args[1];
                    //PointLight.quadratic = args[2];

                    Lights.constant = args[0];
                    Lights.linear = args[1];
                    Lights.quadratic = args[2];
                    break;

                case Command.diffuse:
                    if (args.Count < 3)
                        return false;

                    material.diffuse = new ColorRGB((int)(args[0] * 255), (int)(args[1] * 255), (int)(args[2] * 255));
                    break;

                case Command.specular:
                    if (args.Count < 3)
                        return false;

                    material.specular = new ColorRGB((int)(args[0] * 255), (int)(args[1] * 255), (int)(args[2] * 255));
                    break;

                case Command.shininess:
                    if (args.Count < 1)
                        return false;

                    material.shininess = args[0];
                    break;

                case Command.emission:
                    if (args.Count < 3)
                        return false;

                    material.emission = new ColorRGB((int)(args[0] * 255), (int)(args[1] * 255), (int)(args[2] * 255));
                    break;

                case Command.maxDepth:
                    if (args.Count < 1)
                        return false;

                    Raytracer.main.maxDepth = (int)(args[0]);
                    break;
            }

            return true;
        }
    }
}
