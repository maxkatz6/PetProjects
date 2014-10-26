using System;
using Ormeli.Graphics.Cameras;

namespace Ormeli.Graphics.Effects
{
    public class FontEffect : Effect
    {

        private const string TEXTUREName = "FontTexture";
        private const string MATRIXName = "Ortho";
        private IntPtr _texture;
        private IntPtr _matrix;

        protected override void InitEffect()
        {
            _texture = Base.GetParameterByName(TEXTUREName);
            _matrix = Base.GetParameterByName(MATRIXName);
        }

        protected override void InitAttribs()
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
