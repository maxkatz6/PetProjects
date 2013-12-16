#region Using

using SharpDX;
using System;
using System.Collections.Generic;
using TDF.Core;
using TDF.Graphics.Data;

#endregion Using

namespace TDF.Graphics.Models
{
    public class GeometryGenerator
    {
        public static MeshGeometry<BumpVertex> CreateBox(float width, float height, float depth)
        {
            MeshGeometry<BumpVertex> mesh;
            mesh.Indices = new List<uint>
            {
                0,
                1,
                2,
                0,
                2,
                3,
                4,
                5,
                6,
                4,
                6,
                7,
                8,
                9,
                10,
                8,
                10,
                11,
                12,
                13,
                14,
                12,
                14,
                15,
                16,
                17,
                18,
                16,
                18,
                19,
                20,
                21,
                22,
                20,
                22,
                23
            };

            mesh.Vertices = new List<BumpVertex>();

            var w2 = 0.5f * width;
            var h2 = 0.5f * height;
            var d2 = 0.5f * depth;
            // front
            mesh.Vertices.Add(new BumpVertex(-w2, -h2, -d2, 0, 0, -1, 1, 0, 0, 0, 1));
            mesh.Vertices.Add(new BumpVertex(-w2, +h2, -d2, 0, 0, -1, 1, 0, 0, 0, 0));
            mesh.Vertices.Add(new BumpVertex(+w2, +h2, -d2, 0, 0, -1, 1, 0, 0, 1, 0));
            mesh.Vertices.Add(new BumpVertex(+w2, -h2, -d2, 0, 0, -1, 1, 0, 0, 1, 1));
            // back
            mesh.Vertices.Add(new BumpVertex(-w2, -h2, +d2, 0, 0, 1, -1, 0, 0, 1, 1));
            mesh.Vertices.Add(new BumpVertex(+w2, -h2, +d2, 0, 0, 1, -1, 0, 0, 0, 1));
            mesh.Vertices.Add(new BumpVertex(+w2, +h2, +d2, 0, 0, 1, -1, 0, 0, 0, 0));
            mesh.Vertices.Add(new BumpVertex(-w2, +h2, +d2, 0, 0, 1, -1, 0, 0, 1, 0));
            // top
            mesh.Vertices.Add(new BumpVertex(-w2, +h2, -d2, 0, 1, 0, 1, 0, 0, 0, 1));
            mesh.Vertices.Add(new BumpVertex(-w2, +h2, +d2, 0, 1, 0, 1, 0, 0, 0, 0));
            mesh.Vertices.Add(new BumpVertex(+w2, +h2, +d2, 0, 1, 0, 1, 0, 0, 1, 0));
            mesh.Vertices.Add(new BumpVertex(+w2, +h2, -d2, 0, 1, 0, 1, 0, 0, 1, 1));
            // bottom
            mesh.Vertices.Add(new BumpVertex(-w2, -h2, -d2, 0, -1, 0, -1, 0, 0, 1, 1));
            mesh.Vertices.Add(new BumpVertex(+w2, -h2, -d2, 0, -1, 0, -1, 0, 0, 0, 1));
            mesh.Vertices.Add(new BumpVertex(+w2, -h2, +d2, 0, -1, 0, -1, 0, 0, 0, 0));
            mesh.Vertices.Add(new BumpVertex(-w2, -h2, +d2, 0, -1, 0, -1, 0, 0, 1, 0));
            // left
            mesh.Vertices.Add(new BumpVertex(-w2, -h2, +d2, -1, 0, 0, 0, 0, -1, 0, 1));
            mesh.Vertices.Add(new BumpVertex(-w2, +h2, +d2, -1, 0, 0, 0, 0, -1, 0, 0));
            mesh.Vertices.Add(new BumpVertex(-w2, +h2, -d2, -1, 0, 0, 0, 0, -1, 1, 0));
            mesh.Vertices.Add(new BumpVertex(-w2, -h2, -d2, -1, 0, 0, 0, 0, -1, 1, 1));
            // right
            mesh.Vertices.Add(new BumpVertex(+w2, -h2, -d2, 1, 0, 0, 0, 0, 1, 0, 1));
            mesh.Vertices.Add(new BumpVertex(+w2, +h2, -d2, 1, 0, 0, 0, 0, 1, 0, 0));
            mesh.Vertices.Add(new BumpVertex(+w2, +h2, +d2, 1, 0, 0, 0, 0, 1, 1, 0));
            mesh.Vertices.Add(new BumpVertex(+w2, -h2, +d2, 1, 0, 0, 0, 0, 1, 1, 1));

            return mesh;
        }

        public static MeshGeometry<BumpVertex> CreateCylinder(float bottomRadius, float topRadius, float height,
            uint sliceCount, uint stackCount)
        {
            MeshGeometry<BumpVertex> mesh;
            mesh.Indices = new List<uint>();
            mesh.Vertices = new List<BumpVertex>();

            float stackHeight = height / stackCount;
            float radiusStep = (topRadius - bottomRadius) / stackCount;
            uint ringCount = stackCount + 1;

            for (uint i = 0; i < ringCount; i++)
            {
                float y = -0.5f * height + i * stackHeight;
                float r = bottomRadius + i * radiusStep;
                double dTheta = 2.0f * Math.PI / sliceCount;
                for (uint j = 0; j <= sliceCount; j++)
                {
                    double c = Math.Cos(j * dTheta);
                    double s = Math.Sin(j * dTheta);

                    var v = new Vector3((float)(r * c), y, (float)(r * s));
                    var uv = new Vector2((float)j / sliceCount, 1.0f - (float)i / stackCount);
                    var t = new Vector3((float)-s, 0.0f, (float)c);

                    float dr = bottomRadius - topRadius;
                    var bitangent = new Vector3((float)(dr * c), -height, (float)(dr * s));

                    Vector3 n = Vector3.Normalize(Vector3.Cross(t, bitangent));

                    mesh.Vertices.Add(new BumpVertex(v, n, uv, t));
                }
            }
            uint ringVertexCount = sliceCount + 1;
            for (uint i = 0; i < stackCount; i++)
            {
                for (uint j = 0; j < sliceCount; j++)
                {
                    mesh.Indices.Add(i * ringVertexCount + j);
                    mesh.Indices.Add((i + 1) * ringVertexCount + j);
                    mesh.Indices.Add((i + 1) * ringVertexCount + j + 1);

                    mesh.Indices.Add(i * ringVertexCount + j);
                    mesh.Indices.Add((i + 1) * ringVertexCount + j + 1);
                    mesh.Indices.Add(i * ringVertexCount + j + 1);
                }
            }
            BuildCylinderTopCap(topRadius, height, sliceCount, ref mesh.Vertices, ref mesh.Indices);
            BuildCylinderBottomCap(bottomRadius, height, sliceCount, ref mesh.Vertices, ref mesh.Indices);
            return mesh;
        }

        /// <summary>
        ///     Creates the grid.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="depth">The depth.</param>
        /// <param name="cD">Count  mesh.Verticesices in depth</param>
        /// <param name="cW">Count  mesh.Verticesices in width</param>
        /// <returns></returns>
        public static MeshGeometry<BumpVertex> CreateGrid(float width, float depth, uint cD, uint cW)
        {
            MeshGeometry<BumpVertex> mesh;
            mesh.Indices = new List<uint>();
            mesh.Vertices = new List<BumpVertex>();

            float halfWidth = width * 0.5f;
            float halfDepth = depth * 0.5f;

            float dx = width / (cW - 1);
            float dz = depth / (cD - 1);

            float du = 1.0f / (cW - 1);
            float dv = 1.0f / (cD - 1);

            for (int i = 0; i < cD; i++)
            {
                float z = halfDepth - i * dz;
                for (int j = 0; j < cW; j++)
                {
                    float x = -halfWidth + j * dx;
                    mesh.Vertices.Add(new BumpVertex(new Vector3(x, 0, z), new Vector3(0, 1, 0), new Vector2(j * du, i * dv),
                        new Vector3(1, 0, 0)));
                }
            }
            for (uint i = 0; i < cD - 1; i++)
            {
                for (uint j = 0; j < cW - 1; j++)
                {
                    mesh.Indices.Add(i * cW + j);
                    mesh.Indices.Add(i * cW + j + 1);
                    mesh.Indices.Add((i + 1) * cW + j);

                    mesh.Indices.Add((i + 1) * cW + j);
                    mesh.Indices.Add(i * cW + j + 1);
                    mesh.Indices.Add((i + 1) * cW + j + 1);
                }
            }

            return mesh;
        }

        public static MeshGeometry<BumpVertex> CreateSphere(float radius, uint sliceCount, uint stackCount)
        {
            MeshGeometry<BumpVertex> mesh;
            mesh.Indices = new List<uint>();
            mesh.Vertices = new List<BumpVertex> { new BumpVertex(0, radius, 0, 0, 1, 0, 1, 0, 0, 0, 0) };

            double phiStep = Math.PI / stackCount;
            double thetaStep = 2.0f * Math.PI / sliceCount;

            for (uint i = 1; i <= stackCount - 1; i++)
            {
                double phi = i * phiStep;
                for (uint j = 0; j <= sliceCount; j++)
                {
                    double theta = j * thetaStep;
                    var p = new Vector3(
                        (float)(radius * Math.Sin(phi) * Math.Cos(theta)),
                        (float)(radius * Math.Cos(phi)),
                        (float)(radius * Math.Sin(phi) * Math.Sin(theta))
                        );

                    var t = new Vector3((float)(-radius * Math.Sin(phi) * Math.Sin(theta)), 0,
                        (float)(radius * Math.Sin(phi) * Math.Cos(theta)));
                    t.Normalize();
                    Vector3 n = Vector3.Normalize(p);

                    var uv = new Vector2((float)(theta / (Math.PI * 2)), (float)(phi / Math.PI));
                    mesh.Vertices.Add(new BumpVertex(p, n, uv, t));
                }
            }
            mesh.Vertices.Add(new BumpVertex(0, -radius, 0, 0, -1, 0, 1, 0, 0, 0, 1));

            for (uint i = 1; i <= sliceCount; i++)
            {
                mesh.Indices.Add(0);
                mesh.Indices.Add(i + 1);
                mesh.Indices.Add(i);
            }
            uint baseIndex = 1;
            uint ringVertexCount = sliceCount + 1;
            for (uint i = 0; i < stackCount - 2; i++)
            {
                for (uint j = 0; j < sliceCount; j++)
                {
                    mesh.Indices.Add(baseIndex + i * ringVertexCount + j);
                    mesh.Indices.Add(baseIndex + i * ringVertexCount + j + 1);
                    mesh.Indices.Add(baseIndex + (i + 1) * ringVertexCount + j);

                    mesh.Indices.Add(baseIndex + (i + 1) * ringVertexCount + j);
                    mesh.Indices.Add(baseIndex + i * ringVertexCount + j + 1);
                    mesh.Indices.Add(baseIndex + (i + 1) * ringVertexCount + j + 1);
                }
            }
            uint southPoleIndex = (uint)mesh.Vertices.Count - 1;
            baseIndex = southPoleIndex - ringVertexCount;
            for (uint i = 0; i < sliceCount; i++)
            {
                mesh.Indices.Add(southPoleIndex);
                mesh.Indices.Add(baseIndex + i);
                mesh.Indices.Add(baseIndex + i + 1);
            }
            return mesh;
        }

        private static void BuildCylinderBottomCap(float bottomRadius, float height, uint sliceCount,
            ref List<BumpVertex> vert, ref List<uint> inds)
        {
            var baseIndex = (uint)vert.Count;

            float y = -0.5f * height;
            double dTheta = 2.0f * Math.PI / sliceCount;

            for (int i = 0; i <= sliceCount; i++)
            {
                double x = bottomRadius * Math.Cos(i * dTheta);
                double z = bottomRadius * Math.Sin(i * dTheta);

                double u = x / height + 0.5f;
                double v = z / height + 0.5f;
                vert.Add(new BumpVertex((float)x, y, (float)z, 0, -1, 0, 1, 0, 0, (float)u, (float)v));
            }
            vert.Add(new BumpVertex(0, y, 0, 0, -1, 0, 1, 0, 0, 0.5f, 0.5f));
            uint centerIndex = baseIndex - 1;
            for (uint i = 0; i < sliceCount; i++)
            {
                inds.Add(centerIndex);
                inds.Add(baseIndex + i);
                inds.Add(baseIndex + i + 1);
            }
        }

        private static void BuildCylinderTopCap(float topRadius, float height, uint sliceCount,
            ref List<BumpVertex> vert, ref List<uint> inds)
        {
            var baseIndex = (uint)(vert.Count);

            float y = 0.5f * height;
            double dTheta = 2.0f * Math.PI / sliceCount;

            for (int i = 0; i <= sliceCount; i++)
            {
                double x = topRadius * Math.Cos(i * dTheta);
                double z = topRadius * Math.Sin(i * dTheta);

                double u = x / height + 0.5f;
                double v = z / height + 0.5f;
                vert.Add(new BumpVertex((float)x, y, (float)z, 0, 1, 0, 1, 0, 0, (float)u, (float)v));
            }
            vert.Add(new BumpVertex(0, y, 0, 0, 1, 0, 1, 0, 0, 0.5f, 0.5f));
            uint centerIndex = baseIndex - 1;
            for (uint i = 0; i < sliceCount; i++)
            {
                inds.Add(centerIndex);
                inds.Add(baseIndex + i + 1);
                inds.Add(baseIndex + i);
            }
        }

        public struct MeshGeometry<T> where T : struct
        {
            public List<uint> Indices;
            public List<T> Vertices;

            public MeshGeometry<TC> Convert<TC>() where TC : struct
            {
                var verts = new List<TC>(Vertices.Count);
                for (int i = 0; i < Vertices.Count; i++)
                {
                    verts.Add(Vertices[i].ConvertTo<TC>());
                }

                return new MeshGeometry<TC>
                {
                    Indices = Indices,
                    Vertices = verts
                    //Vertices = Vertices.Select(arg => arg.ConvertTo<TC>()).ToList()
                };
            }

            public StaticMesh ToMesh()
            {
                var returnMesh = new StaticMesh();
                returnMesh.SetIndices(Indices);
                returnMesh.SetVertices(Vertices);
                return returnMesh;
            }
        }
    }
}