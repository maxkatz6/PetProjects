using Ormeli.GAPI.Interfaces;
using Ormeli.Graphics.Components;

namespace Ormeli.Graphics.Managers
{
    public static class EffectManager
    {
        private const int MaxAttrContainers = 20;

        public static IAttribsContainer[] AttribsContainers = new IAttribsContainer[MaxAttrContainers];
        public static Attrib[][] Attribs = new Attrib[MaxAttrContainers][];

        static EffectManager()
        {
            Attribs[BitmapVertex.Number] = BitmapVertex.Attribs;
            Attribs[ColorVertex.Number] = ColorVertex.Attribs;
            Attribs[TextureVertex.Number] = TextureVertex.Attribs;
        }

        public static void AcceptAttributes(int num)
        {
            AttribsContainers[num].Accept();
        }
    }
}
