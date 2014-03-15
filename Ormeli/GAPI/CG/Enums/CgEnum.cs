using System;

namespace Ormeli.CG
{
    [Serializable]
    public enum CGenum
    {
        /// <summary>
        /// Unknown resource.
        /// </summary>
        Unknown = 4096,
        /// <summary>
        /// Specifies an input parameter.
        /// </summary>
        In = 4097,
        /// <summary>
        /// Specifies an output parameter.
        /// </summary>
        Out = 4098,
        /// <summary>
        /// Specifies a parameter that is both input and output.
        /// </summary>
        InOut = 4099,
        /// <summary>
        /// A structure parameter that contains parameters that differ in variability.
        /// </summary>
        Mixed = 4100,
        /// <summary>
        /// A varying parameter is one whose value changes with each invocation of the program.
        /// </summary>
        Varying = 4101,
        /// <summary>
        /// A uniform parameter is one whose value does not chance with each invocation of a program, but whose value can change between groups of program invocations.
        /// </summary>
        Uniform = 4102,
        /// <summary>
        /// A constant parameter never changes for the life of a compiled program. Modifying a constant parameter requires program recompilation.
        /// </summary>
        Constant = 4103,
        /// <summary>
        /// The original Cg source program.
        /// </summary>
        ProgramSource = 4104,    /* GetProgramString */
        /// <summary>
        /// The main entry point for the program.
        /// </summary>
        ProgramEntry = 4105,     /* GetProgramString */
        /// <summary>
        /// The string for the compiled program.
        /// </summary>
        CompiledProgram = 4106,  /* GetProgramString */
        /// <summary>
        /// The profile for the program.
        /// </summary>
        ProgramProfile = 4107,   /* GetProgramString */
        /// <summary>
        /// A global.
        /// </summary>
        Global = 4108,
        /// <summary>
        /// The program.
        /// </summary>
        Program = 4109,
        /// <summary>
        /// The default values for a uniform parameter.
        /// </summary>
        Default = 4110,
        /// <summary>
        /// An error.
        /// </summary>
        Error = 4111,
        /// <summary>
        /// A string that contains Cg source code.
        /// </summary>
        Source = 4112,
        /// <summary>
        /// A string that contains object code that resulted from the precompilation of some Cg source code.
        /// </summary>
        Object = 4113,
        CompileManual = 4114,
        CompileImmediate = 4115,
        CompileLazy = 4116,
        Current = 4117,
        Literal = 4118,
        Version = 4119,     /* GetString                              */
        RowMajor = 4120,
        ColumnMajor = 4121,
        Fragment = 4122,    /* GetProgramInput and GetProgramOutput   */
        Vertex = 4123,      /* GetProgramInput and GetProgramOutput   */
        Point = 4124,       /* Geometry program GetProgramInput       */
        Line = 4125,        /* Geometry program GetProgramInput       */
        LineAdj = 4126,     /* Geometry program GetProgramInput       */
        Triangle = 4127,    /* Geometry program GetProgramInput       */
        TriangleAdj = 4128, /* Geometry program GetProgramInput       */
        PointOut = 4129,    /* Geometry program GetProgramOutput      */
        LineOut = 4130,     /* Geometry program GetProgramOutput      */
        TriangleOut = 4131, /* Geometry program GetProgramOutput      */
        ImmediateParameterSetting = 4132,
        DeferredParameterSetting = 4133,
        NoLocksPolicy = 4134,
        ThreadSafePolicy = 4135,
        ForceUpperCasePolicy = 4136,
        UnchangedCasePolicy = 4137,
        IsOpenglProfile = 4138,
        IsDirect3DProfile = 4139,
        IsDirect3D8Profile = 4140,
        IsDirect3D9Profile = 4141,
        IsDirect3D10Profile = 4142,
        IsVertexProfile = 4143,
        IsFragmentProfile = 4144,
        IsGeometryProfile = 4145,
        IsTranslationProfile = 4146,
        IsHLSLProfile = 4147,
        IsGLSLProfile = 4148,
        IsTessellationControlProfile = 4149,
        IsTessellationEvaluationProfile = 4150,
        Patch = 4152,       /* GetProgramInput and GetProgramOutput */
        IsDirect3D11Profile = 4153
    }
}
