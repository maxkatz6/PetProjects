using Ormeli.Core.Patterns;
using Ormeli.Graphics.Components;
using Ormeli.Graphics.Managers;
using SharpDX;

namespace Ormeli.Graphics
{
    public class Mesh : IDrawable
    {
        internal Buffer Vb, Ib;

        public bool IsDynamic { get; set; }
        public int ShaderN { get; set; }
        public Texture Texture { get; set; }
        public int VertexSize { get; set; }
        public int IndexCount { get; set; }
        public string Tech { get; set; }

        public virtual void Draw(Matrix matrix)
        {
            EffectManager.Effects[ShaderN].SetMatrix(matrix);
            EffectManager.Effects[ShaderN].Render(this);
        }
    }
}
