using Assimp;
using SharpDX;

namespace TDFExample_ModelConverter.Core
{
    public static class Helper
    {

        /// <summary>
        /// Convert Assimp.Color4D to engine Color
        /// </summary>
        /// <param name="inputColor">Input Color</param>
        /// <returns></returns>
        public static Color ToColor(this Color4D inputColor)
        {
            return new Color(inputColor.R, inputColor.G, inputColor.B, inputColor.A);
        }

        public static Color ToColor(this System.Drawing.Color inputColor)
        {
            return new Color(inputColor.R, inputColor.G, inputColor.B, inputColor.A);
        }

        /// <summary>
        /// Convert from Assimp.Material to engine Material
        /// </summary>
        /// <param name="m">The material</param>
        /// <returns></returns>
        public static TDF.Graphics.Data.Material ToMaterial(this Material m)
        {
            var ret = new TDF.Graphics.Data.Material
            {
                Ambient = new Color4(m.ColorAmbient.A, m.ColorAmbient.R, m.ColorAmbient.G, m.ColorAmbient.B),
                Diffuse = new Color4(m.ColorDiffuse.A, m.ColorAmbient.R, m.ColorAmbient.G, m.ColorAmbient.B),
                Specular = new Color4(m.Shininess, m.ColorSpecular.R, m.ColorSpecular.G, m.ColorSpecular.B),
                Reflect = new Color4(m.ColorReflective.A, m.ColorReflective.R, m.ColorReflective.G, m.ColorReflective.B)
            };

            if (ret.Ambient == new Color4(0, 0, 0, 0))
            {
                ret.Ambient = Color.Gray;
            }
            if (ret.Diffuse == new Color4(0, 0, 0, 0) || ret.Diffuse == Color.Black)
            {
                ret.Diffuse = Color.White;
            }

            if (m.ColorSpecular == new Color4D(0, 0, 0, 0) || m.ColorSpecular == new Color4D(0, 0, 0))
            {
                ret.Specular = new Color4(ret.Specular.Alpha, 0.5f, 0.5f, 0.5f);
            }
            return ret;
        }

        /// <summary>
        /// Convert Assimp.Vector3D to engine Vector2. Z coord will lose
        /// </summary>
        /// <param name="vector">Assimp.Vector3D.</param>
        /// <returns></returns>
        public static Vector2 ToVector2(this Vector3D vector)
        {
            return new Vector2(vector.X, vector.Y);
        }

        /// <summary>
        /// Convert Assimp.Vector3D to engine Vector3
        /// </summary>
        /// <param name="vector">Assimp.Vector3D.</param>
        /// <returns></returns>
        public static Vector3 ToVector3(this Vector3D vector)
        {
            return new Vector3(vector.X, vector.Y, vector.Z);
        }
    }
}
