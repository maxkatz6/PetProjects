using System;
using Lain.Graphics.Drawable;
using SharpDX;

namespace Lain.Graphics
{
	public partial class GeometryGenerator
	{
		public static GeometryInfo<Vertex> CreateHemiSphere<Vertex>(float diameter = 1.0f, int tessellation = 16) where Vertex : struct, IVertex
		{
			if (tessellation < 3) tessellation = 3;

			int verticalSegments = tessellation;
			int horizontalSegments = tessellation * 2;

			var vertices = new Vertex[(verticalSegments / 2 + 1) * (horizontalSegments + 1)];
			var indices = new int[(verticalSegments / 2) * (horizontalSegments + 1) * 6];

			float radius = diameter / 2;

			int vertexCount = 0;
			// Create rings of vertices at progressively higher latitudes.
			for (int i = verticalSegments; i >= verticalSegments / 2; i--)
			{
				float v = 1.0f - (float)i * 2 / verticalSegments;

				var latitude = (float)((i * Math.PI / verticalSegments) - Math.PI / 2.0);
				var dy = (float)Math.Sin(latitude);
				var dxz = (float)Math.Cos(latitude);

				// Create a single ring of vertices at this latitude.
				for (int j = 0; j <= horizontalSegments; j++)
				{
					float u = (float)j / horizontalSegments;

					var longitude = (float)(j * 2.0 * Math.PI / horizontalSegments);
					var dx = (float)Math.Sin(longitude);
					var dz = (float)Math.Cos(longitude);

					dx *= dxz;
					dz *= dxz;

					var normal = new Vector3(dx, dy, dz);
					var textureCoordinate = new Vector2(u, v);
					vertices[vertexCount++] = new Vertex { Position = normal * radius, Normal = normal, TexCoord = textureCoordinate };
				}
			}

			// Fill the index buffer with triangles joining each pair of latitude rings.
			int stride = horizontalSegments + 1;

			int indexCount = 0;
			for (int i = 0; i < verticalSegments / 2; i++)
			{
				for (int j = 0; j <= horizontalSegments; j++)
				{
					int nextI = i + 1;
					int nextJ = (j + 1) % stride;

					indices[indexCount++] = (i * stride + j);
					indices[indexCount++] = (nextI * stride + j);
					indices[indexCount++] = (i * stride + nextJ);

					indices[indexCount++] = (i * stride + nextJ);
					indices[indexCount++] = (nextI * stride + j);
					indices[indexCount++] = (nextI * stride + nextJ);
				}
			}

			return new GeometryInfo<Vertex> { Indices = indices, Vertices = vertices };
		} 
	}
}