using System;

namespace Ormeli.CG
{
    [Serializable]
    public enum CGerror
    {
        /// <summary>
        /// No error has occurred.
        /// </summary>
        NoError = 0,
        /// <summary>
        /// The compile returned an error.
        /// </summary>
        Compiler = 1,
        /// <summary>
        /// The parameter used is invalid.
        /// </summary>
        InvalidParameter = 2,
        /// <summary>
        /// The profile is not supported.
        /// </summary>
        InvalidProfile = 3,
        /// <summary>
        /// The program could not load.
        /// </summary>
        ProgramLoad = 4,
        /// <summary>
        /// The program could not bind.
        /// </summary>
        ProgramBind = 5,
        /// <summary>
        /// The program must be loaded before this operation may be used.
        /// </summary>
        ProgramNotLoaded = 6,
        /// <summary>
        /// An unsupported GL extension was required to perform this operation.
        /// </summary>
        UnsupportedGLExtension = 7,
        /// <summary>
        /// An unknown value type was assigned to a parameter.
        /// </summary>
        InvalidValueType = 8,
        /// <summary>
        /// The parameter is not of matrix type.
        /// </summary>
        NotMatrixParam = 9,
        /// <summary>
        /// The enumerant parameter has an invalid value.
        /// </summary>
        InvalidEnumerant = 10,
        /// <summary>
        /// The parameter must be a 4x4 matrix type.
        /// </summary>
        Not_4X4Matrix = 11,
        /// <summary>
        /// The file could not be read.
        /// </summary>
        FileRead = 12,
        /// <summary>
        /// The file could not be written.
        /// </summary>
        FileWrite = 13,
        /// <summary>
        /// Nvparse could not successfully parse the output from the Cg compiler backend.
        /// </summary>
        Nvparse = 14,
        /// <summary>
        /// Memory allocation failed.
        /// </summary>
        MemoryAlloc = 15,
        /// <summary>
        /// Invalid context handle.
        /// </summary>
        InvalidContextHandle = 16,
        /// <summary>
        /// Invalid program handle.
        /// </summary>
        InvalidProgramHandle = 17,
        /// <summary>
        /// Invalid parameter handle.
        /// </summary>
        InvalidParamHandle = 18,
        /// <summary>
        /// The specified profile is unknown.
        /// </summary>
        UnknownProfile = 19,
        /// <summary>
        /// The variable arguments were specified incorrectly.
        /// </summary>
        VarArg = 20,
        /// <summary>
        /// The dimension value is invalid.
        /// </summary>
        InvalidDimension = 21,
        /// <summary>
        /// The parameter must be an array.
        /// </summary>
        ArrayParam = 22,
        /// <summary>
        /// Index into the array is out of bounds.
        /// </summary>
        OutOfArrayBounds = 23,
        /// <summary>
        /// A type being added to the context conflicts with an existing type.
        /// </summary>
        ConflictingTypes = 24,
        /// <summary>
        /// A type being added to the context conflicts with an existing type.
        /// </summary>
        ConflictingParameterTypes = 25,
        /// <summary>
        /// The parameter must be global.
        /// </summary>
        ParameterIsNotShared = 26,
        /// <summary>
        /// The parameter could not be changed to the given variability.
        /// </summary>
        InvalidParameterVariability = 27,
        /// <summary>
        /// Cannot destroy the parameter.It is bound to other parameters or is not a root parameter.
        /// </summary>
        CannotDestroyParameter = 28,
        /// <summary>
        /// The parameter is not a root parameter.
        /// </summary>
        NotRootParameter = 29,
        /// <summary>
        /// The two parameters being bound do not match.
        /// </summary>
        ParametersDoNotMatch = 30,
        /// <summary>
        /// The parameter is not a program parameter.
        /// </summary>
        IsNotProgramParameter = 31,
        /// <summary>
        /// The type of the parameter is invalid.
        /// </summary>
        InvalidParameterType = 32,
        /// <summary>
        /// The parameter must be a resizable array.
        /// </summary>
        ParameterIsNotResizableArray = 33,
        /// <summary>
        /// The size value is invalid.
        /// </summary>
        InvalidSize = 34,
        /// <summary>
        /// Cannot bind the given parameters. Binding will form a cycle.
        /// </summary>
        BindCreatesCycle = 35,
        /// <summary>
        /// Cannot bind the given parameters.  Array types do not match.
        /// </summary>
        ArrayTypesDoNotMatch = 36,
        /// <summary>
        /// Cannot bind the given parameters. Array dimensions do not match.
        /// </summary>
        ArrayDimensionsDoNotMatch = 37,
        /// <summary>
        /// The array is has the wrong dimension.
        /// </summary>
        ArrayHasWrongDimension = 38,
        /// <summary>
        /// Connecting the parameters failed because The type of the 
        /// source parameter is not defined within the given program 
        /// or does not match the type with the same name in the program.
        /// </summary>
        TypeIsNotDefinedInProgram = 39,
        /// <summary>
        /// Invalid effect handle.
        /// </summary>
        InvalidEffectHandle = 40,
        /// <summary>
        /// Invalid state handle.
        /// </summary>
        InvalidStateHandle = 41,
        /// <summary>
        /// Invalid state assignment handle.
        /// </summary>
        InvalidStateAssignmentHandle = 42,
        /// <summary>
        /// Invalid pass handle.
        /// </summary>
        InvalidPassHandle = 43,
        /// <summary>
        /// Invalid annotation handle.
        /// </summary>
        InvalidAnnotationHandle = 44,
        /// <summary>
        /// Invalid technique handle.
        /// </summary>
        InvalidTechniqueHandle = 45,
        /// <summary>
        /// Invalid parameter handle.
        /// Do not use this! Use INVALID_PARAM_HANDLE instead.
        /// </summary>
        InvalidParameterHandle = 46,
        /// <summary>
        /// Invalid parameter handle.
        /// </summary>
        StateAssignmentTypeMismatch = 47,
        /// <summary>
        /// Invalid function handle.
        /// </summary>
        InvalidFunctionHandle = 48,
        /// <summary>
        /// Technique did not pass validation.
        /// </summary>
        InvalidTechnique = 49,
        /// <summary>
        /// The supplied pointer is NULL.
        /// </summary>
        InvalidPointer = 50,
        /// <summary>
        /// Not enough data was provided.
        /// </summary>
        NotEnoughData = 51,
        /// <summary>
        /// The parameter is not of a numeric type.
        /// </summary>
        NonNumericParameter = 52,
        /// <summary>
        /// The specified array sizes are not compatible with the given array.
        /// </summary>
        ArraySizeMismatch = 53,
        CannotSetNonUniformParameter = 54,
        DuplicateName = 55,
        InvalidObjHandle = 56,
        InvalidBufferHandle = 57,
        BufferIndexOutOfRange = 58,
        BufferAlreadyMapped = 59,
        BufferUpdateNotAllowed = 60,
        GLSLgUncombinedLoad = 61,
        ErrorMax
    }
}
