using Ormeli.Cg;

namespace Ormeli.Graphics
{
    public static class EffectManager
    {
        private const int MaxAttrContainers = 20;
        private const int MaxEffectCount = 10;

        public static CgEffect[] Effects = new CgEffect[MaxEffectCount];
        public static IAttribsContainer[] AttribsContainers = new IAttribsContainer[MaxAttrContainers];
        public static Attrib[][] Attribs = new Attrib[MaxAttrContainers][];

        private static int _lastAddedEffect;
        static EffectManager()
        {
            Attribs[BitmapVertex.Number] = BitmapVertex.Attribs;
            Attribs[ColorVertex.Number] = ColorVertex.Attribs;
            Attribs[TextureVertex.Number] = TextureVertex.Attribs;
        }

        public static int AddEffect(CgEffect effect)
        {
            Effects[_lastAddedEffect++] = effect;
            return _lastAddedEffect;
        }

        public static void AcceptAttributes(int num)
        {
            AttribsContainers[num].Accept();
        }
    }
}
