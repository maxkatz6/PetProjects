using System;

namespace Ormeli.Cg
{
    public partial class CG
    {
        [Serializable]
        public enum Domain
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
}