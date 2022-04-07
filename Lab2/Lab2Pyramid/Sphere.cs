using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2Pyramid
{
    class Sphere
    {
        public List<float> Vertecies = new List<float>();
        public List<float> Normals = new List<float>();
        public List<float> TexCoords = new List<float>();

        public List<int> Indices = new List<int>();
        public List<int> lineIndices = new List<int>();

        public float R, X, Y, Z;
        public int sectorCount, stackCount;

        public Sphere(float R, float X, float Y, float Z, int sectorCount = 30, int stackCount = 90)
        {
            this.R = R;
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.sectorCount = sectorCount;
            this.stackCount = stackCount;
        }

        public float[] GetNormals()
        {
            return Normals.ToArray();
        }

        public float[] GetVertecies()
        {
            const float PI = 3.1415926f;

            float x, y, z, xy;                              // vertex position
            float nx, ny, nz, lengthInv = 1.0f / R;    // vertex normal
            float s, t;                                     // vertex texCoord

            float sectorStep = 2 * PI / sectorCount;
            float stackStep = PI / stackCount;
            float sectorAngle, stackAngle;

            for (int i = 0; i <= stackCount; ++i)
            {
                stackAngle = PI / 2 - i * stackStep;        // starting from pi/2 to -pi/2
                xy = R * (float)Math.Cos(stackAngle);             // r * cos(u)
                z = R * (float)Math.Sin(stackAngle) + Z;              // r * sin(u)
                for (int j = 0; j <= sectorCount; ++j)
                {
                    sectorAngle = j * sectorStep;           // starting from 0 to 2pi

                    // vertex position (x, y, z)
                    x = xy * (float)Math.Cos(sectorAngle) + X;             // r * cos(u) * cos(v)
                    y = xy * (float)Math.Sin(sectorAngle) + Y;             // r * cos(u) * sin(v)
                    Vertecies.Add(x);
                    Vertecies.Add(y);
                    Vertecies.Add(z);

                    // normalized vertex normal (nx, ny, nz)
                    nx = x * lengthInv;
                    ny = y * lengthInv;
                    nz = z * lengthInv;
                    Normals.Add(nx);
                    Normals.Add(ny);
                    Normals.Add(nz);

                    // vertex tex coord (s, t) range between [0, 1]
                    s = (float)j / sectorCount;
                    t = (float)i / stackCount;
                    TexCoords.Add(s);
                    TexCoords.Add(t);

                }
            }
            return Vertecies.ToArray();
        }

        public int[] GetIndices()
        {
            int k1, k2;

            for (int i = 0; i < stackCount; ++i)
            {
                k1 = i * (sectorCount + 1);     // beginning of current stack
                k2 = k1 + sectorCount + 1;      // beginning of next stack

                for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
                {
                    // 2 triangles per sector excluding first and last stacks
                    // k1 => k2 => k1+1
                    if (i != 0)
                    {
                        Indices.Add(k1);
                        Indices.Add(k2);
                        Indices.Add(k1 + 1);
                    }

                    // k1+1 => k2 => k2+1
                    if (i != (stackCount - 1))
                    {
                        Indices.Add(k1 + 1);
                        Indices.Add(k2);
                        Indices.Add(k2 + 1);
                    }

                    // store indices for lines
                    // vertical lines for all stacks, k1 => k2
                    lineIndices.Add(k1);
                    lineIndices.Add(k2);
                    if (i != 0)  // horizontal lines except 1st stack, k1 => k+1
                    {
                        lineIndices.Add(k1);
                        lineIndices.Add(k1 + 1);
                    }
                }
            }

            return Indices.ToArray();

        }


        public float[] GetAllTogether()
        {
            List<float> result = new List<float>();

            for (int i = 0; i < Vertecies.Count / 3; ++i)
            {
                result.Add(Vertecies[i * 3]);
                result.Add(Vertecies[i * 3 + 1]);
                result.Add(Vertecies[i * 3 + 2]);

                result.Add(Normals[i * 3]);
                result.Add(Normals[i * 3 + 1]);
                result.Add(Normals[i * 3 + 2]);

                result.Add(TexCoords[i * 2]);
                result.Add(TexCoords[i * 2 + 1]);
            }


            return result.ToArray();


        }
    }
}
