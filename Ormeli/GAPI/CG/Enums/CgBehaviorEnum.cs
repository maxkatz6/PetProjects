using System;

namespace Ormeli.Cg
{
    public partial class CG
    {
        [Serializable]
        public enum Behavior
        {
            Unknown = 0,

            /// <summary>
            /// Latest behavior supported at runtime. 
            /// </summary>
            Latest = 1,
            Behavior2200 = 1000,

            /// <summary>
            /// Default behavior.
            /// </summary>
            Behavior3000 = 2000,

            /// <summary>
            /// Latest behavior supported at compile time.
            /// </summary>
            Current = Behavior3000
        }
    }
}
