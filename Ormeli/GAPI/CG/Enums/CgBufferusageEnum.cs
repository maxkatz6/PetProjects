using System;

namespace Ormeli.Cg
{
    public partial class CG
    {
        [Serializable]
        public enum BufferUsage
        {
            StreamDraw = 0,
            StreamRead = 1,
            StreamCopy = 2,
            StaticDraw = 3,
            StaticRead = 4,
            StaticCopy = 5,
            DynamicDraw = 6,
            DynamicRead = 7,
            DynamicCopy = 8
        }
    }
}
