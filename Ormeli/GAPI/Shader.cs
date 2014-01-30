using Ormeli.Core.Patterns;

namespace Ormeli
{
    internal abstract class Shader : Disposable
    {
        public abstract void Render();
        public abstract void Initialize(string fileName);
    }
}
