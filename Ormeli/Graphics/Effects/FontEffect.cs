using System;
using Ormeli.Graphics.Cameras;

namespace Ormeli.Graphics.Effects
{
    public class FontEffect : Effect
    {

        private const string TEXTUREName = "FontTexture";
        private const string MATRIXName = "Ortho";
        private readonly IntPtr _texture;
        private readonly IntPtr _matrix;

        public FontEffect(string file) : base(file)
        {
            _texture = Base.GetParameterByName(TEXTUREName);
            _matrix = Base.GetParameterByName(MATRIXName);
        }

        protected override void InitEffect()
        {
            Base.InitAttrib("Font", ColorVertex.Number);
        }

        public void SetFontTexture(Texture tex)
        {
            Base.SetTexture(_texture, tex);
        }

        public void SetFontDesc()
        {
            
        }

        public void UpdateOrthoMatrix()
        {
            Base.SetMatrix(_matrix, Camera.Current.Ortho);
        }

    }
}
