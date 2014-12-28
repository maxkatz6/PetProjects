using System.Collections.Generic;
using Ormeli.Core.Patterns;
using Ormeli.Graphics.Builders;
using SharpDX;

namespace Ormeli.Graphics.Components
{
    public class Scene : Disposable
    {
        private readonly List<Object> _objects = new List<Object>(1000);

        public void AddObject(Model model, Vector3 pos)
        {
            _objects.Add(new Object { Model = model, WorldMatrix = Matrix.Translation(pos) });
        }
        public void AddObject(string json, Vector3 pos)
        {
            string file = json; //...will get file name from json
            Model model = ModelBuilder.Create().LoadFromFile(file);
            _objects.Add(new Object { Model = model, WorldMatrix = Matrix.Translation(pos) });
        }

        public void Render()
        {
            foreach (var m in _objects)
                m.Model.Draw(m.WorldMatrix);
        }
    }
}
