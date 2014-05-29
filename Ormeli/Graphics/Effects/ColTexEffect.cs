using System;
using Ormeli.Math;

namespace Ormeli.Graphics.Effects
{
    public class ColTexEffect : Effect
    {
        private const string TEXTUREName = "Decal";
        private const string MATRIXName = "WVP";
        private readonly IntPtr _texture;
        private readonly IntPtr _matrix;

        public ColTexEffect(string file) : base(file)
        {
            _texture = Base.GetParameterByName(TEXTUREName);
            _matrix = Base.GetParameterByName(MATRIXName);
        }

        protected override void InitEffect()
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
