using System;

namespace Ormeli.Cg
{
    public partial class CG
    {
        #region Enumerations

        [Serializable]
        public enum GlEnum
        {
            /// <summary>
            /// Identity matrix.
            /// </summary>
            MatrixIdentity = 0,

            /// <summary>
            /// Transpose matrix.
            /// </summary>
            MatrixTranspose = 1,

            /// <summary>
            /// Inverse matrix.
            /// </summary>
            MatrixInverse = 2,

            /// <summary>
            /// Inverse and transpose the matrix.
            /// </summary>
            MatrixInverseTranspose = 3,

            /// <summary>
            /// Modelview matrix.
            /// </summary>
            ModelviewMatrix = 4,

            /// <summary>
            /// Projection matrix.
            /// </summary>
            ProjectionMatrix = 5,

            /// <summary>
            /// Texture matrix.
            /// </summary>
            TextureMatrix = 6,

            /// <summary>
            /// Concatenated modelview and projection matrices.
            /// </summary>
            ModelviewProjectionMatrix = 7,

            /// <summary>
            /// Vertex profile (returned by cgGLGetLatestProfile)
            /// </summary>
            Vertex = 8,

            /// <summary>
            /// Fragment profile (returned by cgGLGetLatestProfile)
            /// </summary>
            Fragment = 9,

            /// <summary>
            /// Geometry profile (returned by cgGLGetLatestProfile)
            /// </summary>
            Geometry = 10,

            /// <summary>
            /// Tessellation control.
            /// </summary>
            TessellationControl = 11,

            /// <summary>
            /// Tessellation evaluation.
            /// </summary>
            TessellationEvaluation = 12
        }

        [Serializable]
        public enum GlMatrixTransform
        {
            MatrixIdentity = 0,
            MatrixTranspose = 1,
            MatrixInverse = 2,
            MatrixInverseTranspose = 3
        }

        [Serializable]
        public enum GlMatrixType
        {
            ModelviewMatrix = 4,
            ProjectionMatrix = 5,
            TextureMatrix = 6,
            ModelviewProjectionMatrix = 7
        }

        [Serializable]
        public enum GlProfile
        {
            Vertex = 8,
            Fragment = 9,
            Geometry = 10
        }

        [Serializable]
        public enum GlTessellation
        {
            Control = 11,
            Evaluation = 12
        }

        #endregion
    }
}