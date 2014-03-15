using System;

namespace Ormeli.CG
{
    [Serializable]
    public enum CGdomain
    {
        Unknown = 0,
        First = 1,
        Vertex = 1,
        Fragment = 2,
        Geometry = 3,
        TessellationControl = 4,
        TessellationEvaluation = 5
    }
}
