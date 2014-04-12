using System;

namespace Ormeli.Cg
{
    public partial class CG
    {
        [Serializable]
        public enum Profile
        {
            /// <summary>
            /// Start profile.
            /// </summary>
            Start = 6144,

            /// <summary>
            /// Unknown profile.
            /// </summary>
            Unknown = 6145,

            /// <summary>
            /// OpenGL fragment profile for NV2x (GeForce3, GeForce4 Ti, Quadro DCC, etc.) 
            /// <para>Introduced by GeForce3.</para>
            /// </summary>
            Vp20 = 6146,

            /// <summary>
            /// OpenGL fragment profile for NV2x (GeForce3, GeForce4 Ti, Quadro DCC, etc.) 
            /// <para>Introduced by GeForce3.</para>
            /// </summary>
            Fp20 = 6147,

            /// <summary>
            /// OpenGL fragment profile for NV3x (GeForce FX, Quadro FX, etc.) 
            /// <para>Introduced by the GeForce FX and Quadro FX line of NVIDIA GPUs.</para>
            /// </summary>
            Vp30 = 6148,

            /// <summary>
            /// OpenGL fragment profile for NV3x (GeForce FX, Quadro FX, etc.)
            /// <para>Introduced by the GeForce FX and Quadro FX line of NVIDIA GPUs.</para>
            /// </summary>
            Fp30 = 6149,

            /// <summary>
            /// OpenGL vertex profile for multi-vendor ARB_vertex_program extension. 
            /// <para>Introduced by GeForce3 and Quadro DCC GPUs. ATI GPUs also support this extension.</para>
            /// </summary>
            Arbvp1 = 6150,

            /// <summary>
            /// OpenGL fragment profile for NVIDIA GeForce 6/7 Series, NV4x-based Quadro FX. 
            /// <para>Introduced by the GeForce 6800 and other NV4x-based NVIDIA GPUs.</para>
            /// </summary>
            Fp40 = 6151,

            /// <summary>
            /// OpenGL fragment profile for multi-vendor ARB_fragment_program extension. 
            /// <para>Introduced by GeForce FX and other DirectX 9 GPUs. ATI GPUs also support this extension.</para>
            /// </summary>
            Arbfp1 = 7000,

            /// <summary>
            /// OpenGL vertex profile for NVIDIA GeForce 6/7 Series, NV4x-based Quadro FX. 
            /// <para>Introduced by the GeForce 6800 and other NV4x-based NVIDIA GPUs.</para>
            /// </summary>
            Vp40 = 7001,

            /// <summary>
            /// OpenGL vertex profile for the OpenGL Shading Lanauge (GLSL). 
            /// </summary>
            GLSLv = 7007,

            /// <summary>
            /// OpenGL fragment profile for the OpenGL Shading Lanauge (GLSL). 
            /// </summary>
            GLSLf = 7008,

            /// <summary>
            /// OpenGL geometry profile for the OpenGL Shading Lanauge (GLSL). 
            /// </summary>
            GLSLg = 7016,

            /// <summary>
            /// OpenGL Tessellation Control programs.
            /// </summary>
            GLSLc = 7009,

            /// <summary>
            /// Deprecated alias for GP4FP.
            /// </summary>
            GPUFp = 7010,

            /// <summary>
            /// Deprecated alias for GP4VP. 
            /// </summary>
            GPUVp = 7011,

            /// <summary>
            /// Deprecated alias for GP4GP. 
            /// </summary>
            GPUGp = 7012,

            /// <summary>
            /// OpenGL fragment profile for NVIDIA GeForce 8/9/100/200/300 Series, OpenGL 3.x Quadro.
            /// <para>Requires OpenGL support for the NV_gpu_program4 extension. This extension was introduced by the GeForce 6800 and other G8x-based GPUs.</para>
            /// </summary>
            Gp4Fp = 7010,

            /// <summary>
            /// OpenGL vertex profile for NVIDIA GeForce 8/9/100/200/300 Series, OpenGL 3.x Quadro.
            /// <para>Requires OpenGL support for the NV_gpu_program4 extension. This extension was introduced by the GeForce 8800 and other G8x-based GPUs.</para>
            /// </summary>
            Gp4Vp = 7011,

            /// <summary>
            /// OpenGL geometry profile for NVIDIA GeForce 8/9/100/200/300 Series, OpenGL 3.x Quadro.
            /// <para>Requires OpenGL support for the NV_gpu_program4 extension. This extension was introduced by the GeForce 8800 and other G8x-based GPUs.</para>
            /// </summary>
            Gp4Gp = 7012,

            /// <summary>
            /// OpenGL fragment profile for NVIDIA GeForce 400 Series, OpenGL 4.x Quadro. 
            /// </summary>
            Gp5Fp = 7017,

            /// <summary>
            /// OpenGL Vertex programs. GeForce 400 Series, OpenGL 4.x Quadro.
            /// </summary>
            Gp5Vp = 7018,

            /// <summary>
            /// OpenGL geometry profile for NVIDIA GeForce 400 Series, OpenGL 4.x Quadro. 
            /// </summary>
            Gp5Gp = 7019,

            /// <summary>
            /// OpenGL Tessellation control profile for NVIDIA GeForce 400 Series, OpenGL 4.x Quadro.
            /// </summary>
            Gp5Tcp = 7020,

            /// <summary>
            /// OpenGL Tessellation evaluation profile for NVIDIA GeForce 400 Series, OpenGL 4.x Quadro. 
            /// </summary>
            Gp5Tep = 7021,

            /// <summary>
            /// Direct3D Shader Model 1.1 vertex profile for DirectX 8.
            /// <para>Introduced by GeForce 2 (NV1x) for DirectX 8.</para>
            /// </summary>
            Vs11 = 6153,

            /// <summary>
            /// Direct3D Shader Model 2.0 vertex profile for DirectX 9.
            /// <para>Introduced by GeForce FX (NV3x) for DirectX 9.</para>
            /// </summary>
            Vs20 = 6154,

            /// <summary>
            /// Direct3D Shader Model 2.0 Extended vertex profile for DirectX 9.
            /// <para>Introduced by GeForce FX (NV3x) for DirectX 9.</para>
            /// </summary>
            Vs2X = 6155,

            /// <summary>
            /// Direct3D Software Shader for Model 2.0 Extended vertex profile.
            /// <para>Introduced by GeForce FX (NV3x) for DirectX 9.</para>
            /// </summary>
            Vs2Sw = 6156,

            /// <summary>
            /// Direct3D Shader Model 3.0 vertex profile for DirectX 9.
            /// <para>Introduced by GeForce FX (NV3x) for DirectX 9.</para>
            /// </summary>
            Vs30 = 6157,

            /// <summary>
            /// Translation profile to DirectX 9's High Level Shader Language for vertex shaders. 
            /// </summary>
            HLSLv = 6158,

            /// <summary>
            /// Direct3D Shader Model 1.1 fragment profile for DirectX 8.
            /// <para>Introduced by GeForce 2 (NV1x) for DirectX 8.</para>
            /// </summary>
            Ps11 = 6159,

            /// <summary>
            /// Direct3D Shader Model 1.2 fragment profile for DirectX 8.
            /// </summ
            Ps12 = 6160,

            /// <summary>
            /// Direct3D Shader Model 1.3 fragment profile for DirectX 8.
            /// <para>Introduced by GeForce 2 (NV1x) for DirectX 8..</para>
            /// </summary>
            Ps13 = 6161,

            /// <summary>
            /// Direct3D Shader Model 2.0 fragment profile for DirectX 9.
            /// <para>Introduced by GeForce 3 (NV2x) for DirectX 8.</para>
            /// </summary>
            Ps20 = 6162,

            /// <summary>
            /// Direct3D Shader Model 2.0 Extended fragment profile for DirectX 9.
            /// <para>Introduced by GeForce FX (NV3x) for DirectX 9.</para>
            /// </summary>
            Ps2X = 6163,

            /// <summary>
            /// Direct3D Software Shader for Model 2.0 Extended fragment profile.
            /// <para>Introduced by GeForce FX (NV3x) for DirectX 9.</para>
            /// </summary>
            Ps2Sw = 6164,

            /// <summary>
            /// Direct3D Shader Model 3.0 fragment profile for DirectX 9.
            /// <para>Introduced by GeForce FX (NV3x) for DirectX 9.</para>
            /// </summary>
            Ps30 = 6165,

            /// <summary>
            /// Translation profile to DirectX 9's High Level Shader Language for pixel shaders.
            /// </summary>
            HLSLf = 6166,

            /// <summary>
            /// Translation profile to DirectX 10's High Level Shader Language for vertex shaders. 
            /// </summary>
            Vs40 = 6167,

            /// <summary>
            /// Translation profile to DirectX 10's High Level Shader Language for pixel shaders.
            /// </summary>
            Ps40 = 6168,

            /// <summary>
            /// Translation profile to DirectX 10's High Level Shader Language for geometry shaders. 
            /// </summary>
            Gs40 = 6169,

            /// <summary>
            /// Translation profile to DirectX 11's High Level Shader Language for vertex shaders.
            /// </summary>
            Vs50 = 6170,

            /// <summary>
            /// Translation profile to DirectX 11's High Level Shader Language for pixel shaders.
            /// </summary>
            Ps50 = 6171,

            /// <summary>
            /// Translation profile to DirectX 11's High Level Shader Language for geometry shaders. 
            /// </summary>
            Gs50 = 6172,

            /// <summary>
            /// Translation profile to DirectX 11's High Level Shader Language for hull shaders.
            /// </summary>
            Hs50 = 6173,

            /// <summary>
            /// Translation profile to DirectX 11's High Level Shader Language for domain shaders.
            /// </summary>
            Ds50 = 6174,

            /// <summary>
            /// DirectX 9 Vertex programs.
            /// </summary>
            Generic = 7002,

            /// <summary>
            /// Max profile.
            /// </summary>
            Max = 7100
        }
    }
}