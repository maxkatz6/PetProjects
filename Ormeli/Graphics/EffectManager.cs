using System.Collections.Generic;
using Ormeli.Cg;

namespace Ormeli.Graphics
{
    public static class EffectManager
    {
        private const int MaxAttrContainers = 20;

        public static List<CgEffect> Effects = new List<CgEffect>();
        public static IAttribsContainer[] AttribsContainers = new IAttribsContainer[MaxAttrContainers];
        public static Attrib[][] Attribs = new Attrib[MaxAttrContainers][];

        static EffectManager()
        {
            Attribs[ColorVertex.Number] = ColorVertex.Attribs;
            Attribs[TextureVertex.Number] = TextureVertex.Attribs;
        }
    }
}
