using System;
using Ormeli.Math;

namespace Ormeli.Graphics.Effects
{
    public class ColTexEffect : Effect
    {
        private const string TEXTUREName = "Decal";
        private const string MATRIXName = "WVP";
        private IntPtr _texture;
        private IntPtr _matrix;

        protected override void InitEffect()
        {
            _texture = Base.GetParameterByName(TEXTUREName);
            _matrix = Base.GetParameterByName(MATRIXName);
        }

        protected override void InitAttribs()
        {
            Base.InitAttrib("Color", ColorVertex.Number);
            Base.InitAttrib("Texture", TextureVertex.Number);
        }

        public void SetTexture(int tex)
        {
            Base.SetTexture(_texture, tex);
        }

        public void SetMatrix(Matrix mt)
        {
            Base.SetMatrix(_matrix, mt);
        }

    }
}
