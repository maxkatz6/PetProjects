using System.Collections.Generic;
using Ormeli.Math;
using SharpDX;

namespace Ormeli.Graphics
{
    public class GeometryGenerator
    {

        public static TextureMesh CreateBox(float width, float height, float depth)
        {

            var mV = new List<TextureVertex>();

            float w2 = 0.5f*width;
            float h2 = 0.5f*height;
            float d2 = 0.5f*depth;
            // front
            mV.Add(new TextureVertex(-w2, -h2, d2, 0, 1));
            mV.Add(new TextureVertex(-w2, +h2, d2, 0, 0));
            mV.Add(new TextureVertex(+w2, +h2, d2, 1, 0));
            mV.Add(new TextureVertex(+w2, -h2, d2, 1, 1));
            // back
            mV.Add(new TextureVertex(-w2, -h2, -d2, 1, 1));
            mV.Add(new TextureVertex(+w2, -h2, -d2, 0, 1));
            mV.Add(new TextureVertex(+w2, +h2, -d2, 0, 0));
            mV.Add(new TextureVertex(-w2, +h2, -d2, 1, 0));
            // top
            mV.Add(new TextureVertex(-w2, +h2, d2, 0, 1));
            mV.Add(new TextureVertex(-w2, +h2, -d2, 0, 0));
            mV.Add(new TextureVertex(+w2, +h2, -d2, 1, 0));
            mV.Add(new TextureVertex(+w2, +h2, d2, 1, 1));
            // bottom
            mV.Add(new TextureVertex(-w2, -h2, d2, 1, 1));
            mV.Add(new TextureVertex(+w2, -h2, d2, 0, 1));
            mV.Add(new TextureVertex(+w2, -h2, -d2, 0, 0));
            mV.Add(new TextureVertex(-w2, -h2, -d2, 1, 0));
            // left
            mV.Add(new TextureVertex(-w2, -h2, -d2, 0, 1));
            mV.Add(new TextureVertex(-w2, +h2, -d2, 0, 0));
            mV.Add(new TextureVertex(-w2, +h2,d2, 1, 0));
            mV.Add(new TextureVertex(-w2, -h2, d2, 1, 1));
            // right
            mV.Add(new TextureVertex(+w2, -h2, d2, 0, 1));
            mV.Add(new TextureVertex(+w2, +h2, d2, 0, 0));
            mV.Add(new TextureVertex(+w2, +h2, -d2, 1, 0));
            mV.Add(new TextureVertex(+w2, -h2, -d2, 1, 1));


            var m = new TextureMesh();
            m.Initialize(new[]
            {
                0, 1, 2, 0, 2, 3, 4, 5, 6, 4, 6, 7, 8, 9, 10, 8, 10, 11, 12, 13, 14, 12, 14, 15, 16, 17, 18, 16, 18,
                19,
                20, 21, 22, 20, 22, 23
            }, mV.ToArray());
            return m;
        }

        public static TextureMesh CreateGrid(float width, float depth)
        {
            var w2 = 0.5f * width;
            var d2 = 0.5f * depth;
            var mV = new[]
            {
                new TextureVertex(new Vector3(-w2,0,-d2), new Vector2(1,1)),
                new TextureVertex(new Vector3(w2,0,-d2), new Vector2(0,1)),
                new TextureVertex(new Vector3(w2,0,d2), new Vector2(0,0)),
                new TextureVertex(new Vector3(-w2,0,d2), new Vector2(1,0))
            };
            var m = new TextureMesh();
            m.Initialize(new[] { 0, 1, 2, 0, 2, 3 }, mV);
            return m;
        }

        public static TextureMesh CreateSphere(float radius, int sliceCount, int stackCount)
        {
            var mI = new List<int>();
            var mV = new List<TextureVertex> { new TextureVertex(0, radius, 0, 1, 0) };

            double phiStep = System.Math.PI / stackCount;
            double thetaStep = 2.0f * System.Math.PI / sliceCount;

            for (int i = 1; i <= stackCount - 1; i++)
            {
                double phi = i * phiStep;
                for (int j = 0; j <= sliceCount; j++)
                {
                    double theta = j * thetaStep;
                    var p = new Vector3(
                        (float)(radius * System.Math.Sin(phi) * System.Math.Cos(theta)),
                        (float)(radius * System.Math.Cos(phi)),
                        -(float)(radius * System.Math.Sin(phi) * System.Math.Sin(theta))
                        );

                    var uv = new Vector2((float)(theta / (System.Math.PI * 2)), (float)(phi / System.Math.PI));
                    mV.Add(new TextureVertex(p, uv));
                }
            }
            mV.Add(new TextureVertex(0, -radius, 0, 0,1));

            for (int i = 1; i <= sliceCount; i++)
            {
                mI.Add(0);
                mI.Add(i + 1);
                mI.Add(i);
            }
            int baseIndex = 1;
            int ringVertexCount = sliceCount + 1;
            for (int i = 0; i < stackCount - 2; i++)
            {
                for (int j = 0; j < sliceCount; j++)
                {
                    mI.Add(baseIndex + i * ringVertexCount + j);
                    mI.Add(baseIndex + i * ringVertexCount + j + 1);
                    mI.Add(baseIndex + (i + 1) * ringVertexCount + j);

                    mI.Add(baseIndex + (i + 1) * ringVertexCount + j);
                    mI.Add(baseIndex + i * ringVertexCount + j + 1);
                    mI.Add(baseIndex + (i + 1) * ringVertexCount + j + 1);
                }
            }
            int southPoleIndex = mV.Count - 1;
            baseIndex = southPoleIndex - ringVertexCount;
            for (int i = 0; i < sliceCount; i++)
            {
                mI.Add(southPoleIndex);
                mI.Add(baseIndex + i);
                mI.Add(baseIndex + i + 1);
            }
            var m = new TextureMesh();
            m.Initialize(mI.ToArray(), mV.ToArray());
            return m;
        }
    }
}