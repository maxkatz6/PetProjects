using Ormeli.GAPI.Interfaces;
using Ormeli.Graphics.Components;

namespace Ormeli.Graphics.Managers
{
    public static class EffectManager
    {
        private const int MaxAttrContainers = 20;
        private const int MaxEffectCount = 10;

        public static Effect[] Effects = new Effect[MaxEffectCount];
        public static IAttribsContainer[] AttribsContainers = new IAttribsContainer[MaxAttrContainers];
        public static Attrib[][] Attribs = new Attrib[MaxAttrContainers][];

        private static int _lastAddedEffect;
        static EffectManager()
        {
            Attribs[BitmapVertex.Number] = BitmapVertex.Attribs;
            Attribs[ColorVertex.Number] = ColorVertex.Attribs;
            Attribs[TextureVertex.Number] = TextureVertex.Attribs;
        }

        public static void AddAttribute(int num, Attrib[] a)
        {
            Attribs[num] = a;
        }

        public static int AddEffect(Effect effect)
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
