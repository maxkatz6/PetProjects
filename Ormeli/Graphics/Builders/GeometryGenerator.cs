using System.Collections.Generic;
using SharpDX;

namespace Ormeli.Graphics.Builders
{
	public struct GeometryInfo<T> where T : struct, IVertex
	{
		public T[] Vertices { get; set; }
		public int[] Indices { get; set; }
	}

	public class GeometryGenerator
	{
		public static GeometryInfo<Vertex> CreateBox<Vertex>(float width, float height, float depth, bool isDynamic = false)
			where Vertex : struct, IVertex
		{
			float w2 = 0.5f*width, h2 = 0.5f*height, d2 = 0.5f*depth;
			var mV = new List<Vertex>
			{
				// front
				new Vertex {Position = new Vector3(-w2, -h2, d2), TexCoord = new Vector2(0, 1)},
				new Vertex {Position = new Vector3(-w2, +h2, d2), TexCoord = new Vector2(0, 0)},
				new Vertex {Position = new Vector3(+w2, +h2, d2), TexCoord = new Vector2(1, 0)},
				new Vertex {Position = new Vector3(+w2, -h2, d2), TexCoord = new Vector2(1, 1)},
				// back
				new Vertex {Position = new Vector3(-w2, -h2, -d2), TexCoord = new Vector2(1, 1)},
				new Vertex {Position = new Vector3(+w2, -h2, -d2), TexCoord = new Vector2(0, 1)},
				new Vertex {Position = new Vector3(+w2, +h2, -d2), TexCoord = new Vector2(0, 0)},
				new Vertex {Position = new Vector3(-w2, +h2, -d2), TexCoord = new Vector2(1, 0)},
				// top
				new Vertex {Position = new Vector3(-w2, +h2, d2), TexCoord = new Vector2(0, 1)},
				new Vertex {Position = new Vector3(-w2, +h2, -d2), TexCoord = new Vector2(0, 0)},
				new Vertex {Position = new Vector3(+w2, +h2, -d2), TexCoord = new Vector2(1, 0)},
				new Vertex {Position = new Vector3(+w2, +h2, d2), TexCoord = new Vector2(1, 1)},
				// bottom
				new Vertex {Position = new Vector3(-w2, -h2, d2), TexCoord = new Vector2(1, 1)},
				new Vertex {Position = new Vector3(+w2, -h2, d2), TexCoord = new Vector2(0, 1)},
				new Vertex {Position = new Vector3(+w2, -h2, -d2), TexCoord = new Vector2(0, 0)},
				new Vertex {Position = new Vector3(-w2, -h2, -d2), TexCoord = new Vector2(1, 0)},
				// left
				new Vertex {Position = new Vector3(-w2, -h2, -d2), TexCoord = new Vector2(0, 1)},
				new Vertex {Position = new Vector3(-w2, +h2, -d2), TexCoord = new Vector2(0, 0)},
				new Vertex {Position = new Vector3(-w2, +h2, d2), TexCoord = new Vector2(1, 0)},
				new Vertex {Position = new Vector3(-w2, -h2, d2), TexCoord = new Vector2(1, 1)},
				// right
				new Vertex {Position = new Vector3(+w2, -h2, d2), TexCoord = new Vector2(0, 1)},
				new Vertex {Position = new Vector3(+w2, +h2, d2), TexCoord = new Vector2(0, 0)},
				new Vertex {Position = new Vector3(+w2, +h2, -d2), TexCoord = new Vector2(1, 0)},
				new Vertex {Position = new Vector3(+w2, -h2, -d2), TexCoord = new Vector2(1, 1)}
			};

			return new GeometryInfo<Vertex>
			{
				Indices = new[]
				{
					0, 1, 2, 0, 2, 3, 4, 5, 6, 4, 6, 7, 8, 9, 10,
					8, 10, 11, 12, 13, 14, 12, 14, 15, 16, 17,
					18, 16, 18, 19, 20, 21, 22, 20, 22, 23
				},
				Vertices = mV.ToArray()
			};
		}

		public static GeometryInfo<Vertex> CreateGrid<Vertex>(float width, float depth, bool isDynamic = false)
			where Vertex : struct, IVertex
		{
			float w2 = 0.5f*width, d2 = 0.5f*depth;
			var mV = new[]
			{
				new Vertex {Position = new Vector3(-w2, 0, -d2), TexCoord = new Vector2(1, 1)},
				new Vertex {Position = new Vector3(w2, 0, -d2), TexCoord = new Vector2(0, 1)},
				new Vertex {Position = new Vector3(w2, 0, d2), TexCoord = new Vector2(0, 0)},
				new Vertex {Position = new Vector3(-w2, 0, d2), TexCoord = new Vector2(1, 0)}
			};
			return new GeometryInfo<Vertex> {Indices = new[] {0, 1, 2, 0, 2, 3}, Vertices = mV};
		}

		public static GeometryInfo<Vertex> CreateSphere<Vertex>(float radius, int sliceCount, int stackCount,
			bool isDynamic = false) where Vertex : struct, IVertex
		{
			var mI = new List<int>();
			var mV = new List<Vertex> {new Vertex {Position = new Vector3(0, radius, 0), TexCoord = new Vector2(1, 0)}};

			double phiStep = System.Math.PI/stackCount, thetaStep = 2.0f*System.Math.PI/sliceCount;

			for (var i = 1; i <= stackCount - 1; i++)
			{
				var phi = i*phiStep;
				for (var j = 0; j <= sliceCount; j++)
				{
					var theta = j*thetaStep;
					var p = new Vector3(
						(float) (radius*System.Math.Sin(phi)*System.Math.Cos(theta)),
						(float) (radius*System.Math.Cos(phi)),
						-(float) (radius*System.Math.Sin(phi)*System.Math.Sin(theta))
						);

					var uv = new Vector2((float) (theta/(System.Math.PI*2)), (float) (phi/System.Math.PI));
					mV.Add(new Vertex {Position = p, TexCoord = uv});
				}
			}
			mV.Add(new Vertex {Position = new Vector3(0, -radius, 0), TexCoord = new Vector2(0, 1)});

			for (var i = 1; i <= sliceCount; i++)
			{
				mI.Add(0);
				mI.Add(i + 1);
				mI.Add(i);
			}
			int baseIndex = 1, ringVertexCount = sliceCount + 1;
			for (var i = 0; i < stackCount - 2; i++)
			{
				for (var j = 0; j < sliceCount; j++)
				{
					mI.Add(baseIndex + i*ringVertexCount + j);
					mI.Add(baseIndex + i*ringVertexCount + j + 1);
					mI.Add(baseIndex + (i + 1)*ringVertexCount + j);

					mI.Add(baseIndex + (i + 1)*ringVertexCount + j);
					mI.Add(baseIndex + i*ringVertexCount + j + 1);
					mI.Add(baseIndex + (i + 1)*ringVertexCount + j + 1);
				}
			}
			var southPoleIndex = mV.Count - 1;
			baseIndex = southPoleIndex - ringVertexCount;
			for (var i = 0; i < sliceCount; i++)
			{
				mI.Add(southPoleIndex);
				mI.Add(baseIndex + i);
				mI.Add(baseIndex + i + 1);
			}

			return new GeometryInfo<Vertex> {Indices = mI.ToArray(), Vertices = mV.ToArray()};
		}
	}
}