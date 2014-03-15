using System;

namespace Ormeli.CG
{
    [Serializable]
    public enum CGparameterclass
    {
        Unknown = 0,
        Scalar = 1,
        Vector = 2,
        Matrix = 3,
        Struct = 4,
        Array = 5,
        Sampler = 6,
        Object = 7
    }
}
