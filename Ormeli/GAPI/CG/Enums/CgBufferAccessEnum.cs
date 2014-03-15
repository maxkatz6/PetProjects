using System;

namespace Ormeli.CG
{
    [Serializable]
    public enum CGbufferaccess
    {
        /// <summary>
        /// The application can read but not write through the data pointer.
        /// </summary>
        MapRead = 0,
        /// <summary>
        /// The application can write but not read through the data pointer. 
        /// </summary>
        MapWrite = 1,
        /// <summary>
        /// The application can read and write through the data pointer. 
        /// </summary>
        MapReadWrite = 2,
        /// <summary>
        /// Same as CG_MAP_READ_WRITE if using a GL buffer.
        /// </summary>
        MapWriteDiscard = 3,
        /// <summary>
        /// Same as CG_MAP_READ_WRITE if using a GL buffer.
        /// </summary>
        MapWriteNoOverwrite = 4
    }
}
