﻿using Ormeli.Cg;
using Ormeli.Math;

namespace Ormeli.Graphics.Effects
{
    public class ColTexEffect : CgEffect
    {
        private const string TEXTUREName = "Decal";
        private const string MATRIXName = "WVP";
        private readonly CG.Parameter _texture;
        private readonly CG.Parameter _matrix;

        public ColTexEffect(string file)
            : base(file)
        {
            InitAttrib("Color", ColorVertex.Number);
            InitAttrib("Texture", TextureVertex.Number);
            _texture = CG.GetNamedEffectParameter(Effect, TEXTUREName);
            _matrix = CG.GetNamedEffectParameter(Effect, MATRIXName);
        }

        public void SetTexture(Texture tex)
        {
            SetTexture(_texture, tex);
        }

        public void SetMatrix(Matrix mt)
        {
            SetMatrix(_matrix, mt);
        }
    }
}
