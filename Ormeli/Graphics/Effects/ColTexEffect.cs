using System;
using Ormeli.GAPI.Interfaces;
using Ormeli.Graphics.Components;

namespace Ormeli.Graphics.Effects
{
    public class ColTexEffect : Effect
    {
        private const string TEXTUREName = "Decal";
        private IntPtr _texture;

        protected override void InitEffect()
        {
            _texture = Base.GetParameterByName(TEXTUREName);
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

        protected override void RenderMesh(Mesh mesh)
        {
            SetTexture(mesh.TextureN);
        }
    }
}
