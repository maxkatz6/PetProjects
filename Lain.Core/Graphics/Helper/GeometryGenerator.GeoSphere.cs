﻿using System;
using System.Collections.Generic;
using Lain.Graphics.Drawable;
using SharpDX;

namespace Lain.Graphics
{
    public partial class GeometryGenerator
    {
        private static readonly Vector3[] OctahedronVertices =
        {
            // when looking down the negative z-axis (into the screen)
            new Vector3(0, 1, 0), // 0 top
            new Vector3(0, 0, -1), // 1 front
            new Vector3(1, 0, 0), // 2 right
            new Vector3(0, 0, 1), // 3 back
            new Vector3(-1, 0, 0), // 4 left
            new Vector3(0, -1, 0) // 5 bottom
        };

        private static readonly int[] OctahedronIndices =
        {
            0, 1, 2, // top front-right face
            0, 2, 3, // top back-right face
            0, 3, 4, // top back-left face
            0, 4, 1, // top front-left face
            5, 1, 4, // bottom front-left face
            5, 4, 3, // bottom back-left face
            5, 3, 2, // bottom back-right face
            5, 2, 1 // bottom front-right face
        };

        private static List<Vector3> vertexPositions;
        private static List<int> indexList;
        private static unsafe int* indices;
        // Key: an edge
        // Value: the index of the vertex which lies midway between the two vertices pointed to by the key value
        // This map is used to avoid duplicating vertices when subdividing triangles along edges.
        private static Dictionary<UndirectedEdge, int> subdividedEdges;

        /// <summary>
        ///     Creates a Geodesic sphere.
        /// </summary>
        /// <param name="diameter">The diameter.</param>
        /// <param name="tessellation">The tessellation.</param>
        /// <returns>A Geodesic sphere.</returns>
        public static unsafe GeometryInfo<Vertex> CreateGeoSphere<Vertex>(float diameter = 1.0f, int tessellation = 3)
            where Vertex : struct, IVertex
        {
            subdividedEdges = new Dictionary<UndirectedEdge, int>();

            var radius = diameter/2.0f;

            // Start with an octahedron; copy the data into the vertex/index collection.
            vertexPositions = new List<Vector3>(OctahedronVertices);
            indexList = new List<int>(OctahedronIndices);

            // We know these values by looking at the above index list for the octahedron. Despite the subdivisions that are
            // about to go on, these values aren't ever going to change because the vertices don't move around in the array.
            // We'll need these values later on to fix the singularities that show up at the poles.
            const int northPoleIndex = 0;
            const int southPoleIndex = 5;

            for (var iSubdivision = 0; iSubdivision < tessellation; ++iSubdivision)
            {
                // The new index collection after subdivision.
                var newIndices = new List<int>();
                subdividedEdges.Clear();

                var triangleCount = indexList.Count/3;
                for (var iTriangle = 0; iTriangle < triangleCount; ++iTriangle)
                {
                    // For each edge on this triangle, create a new vertex in the middle of that edge.
                    // The winding order of the triangles we output are the same as the winding order of the inputs.

                    // Indices of the vertices making up this triangle
                    var iv0 = indexList[iTriangle*3 + 0];
                    var iv1 = indexList[iTriangle*3 + 1];
                    var iv2 = indexList[iTriangle*3 + 2];

                    //// The existing vertices
                    //Vector3 v0 = vertexPositions[iv0];
                    //Vector3 v1 = vertexPositions[iv1];
                    //Vector3 v2 = vertexPositions[iv2];

                    // Get the new vertices
                    Vector3 v01; // vertex on the midpoint of v0 and v1
                    Vector3 v12; // ditto v1 and v2
                    Vector3 v20; // ditto v2 and v0
                    int iv01; // index of v01
                    int iv12; // index of v12
                    int iv20; // index of v20

                    // Add/get new vertices and their indices
                    DivideEdge(iv0, iv1, out v01, out iv01);
                    DivideEdge(iv1, iv2, out v12, out iv12);
                    DivideEdge(iv0, iv2, out v20, out iv20);

                    // Add the new indices. We have four new triangles from our original one:
                    //        v0
                    //        o
                    //       /a\
                    //  v20 o---o v01
                    //     /b\c/d\
                    // v2 o---o---o v1
                    //       v12

                    // a
                    newIndices.Add(iv0);
                    newIndices.Add(iv01);
                    newIndices.Add(iv20);

                    // b
                    newIndices.Add(iv20);
                    newIndices.Add(iv12);
                    newIndices.Add(iv2);

                    // d
                    newIndices.Add(iv20);
                    newIndices.Add(iv01);
                    newIndices.Add(iv12);

                    // d
                    newIndices.Add(iv01);
                    newIndices.Add(iv1);
                    newIndices.Add(iv12);
                }

                indexList.Clear();
                indexList.AddRange(newIndices);
            }

            // Now that we've completed subdivision, fill in the final vertex collection
            var vertices = new List<Vertex>(vertexPositions.Count);
            for (var i = 0; i < vertexPositions.Count; i++)
            {
                var vertexValue = vertexPositions[i];

                var normal = vertexValue;
                normal.Normalize();

                var pos = normal*radius;

                // calculate texture coordinates for this vertex
                var longitude = (float) Math.Atan2(normal.X, -normal.Z);
                var latitude = (float) Math.Acos(normal.Y);

                var u = (float) (longitude/(Math.PI*2.0) + 0.5);
                var v = (float) (latitude/Math.PI);

                var texcoord = new Vector2(1.0f - u, v);
                vertices.Add(new Vertex {Position = pos, Normal = normal, TexCoord = texcoord});
            }

            const float XMVectorSplatEpsilon = 1.192092896e-7f;

            // There are a couple of fixes to do. One is a texture coordinate wraparound fixup. At some point, there will be
            // a set of triangles somewhere in the mesh with texture coordinates such that the wraparound across 0.0/1.0
            // occurs across that triangle. Eg. when the left hand side of the triangle has a U coordinate of 0.98 and the
            // right hand side has a U coordinate of 0.0. The intent is that such a triangle should render with a U of 0.98 to
            // 1.0, not 0.98 to 0.0. If we don't do this fixup, there will be a visible seam across one side of the sphere.
            // 
            // Luckily this is relatively easy to fix. There is a straight edge which runs down the prime meridian of the
            // completed sphere. If you imagine the vertices along that edge, they circumscribe a semicircular arc starting at
            // y=1 and ending at y=-1, and sweeping across the range of z=0 to z=1. x stays zero. It's along this edge that we
            // need to duplicate our vertices - and provide the correct texture coordinates.
            var preCount = vertices.Count;
            var indicesArray = indexList.ToArray();
            fixed (void* pIndices = indicesArray)
            {
                indices = (int*) pIndices;

                for (var i = 0; i < preCount; ++i)
                {
                    // This vertex is on the prime meridian if position.x and texcoord.u are both zero (allowing for small epsilon).
                    var isOnPrimeMeridian = MathUtil.WithinEpsilon(vertices[i].Position.X, 0, XMVectorSplatEpsilon)
                                            && MathUtil.WithinEpsilon(vertices[i].TexCoord.X, 0, XMVectorSplatEpsilon);

                    if (isOnPrimeMeridian)
                    {
                        var newIndex = vertices.Count;

                        // copy this vertex, correct the texture coordinate, and add the vertex
                        var v = vertices[i];
                        v.TexCoord = new Vector2(1.0f, v.TexCoord.Y);
                        vertices.Add(v);

                        // Now find all the triangles which contain this vertex and update them if necessary
                        for (var j = 0; j < indexList.Count; j += 3)
                        {
                            var triIndex0 = &indices[j + 0];
                            var triIndex1 = &indices[j + 1];
                            var triIndex2 = &indices[j + 2];

                            if (*triIndex0 == i)
                            {
                                // nothing; just keep going
                            }
                            else if (*triIndex1 == i)
                            {
                                Utilities.Swap(ref *triIndex0, ref *triIndex1);
                                Utilities.Swap(ref *triIndex1, ref *triIndex2);
                            }
                            else if (*triIndex2 == i)
                            {
                                Utilities.Swap(ref *triIndex0, ref *triIndex2);
                                Utilities.Swap(ref *triIndex1, ref *triIndex2);
                            }
                            else
                            {
                                // this triangle doesn't use the vertex we're interested in
                                continue;
                            }

                            // check the other two vertices to see if we might need to fix this triangle
                            if (Math.Abs(vertices[*triIndex0].TexCoord.X - vertices[*triIndex1].TexCoord.X) > 0.5f ||
                                Math.Abs(vertices[*triIndex0].TexCoord.X - vertices[*triIndex2].TexCoord.X) > 0.5f)
                            {
                                // yep; replace the specified index to point to the new, corrected vertex
                                indices[j + 0] = newIndex;
                            }
                        }
                    }
                }

                FixPole(ref vertices, northPoleIndex);
                FixPole(ref vertices, southPoleIndex);

                // Clear indices as it will not be accessible outside the fixed statement
                indices = (int*) 0;
            }
            return new GeometryInfo<Vertex> {Indices = indicesArray, Vertices = vertices.ToArray()};
        }

        private static unsafe void FixPole<Vertex>(ref List<Vertex> vertices, int poleIndex)
            where Vertex : struct, IVertex
        {
            var poleVertex = vertices[poleIndex];
            var overwrittenPoleVertex = false; // overwriting the original pole vertex saves us one vertex

            for (ushort i = 0; i < indexList.Count; i += 3)
            {
                // These pointers point to the three indices which make up this triangle. pPoleIndex is the pointer to the
                // entry in the index array which represents the pole index, and the other two pointers point to the other
                // two indices making up this triangle.
                int* pPoleIndex;
                int* pOtherIndex0;
                int* pOtherIndex1;
                if (indices[i + 0] == poleIndex)
                {
                    pPoleIndex = &indices[i + 0];
                    pOtherIndex0 = &indices[i + 1];
                    pOtherIndex1 = &indices[i + 2];
                }
                else if (indices[i + 1] == poleIndex)
                {
                    pPoleIndex = &indices[i + 1];
                    pOtherIndex0 = &indices[i + 2];
                    pOtherIndex1 = &indices[i + 0];
                }
                else if (indices[i + 2] == poleIndex)
                {
                    pPoleIndex = &indices[i + 2];
                    pOtherIndex0 = &indices[i + 0];
                    pOtherIndex1 = &indices[i + 1];
                }
                else
                {
                    continue;
                }

                // Calculate the texcoords for the new pole vertex, add it to the vertices and update the index
                var newPoleVertex = poleVertex;
                newPoleVertex.TexCoord =
                    new Vector2((vertices[*pOtherIndex0].TexCoord.X + vertices[*pOtherIndex1].TexCoord.X)*0.5f,
                        poleVertex.TexCoord.Y);


                if (!overwrittenPoleVertex)
                {
                    vertices[poleIndex] = newPoleVertex;
                    overwrittenPoleVertex = true;
                }
                else
                {
                    *pPoleIndex = vertices.Count;
                    vertices.Add(newPoleVertex);
                }
            }
        }

        // Function that, when given the index of two vertices, creates a new vertex at the midpoint of those vertices.
        private static void DivideEdge(int i0, int i1, out Vector3 outVertex, out int outIndex)
        {
            var edge = new UndirectedEdge(i0, i1);

            // Check to see if we've already generated this vertex
            if (subdividedEdges.TryGetValue(edge, out outIndex))
            {
                // We've already generated this vertex before
                outVertex = vertexPositions[outIndex]; // and the vertex itself
            }
            else
            {
                // Haven't generated this vertex before: so add it now

                // outVertex = (vertices[i0] + vertices[i1]) / 2
                outVertex = (vertexPositions[i0] + vertexPositions[i1])*0.5f;
                outIndex = vertexPositions.Count;
                vertexPositions.Add(outVertex);

                // Now add it to the map.
                subdividedEdges[edge] = outIndex;
            }
        }

        // An undirected edge between two vertices, represented by a pair of indexes into a vertex array.
        // Because this edge is undirected, (a,b) is the same as (b,a).
        private struct UndirectedEdge : IEquatable<UndirectedEdge>
        {
            public readonly int Item1;
            public readonly int Item2;

            public UndirectedEdge(int item1, int item2)
            {
                // Makes an undirected edge. Rather than overloading comparison operators to give us the (a,b)==(b,a) property,
                // we'll just ensure that the larger of the two goes first. This'll simplify things greatly.
                Item1 = Math.Max(item1, item2);
                Item2 = Math.Min(item1, item2);
            }

            public bool Equals(UndirectedEdge other)
            {
                return Item1 == other.Item1 && Item2 == other.Item2;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                return obj is UndirectedEdge && Equals((UndirectedEdge) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (Item1.GetHashCode()*397) ^ Item2.GetHashCode();
                }
            }

            public static bool operator ==(UndirectedEdge left, UndirectedEdge right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(UndirectedEdge left, UndirectedEdge right)
            {
                return !left.Equals(right);
            }
        }
    }
}