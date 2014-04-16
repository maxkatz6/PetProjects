using Ormeli.Cg;

namespace Ormeli.Graphics.Effects
{
    public class ColTexEffect : CgEffect
    {
        private const string TEXTUREName= "Decal";
        private readonly CG.Parameter _texture;

        public ColTexEffect(string file)
            : base(file, 0)
        {
            InitAttrib("Color", ColorVertex.Number);
            InitAttrib("Texture", TextureVertex.Number);
            _texture = CG.GetNamedEffectParameter(Effect, TEXTUREName);
        }

        public void SetTexture(Texture tex)
        {
            SetTexture(_texture, tex);
        }
    }
}
