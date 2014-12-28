using System;

namespace Ormeli.GAPI.Interfaces
{
    public enum AttribIndex
    {
        // ReSharper disable UnusedMember.Local
        Position = 0, //Input Vertex, Generic Attribute 0

        BlendWeight = 1, //	Input vertex weight, Generic Attribute 1
        Normal = 2, //Input normal, Generic Attribute 2
        Color = 3, // 	Input primary color, Generic Attribute 3
        Diffuse = 3,
        Specular = 4,
        Color1 = 4, // 	Input secondary color, Generic Attribute 4
        TessFactor = 5,
        FogCoord = 5, // 	Input fog coordinate, Generic Attribute 5
        PSize = 6, //Input point size, Generic Attribute 6
        BlendIindices = 7, //	Generic Attribute 7
        TexCoord = 8,
        TexCoord1 = 9,
        TexCoord2 = 10,
        TexCoord3 = 11,
        TexCoord4 = 12,
        TexCoord5 = 13,
        TexCoord6 = 14,
        TexCoord7 = 15, //Input texture coordinates (texcoord0-texcoord7), Generic Attributes 8-15
        Tangent = 14, //Generic Attribute 14
        Binormal = 15, //Generic Attribute 15
        // ReSharper restore UnusedMember.Local
    }

    public struct Attrib
    {
        public enum OrmeliType
        {
            Float = 0,
            Uint = 1,
            Int = 2
        }
        public AttribIndex AttribIndex;
        public int Offset;
        public int Num;
        public OrmeliType Type;

        public Attrib(AttribIndex attribIndex, int num, OrmeliType type, int offset)
        {
            AttribIndex = attribIndex;
            Num = num;
            Type = type;
            Offset = offset;
        }
    }
    public interface IAttribsContainer
    {
        void Initialize(Attrib[] attribs, IntPtr blobPointer);
        void Accept();
    }
}
