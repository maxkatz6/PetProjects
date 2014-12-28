using SharpDX;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Ormeli.Cg
{
    public static partial class CG
    {
		private const string CgLib = "cg.dll";
		private const string CgGlLib = "cgGl.dll";
		private const string CgDxLib = "cgD3D11.dll";

        public static Bool True = 1;
        public static Bool False = 0;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ErrorCallbackFuncDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ErrorHandlerFuncDelegate(Context context, Error error, IntPtr appdata);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void IncludeCallbackFuncDelegate(Context context, [In] string filename);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate Bool StateCallbackDelegate(StateAssignment cGstateassignment);

        #region Cg

        [DllImport(CgLib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "cgSetPassState"),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetPassState(Pass pass);

        [DllImport(CgLib, EntryPoint = "cgAddStateEnumerant", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void AddStateEnumerant(State state, [In] string name, int value);

        [DllImport(CgLib, EntryPoint = "cgCallStateResetCallback", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool CallStateResetCallback(StateAssignment stateassignment);

        [DllImport(CgLib, EntryPoint = "cgCallStateSetCallback", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool CallStateSetCallback(StateAssignment stateassignment);

        [DllImport(CgLib, EntryPoint = "cgCallStateValidateCallback", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool CallStateValidateCallback(StateAssignment stateassignment);

        [DllImport(CgLib, EntryPoint = "cgCombinePrograms", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Program CombinePrograms(int n, [In] Program[] exeList);

        [DllImport(CgLib, EntryPoint = "cgCombinePrograms2", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Program CombinePrograms2([In] Program program1, [In] Program program2);

        [DllImport(CgLib, EntryPoint = "cgCombinePrograms3", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Program CombinePrograms3([In] Program program1, [In] Program program2,
            [In] Program program3);

        [DllImport(CgLib, EntryPoint = "cgCombinePrograms4", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Program CombinePrograms4([In] Program program1, [In] Program program2,
            [In] Program program3, [In] Program program4);

        [DllImport(CgLib, EntryPoint = "cgCombinePrograms5", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Program CombinePrograms5([In] Program program1, [In] Program program2,
            [In] Program program3, [In] Program program4, [In] Program program5);

        [DllImport(CgLib, EntryPoint = "cgCompileProgram", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void CompileProgram(Program program);

        [DllImport(CgLib, EntryPoint = "cgConnectParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void ConnectParameter(Parameter from, Parameter to);

        [DllImport(CgLib, EntryPoint = "cgCopyEffect", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Effect CopyEffect(Effect effect);

        [DllImport(CgLib, EntryPoint = "cgCopyProgram", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Program CopyProgram(Program program);

        [DllImport(CgLib, EntryPoint = "cgCreateArraySamplerState", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern State CreateArraySamplerState(Context context, [In] string name, Type type, int nelems);

        [DllImport(CgLib, EntryPoint = "cgCreateArrayState", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern State CreateArrayState(Context context, [In] string name, Type type, int nelems);

        [DllImport(CgLib, EntryPoint = "cgCreateBuffer", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Buffer CreateBuffer(Context context, int size, [In] IntPtr data, BufferUsage bufferUsage);

        [DllImport(CgLib, EntryPoint = "cgCreateContext", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Context CreateContext();

        [DllImport(CgLib, EntryPoint = "cgCreateEffect", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi), SuppressUnmanagedCodeSecurity]
        public static extern Effect CreateEffect(Context context, [In] string code, [In] string[] args);

        [DllImport(CgLib, EntryPoint = "cgCreateEffectAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Annotation CreateEffectAnnotation(Effect effect, [In] string name, Type type);

        [DllImport(CgLib, EntryPoint = "cgCreateEffectFromFile", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Effect CreateEffectFromFile(Context context, [In] string filename, [In] string[] args);

        [DllImport(CgLib, EntryPoint = "cgCreateEffectParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter CreateEffectParameter(Effect effect, [In] string name, Type type);

        [DllImport(CgLib, EntryPoint = "cgCreateEffectParameterArray", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter CreateEffectParameterArray(Effect effect, [In] string name, Type type, int length);

        [DllImport(CgLib, EntryPoint = "cgCreateEffectParameterMultiDimArray",
            CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern Parameter CreateEffectParameterMultiDimArray(Effect effect, [In] string name, Type type,
            int dim, [In] int[] lengths);

        [DllImport(CgLib, EntryPoint = "cgCreateObj", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Obj CreateObj(Context context, Enum programType, [In] string source, Profile profile,
            [In] string[] args);

        [DllImport(CgLib, EntryPoint = "cgCreateObjFromFile", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Obj CreateObjFromFile(Context context, Enum programType, [In] string sourceFile,
            Profile profile, [In] string[] args);

        [DllImport(CgLib, EntryPoint = "cgCreateParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter CreateParameter(Context context, Type type);

        [DllImport(CgLib, EntryPoint = "cgCreateParameterAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Annotation CreateParameterAnnotation(Parameter parameter, [In] string name, Type type);

        [DllImport(CgLib, EntryPoint = "cgCreateParameterArray", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter CreateParameterArray(Context context, Type type, int length);

        [DllImport(CgLib, EntryPoint = "cgCreateParameterMultiDimArray", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter CreateParameterMultiDimArray(Context context, Type type, int dim,
            [In] int[] lengths);

        [DllImport(CgLib, EntryPoint = "cgCreatePass", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Pass CreatePass(Technique technique, [In] string name);

        [DllImport(CgLib, EntryPoint = "cgCreatePassAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Annotation CreatePassAnnotation(Pass pass, [In] string name, Type type);

        [DllImport(CgLib, EntryPoint = "cgCreateProgram", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        private static extern Program CreateProgram(Context context, Enum programType, [In] string program,
            Profile profile, [In] string entry, [In] string[] args);

        [DllImport(CgLib, EntryPoint = "cgCreateProgramAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Annotation CreateProgramAnnotation(Program program, [In] string name, Type type);

        [DllImport(CgLib, EntryPoint = "cgCreateProgramFromEffect", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr CreateProgramFromEffect(Effect effect, Profile profile, [In] string entry,
            [In] string[] args);

        [DllImport(CgLib, EntryPoint = "cgCreateProgramFromFile", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Program CreateProgramFromFile(Context context, Enum programType, [In] string programFile,
            Profile profile, [In] string entry, IntPtr args);

        [DllImport(CgLib, EntryPoint = "cgCreateSamplerState", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern State CreateSamplerState(Context context, [In] string name, Type type);

        [DllImport(CgLib, EntryPoint = "cgCreateSamplerStateAssignment", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern StateAssignment CreateSamplerStateAssignment(Parameter parameter, State state);

        [DllImport(CgLib, EntryPoint = "cgCreateState", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern State CreateState(Context context, [In] string name, Type type);

        [DllImport(CgLib, EntryPoint = "cgCreateStateAssignment", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern StateAssignment CreateStateAssignment(Pass pass, State state);

        [DllImport(CgLib, EntryPoint = "cgCreateStateAssignmentIndex", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern StateAssignment CreateStateAssignmentIndex(Pass pass, State state, int index);

        [DllImport(CgLib, EntryPoint = "cgCreateTechnique", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Technique CreateTechnique(Effect effect, [In] string name);

        [DllImport(CgLib, EntryPoint = "cgCreateTechniqueAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Annotation CreateTechniqueAnnotation(Technique technique, [In] string name, Type type);

        [DllImport(CgLib, EntryPoint = "cgDestroyBuffer", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void DestroyBuffer(Buffer buffer);

        [DllImport(CgLib, EntryPoint = "cgDestroyContext", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void DestroyContext(Context context);

        [DllImport(CgLib, EntryPoint = "cgDestroyEffect", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void DestroyEffect(Effect effect);

        [DllImport(CgLib, EntryPoint = "cgDestroyObj", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void DestroyObj(Obj obj);

        [DllImport(CgLib, EntryPoint = "cgDestroyParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void DestroyParameter(Parameter param);

        [DllImport(CgLib, EntryPoint = "cgDestroyProgram", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void DestroyProgram(Program program);

        [DllImport(CgLib, EntryPoint = "cgDisconnectParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void DisconnectParameter(Parameter param);

        [DllImport(CgLib, EntryPoint = "cgEvaluateProgram", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void EvaluateProgram(Program program, [In] [Out] float[] values, int ncomps, int nx, int ny,
            int nz);

        [DllImport(CgLib, EntryPoint = "cgGetAnnotationName", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetAnnotationName(Annotation annotation);

        [DllImport(CgLib, EntryPoint = "cgGetAnnotationType", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Type GetAnnotationType(Annotation annotation);

        [DllImport(CgLib, EntryPoint = "cgGetArrayDimension", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetArrayDimension(Parameter param);

        [DllImport(CgLib, EntryPoint = "cgGetArrayParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetArrayParameter(Parameter aparam, int index);

        [DllImport(CgLib, EntryPoint = "cgGetArraySize", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetArraySize(Parameter param, int dimension);

        [DllImport(CgLib, EntryPoint = "cgGetArrayTotalSize", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetArrayTotalSize(Parameter param);

        [DllImport(CgLib, EntryPoint = "cgGetArrayType", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Type GetArrayType(Parameter param);

        [DllImport(CgLib, EntryPoint = "cgGetAutoCompile", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Enum GetAutoCompile(Context context);

        [DllImport(CgLib, EntryPoint = "cgGetBehavior", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Behavior GetBehavior(string behaviorString);

        [DllImport(CgLib, EntryPoint = "cgGetBehaviorString", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetBehaviorString(Behavior behavior);

        [DllImport(CgLib, EntryPoint = "cgGetBoolAnnotationValues", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetBoolAnnotationValues(Annotation annotation, out int nvalues);

        [DllImport(CgLib, EntryPoint = "cgGetBoolStateAssignmentValues", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool[] GetBoolStateAssignmentValues(StateAssignment stateassignment, int[] nVals);

        [DllImport(CgLib, EntryPoint = "cgGetBooleanAnnotationValues", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int[] GetBooleanAnnotationValues(Annotation annotation, out int nvalues);

        [DllImport(CgLib, EntryPoint = "cgGetBufferSize", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetBufferSize(Buffer buffer);

        [DllImport(CgLib, EntryPoint = "cgGetCompilerIncludeCallback", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IncludeCallbackFuncDelegate GetCompilerIncludeCallback(Context context);

        [DllImport(CgLib, EntryPoint = "cgGetConnectedParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetConnectedParameter(Parameter param);

        [DllImport(CgLib, EntryPoint = "cgGetConnectedStateAssignmentParameter",
            CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetConnectedStateAssignmentParameter(StateAssignment stateassignment);

        [DllImport(CgLib, EntryPoint = "cgGetConnectedToParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetConnectedToParameter(Parameter param, int index);

        [DllImport(CgLib, EntryPoint = "cgGetContextBehavior", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Behavior GetContextBehavior(Context context);

        [DllImport(CgLib, EntryPoint = "cgGetDependentAnnotationParameter",
            CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetDependentAnnotationParameter(Annotation annotation, int index);

        [DllImport(CgLib, EntryPoint = "cgGetDependentProgramArrayStateAssignmentParameter",
            CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetDependentProgramArrayStateAssignmentParameter(StateAssignment sa, int index);

        [DllImport(CgLib, EntryPoint = "cgGetDependentStateAssignmentParameter",
            CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetDependentStateAssignmentParameter(StateAssignment stateassignment, int index);

        [DllImport(CgLib, EntryPoint = "cgGetDomain", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Domain GetDomain(string domainString);

        [DllImport(CgLib, EntryPoint = "cgGetDomainString", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetDomainString(Domain domain);

        [DllImport(CgLib, EntryPoint = "cgGetEffectContext", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Context GetEffectContext(Effect effect);

        [DllImport(CgLib, EntryPoint = "cgGetEffectName", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern string GetEffectName(Effect effect);

        [DllImport(CgLib, EntryPoint = "cgGetEffectParameterBuffer", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Buffer GetEffectParameterBuffer(Parameter param);

        [DllImport(CgLib, EntryPoint = "cgGetEffectParameterBySemantic", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetEffectParameterBySemantic(Effect effect, [In] string name);

        [DllImport(CgLib, EntryPoint = "cgGetEnum", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Enum GetEnum([In] string enumString);

        [DllImport(CgLib, EntryPoint = "cgGetEnumString", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetEnumString(Enum en);

        [DllImport(CgLib, EntryPoint = "cgGetError", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Error GetError();

        [DllImport(CgLib, EntryPoint = "cgGetErrorCallback", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern ErrorCallbackFuncDelegate GetErrorCallback();

        [DllImport(CgLib, EntryPoint = "cgGetErrorHandler", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern ErrorHandlerFuncDelegate GetErrorHandler(out IntPtr data);

        [DllImport(CgLib, EntryPoint = "cgGetErrorString", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetErrorString(Error error);

        [DllImport(CgLib, EntryPoint = "cgGetFirstDependentParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetFirstDependentParameter(Parameter param);

        [DllImport(CgLib, EntryPoint = "cgGetFirstEffect", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Effect GetFirstEffect(Context context);

        [DllImport(CgLib, EntryPoint = "cgGetFirstEffectAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Annotation GetFirstEffectAnnotation(Effect effect);

        [DllImport(CgLib, EntryPoint = "cgGetFirstEffectParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetFirstEffectParameter(Effect effect);

        [DllImport(CgLib, EntryPoint = "cgGetFirstError", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Error GetFirstError();

        [DllImport(CgLib, EntryPoint = "cgGetFirstLeafEffectParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetFirstLeafEffectParameter(Effect effect);

        [DllImport(CgLib, EntryPoint = "cgGetFirstLeafParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetFirstLeafParameter(Program program, Enum nameSpace);

        [DllImport(CgLib, EntryPoint = "cgGetFirstParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetFirstParameter(Program prog, Enum nameSpace);

        [DllImport(CgLib, EntryPoint = "cgGetFirstParameterAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Annotation GetFirstParameterAnnotation(Parameter param);

        [DllImport(CgLib, EntryPoint = "cgGetFirstPass", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Pass GetFirstPass(Technique technique);

        [DllImport(CgLib, EntryPoint = "cgGetFirstPassAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Annotation GetFirstPassAnnotation(Pass pass);

        [DllImport(CgLib, EntryPoint = "cgGetFirstProgram", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Program GetFirstProgram(Context context);

        [DllImport(CgLib, EntryPoint = "cgGetFirstProgramAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Annotation GetFirstProgramAnnotation(Program prog);

        [DllImport(CgLib, EntryPoint = "cgGetFirstSamplerState", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern State GetFirstSamplerState(Context context);

        [DllImport(CgLib, EntryPoint = "cgGetFirstSamplerStateAssignment",
            CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern StateAssignment GetFirstSamplerStateAssignment(Parameter param);

        [DllImport(CgLib, EntryPoint = "cgGetFirstState", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern State GetFirstState(Context context);

        [DllImport(CgLib, EntryPoint = "cgGetFirstStateAssignment", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern StateAssignment GetFirstStateAssignment(Pass pass);

        [DllImport(CgLib, EntryPoint = "cgGetFirstStructParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetFirstStructParameter(Parameter param);

        [DllImport(CgLib, EntryPoint = "cgGetFirstTechnique", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Technique GetFirstTechnique(Effect effect);

        [DllImport(CgLib, EntryPoint = "cgGetFirstTechniqueAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Annotation GetFirstTechniqueAnnotation(Technique technique);

        [DllImport(CgLib, EntryPoint = "cgGetFloatAnnotationValues", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern float[] GetFloatAnnotationValues(Annotation annotation, out int nvalues);

        [DllImport(CgLib, EntryPoint = "cgGetFloatStateAssignmentValues", CallingConvention = CallingConvention.Cdecl
            ), SuppressUnmanagedCodeSecurity]
        public static extern float[] GetFloatStateAssignmentValues(StateAssignment stateassignment, int[] nvalues);

        [DllImport(CgLib, EntryPoint = "cgGetIntAnnotationValues", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int[] GetIntAnnotationValues(Annotation annotation, out int nvalues);

        [DllImport(CgLib, EntryPoint = "cgGetIntStateAssignmentValues", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int[] GetIntStateAssignmentValues(StateAssignment stateassignment, int[] nvalues);

        [DllImport(CgLib, EntryPoint = "cgGetLastErrorString", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetLastErrorString(out Error error);

        [DllImport(CgLib, EntryPoint = "cgGetLastListing", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetLastListing(Context context);

        [DllImport(CgLib, EntryPoint = "cgGetLockingPolicy", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Enum GetLockingPolicy();

        [DllImport(CgLib, EntryPoint = "cgGetMatrixParameterOrder", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Enum GetMatrixParameterOrder(Parameter param);

        [DllImport(CgLib, EntryPoint = "cgGetMatrixParameterdc", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void GetMatrixParameterdc(Parameter param, [In] double[] matrix);

        [DllImport(CgLib, EntryPoint = "cgGetMatrixParameterdr", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void GetMatrixParameterdr(Parameter param, [In] double[] matrix);

        [DllImport(CgLib, EntryPoint = "cgGetMatrixParameterfc", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void GetMatrixParameterfc(Parameter param, [In] float[] matrix);

        [DllImport(CgLib, EntryPoint = "cgGetMatrixParameterfr", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void GetMatrixParameterfr(Parameter param, [In] float[] matrix);

        [DllImport(CgLib, EntryPoint = "cgGetMatrixParameteric", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void GetMatrixParameteric(Parameter param, [In] int[] matrix);

        [DllImport(CgLib, EntryPoint = "cgGetMatrixParameterir", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void GetMatrixParameterir(Parameter param, [In] int[] matrix);

        [DllImport(CgLib, EntryPoint = "cgGetMatrixSize", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void GetMatrixSize(Type type, out int nrows, out int ncols);

        [DllImport(CgLib, EntryPoint = "cgGetNamedEffect", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Effect GetNamedEffect(Context context, [In] string name);

        [DllImport(CgLib, EntryPoint = "cgGetNamedEffectAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Annotation GetNamedEffectAnnotation(Effect effect, [In] string name);

        [DllImport(CgLib, EntryPoint = "cgGetNamedEffectParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetNamedEffectParameter(Effect effect, [In] string name);

        [DllImport(CgLib, EntryPoint = "cgGetNamedParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetNamedParameter(Program program, [In] string parameter);

        [DllImport(CgLib, EntryPoint = "cgGetNamedParameterAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Annotation GetNamedParameterAnnotation(Parameter param, [In] string name);

        [DllImport(CgLib, EntryPoint = "cgGetNamedPass", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Pass GetNamedPass(Technique technique, [In] string name);

        [DllImport(CgLib, EntryPoint = "cgGetNamedPassAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Annotation GetNamedPassAnnotation(Pass pass, [In] string name);

        [DllImport(CgLib, EntryPoint = "cgGetNamedProgramAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Annotation GetNamedProgramAnnotation(Program prog, [In] string name);

        [DllImport(CgLib, EntryPoint = "cgGetNamedProgramParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetNamedProgramParameter(Program program, Enum nameSpace, [In] string name);

        [DllImport(CgLib, EntryPoint = "cgGetNamedSamplerState", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern State GetNamedSamplerState(Context context, [In] string name);

        [DllImport(CgLib, EntryPoint = "cgGetNamedSamplerStateAssignment",
            CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern StateAssignment GetNamedSamplerStateAssignment(Parameter param, [In] string name);

        [DllImport(CgLib, EntryPoint = "cgGetNamedState", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern State GetNamedState(Context context, [In] string name);

        [DllImport(CgLib, EntryPoint = "cgGetNamedStateAssignment", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern StateAssignment GetNamedStateAssignment(Pass pass, [In] string name);

        [DllImport(CgLib, EntryPoint = "cgGetNamedStructParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetNamedStructParameter(Parameter param, [In] string name);

        [DllImport(CgLib, EntryPoint = "cgGetNamedSubParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetNamedSubParameter(Parameter param, [In] string name);

        [DllImport(CgLib, EntryPoint = "cgGetNamedTechnique", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Technique GetNamedTechnique(Effect effect, [In] string name);

        [DllImport(CgLib, EntryPoint = "cgGetNamedTechniqueAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Annotation GetNamedTechniqueAnnotation(Technique technique, [In] string name);

        [DllImport(CgLib, EntryPoint = "cgGetNamedUserType", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Type GetNamedUserType(Handle handle, [In] string name);

        [DllImport(CgLib, EntryPoint = "cgGetNextAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Annotation GetNextAnnotation(Annotation annotation);

        [DllImport(CgLib, EntryPoint = "cgGetNextEffect", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Effect GetNextEffect(Effect effect);

        [DllImport(CgLib, EntryPoint = "cgGetNextLeafParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetNextLeafParameter(Parameter current);

        [DllImport(CgLib, EntryPoint = "cgGetNextParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetNextParameter(Parameter current);

        [DllImport(CgLib, EntryPoint = "cgGetNextPass", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Pass GetNextPass(Pass pass);

        [DllImport(CgLib, EntryPoint = "cgGetNextProgram", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Program GetNextProgram(Program program);

        [DllImport(CgLib, EntryPoint = "cgGetNextState", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern State GetNextState(State state);

        [DllImport(CgLib, EntryPoint = "cgGetNextStateAssignment", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern StateAssignment GetNextStateAssignment(StateAssignment stateassignment);

        [DllImport(CgLib, EntryPoint = "cgGetNextTechnique", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Technique GetNextTechnique(Technique technique);

        [DllImport(CgLib, EntryPoint = "cgGetNumConnectedToParameters", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetNumConnectedToParameters(Parameter param);

        [DllImport(CgLib, EntryPoint = "cgGetNumDependentAnnotationParameters",
            CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int GetNumDependentAnnotationParameters(Annotation annotation);

        [DllImport(CgLib, EntryPoint = "cgGetNumDependentProgramArrayStateAssignmentParameters",
            CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int GetNumDependentProgramArrayStateAssignmentParameters(StateAssignment sa);

        [DllImport(CgLib, EntryPoint = "cgGetNumDependentStateAssignmentParameters",
            CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int GetNumDependentStateAssignmentParameters(StateAssignment stateassignment);

        [DllImport(CgLib, EntryPoint = "cgGetNumParentTypes", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetNumParentTypes(Type type);

        [DllImport(CgLib, EntryPoint = "cgGetNumProgramDomains", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetNumProgramDomains(Program program);

        [DllImport(CgLib, EntryPoint = "cgGetNumStateEnumerants", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetNumStateEnumerants(State state);

        [DllImport(CgLib, EntryPoint = "cgGetNumSupportedProfiles", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetNumSupportedProfiles();

        [DllImport(CgLib, EntryPoint = "cgGetNumUserTypes", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetNumUserTypes(Handle handle);

        [DllImport(CgLib, EntryPoint = "cgGetParameterBaseResource", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Resource GetParameterBaseResource(Parameter param);

        [DllImport(CgLib, EntryPoint = "cgGetParameterBaseType", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Type GetParameterBaseType(Parameter param);

        [DllImport(CgLib, EntryPoint = "cgGetParameterBufferIndex", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterBufferIndex(Parameter parameter);

        [DllImport(CgLib, EntryPoint = "cgGetParameterBufferOffset", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterBufferOffset(Parameter parameter);

        [DllImport(CgLib, EntryPoint = "cgGetParameterClass", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern ParameterClass GetParameterClass(Parameter param);

        [DllImport(CgLib, EntryPoint = "cgGetParameterClassEnum", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern ParameterClass GetParameterClassEnum(string pString);

        [DllImport(CgLib, EntryPoint = "cgGetParameterClassString", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetParameterClassString(ParameterClass pc);

        [DllImport(CgLib, EntryPoint = "cgGetParameterColumns", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterColumns(Parameter param);

        [DllImport(CgLib, EntryPoint = "cgGetParameterContext", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Context GetParameterContext(Parameter param);

        [DllImport(CgLib, EntryPoint = "cgGetParameterDefaultValuedc", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterDefaultValuedc(Parameter param, int nelements, [Out] double[] vals);

        [DllImport(CgLib, EntryPoint = "cgGetParameterDefaultValuedr", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterDefaultValuedr(Parameter param, int nelements, [Out] double[] vals);

        [DllImport(CgLib, EntryPoint = "cgGetParameterDefaultValuefc", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterDefaultValuefc(Parameter param, int nelements, [Out] float[] vals);

        [DllImport(CgLib, EntryPoint = "cgGetParameterDefaultValuefr", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterDefaultValuefr(Parameter param, int nelements, [Out] float[] vals);

        [DllImport(CgLib, EntryPoint = "cgGetParameterDefaultValueic", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterDefaultValueic(Parameter param, int nelements, [Out] int[] vals);

        [DllImport(CgLib, EntryPoint = "cgGetParameterDefaultValueir", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterDefaultValueir(Parameter param, int nelements, [Out] int[] vals);

        [DllImport(CgLib, EntryPoint = "cgGetParameterDirection", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Enum GetParameterDirection(Parameter param);

        [DllImport(CgLib, EntryPoint = "cgGetParameterEffect", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Effect GetParameterEffect(Parameter param);

        [DllImport(CgLib, EntryPoint = "cgGetParameterIndex", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterIndex(Parameter param);

        [DllImport(CgLib, EntryPoint = "cgGetParameterName", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetParameterName(Parameter param);

        [DllImport(CgLib, EntryPoint = "cgGetParameterNamedType", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterNamedType(Parameter param);

        [DllImport(CgLib, EntryPoint = "cgGetParameterOrdinalNumber", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterOrdinalNumber(Parameter param);

        [DllImport(CgLib, EntryPoint = "cgGetParameterProgram", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Program GetParameterProgram(Parameter param);

        [DllImport(CgLib, EntryPoint = "cgGetParameterResource", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Resource GetParameterResource(Parameter param);

        [DllImport(CgLib, EntryPoint = "cgGetParameterResourceIndex", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterResourceIndex(Parameter param);

        [DllImport(CgLib, EntryPoint = "cgGetParameterResourceName", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern string GetParameterResourceName(Parameter param);

        [DllImport(CgLib, EntryPoint = "cgGetParameterResourceSize", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterResourceSize(Parameter parameter);

        [DllImport(CgLib, EntryPoint = "cgGetParameterResourceType", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Type GetParameterResourceType(Parameter parameter);

        [DllImport(CgLib, EntryPoint = "cgGetParameterRows", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterRows(Parameter param);

        [DllImport(CgLib, EntryPoint = "cgGetParameterSemantic", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetParameterSemantic(Parameter param);

        [DllImport(CgLib, EntryPoint = "cgGetParameterSettingMode", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Enum GetParameterSettingMode(Context context);

        [DllImport(CgLib, EntryPoint = "cgGetParameterType", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Type GetParameterType(Parameter param);

        [DllImport(CgLib, EntryPoint = "cgGetParameterValuedc", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterValuedc(Parameter param, int nelements, [Out] double[] vals);

        [DllImport(CgLib, EntryPoint = "cgGetParameterValuedr", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterValuedr(Parameter param, int nelements, [Out] double[] vals);

        [DllImport(CgLib, EntryPoint = "cgGetParameterValuefc", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterValuefc(Parameter param, int nelements, [Out] float[] vals);

        [DllImport(CgLib, EntryPoint = "cgGetParameterValuefr", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterValuefr(Parameter param, int nelements, [Out] float[] vals);

        [DllImport(CgLib, EntryPoint = "cgGetParameterValueic", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterValueic(Parameter param, int nelements, [Out] int[] vals);

        [DllImport(CgLib, EntryPoint = "cgGetParameterValueir", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterValueir(Parameter param, int nelements, [Out] int[] vals);

        [DllImport(CgLib, EntryPoint = "cgGetParameterValues", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern unsafe double* GetParameterValues(Parameter param, Enum value_type, int* nvalues);

        [DllImport(CgLib, EntryPoint = "cgGetParameterValues", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern double[] GetParameterValues(Parameter param, Enum value_type, [In] int[] nvalues);

        [DllImport(CgLib, EntryPoint = "cgGetParameterVariability", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Enum GetParameterVariability(Parameter param);

        [DllImport(CgLib, EntryPoint = "cgGetParentType", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Type GetParentType(Type type, int index);

        [DllImport(CgLib, EntryPoint = "cgGetPassName", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetPassName(Pass pass);

        [DllImport(CgLib, EntryPoint = "cgGetPassProgram", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Program GetPassProgram(Pass pass, Domain domain);

        [DllImport(CgLib, EntryPoint = "cgGetPassTechnique", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Technique GetPassTechnique(Pass pass);

        [DllImport(CgLib, EntryPoint = "cgGetProfile", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Profile GetProfile([In] string profile_string);

        [DllImport(CgLib, EntryPoint = "cgGetProfileDomain", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Domain GetProfileDomain(Profile profile);

        [DllImport(CgLib, EntryPoint = "cgGetProfileProperty", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool GetProfileProperty(Profile profile, Enum query);

        [DllImport(CgLib, EntryPoint = "cgGetProfileString", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern string GetProfileString(Profile profile);

        [DllImport(CgLib, EntryPoint = "cgGetProgramBuffer", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Buffer GetProgramBuffer(Program program, int bufferIndex);

        [DllImport(CgLib, EntryPoint = "cgGetProgramBufferMaxIndex", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetProgramBufferMaxIndex(Profile profile);

        [DllImport(CgLib, EntryPoint = "cgGetProgramBufferMaxSize", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetProgramBufferMaxSize(Profile profile);

        [DllImport(CgLib, EntryPoint = "cgGetProgramContext", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Context GetProgramContext(Program program);

        [DllImport(CgLib, EntryPoint = "cgGetProgramDomain", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Domain GetProgramDomain(Program program);

        [DllImport(CgLib, EntryPoint = "cgGetProgramDomainProfile", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Profile GetProgramDomainProfile(Program program, int index);

        [DllImport(CgLib, EntryPoint = "cgGetProgramDomainProgram", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Program GetProgramDomainProgram(Program program, int index);

        [DllImport(CgLib, EntryPoint = "cgGetProgramInput", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Enum GetProgramInput(Program program);

        [DllImport(CgLib, EntryPoint = "cgGetProgramOptions", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetProgramOptions(Program prog);

        [DllImport(CgLib, EntryPoint = "cgGetProgramOutput", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Enum GetProgramOutput(Program program);

        [DllImport(CgLib, EntryPoint = "cgGetProgramProfile", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Profile GetProgramProfile(Program program);

        [DllImport(CgLib, EntryPoint = "cgGetProgramStateAssignmentValue",
            CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern Program GetProgramStateAssignmentValue(StateAssignment stateassignment);

        [DllImport(CgLib, EntryPoint = "cgGetProgramString", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetProgramString(Program program, Enum sourceType);

        [DllImport(CgLib, EntryPoint = "cgGetResource", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Resource GetResource([In] string resource_string);

        [DllImport(CgLib, EntryPoint = "cgGetResourceString", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetResourceString(Resource resource);

        [DllImport(CgLib, EntryPoint = "cgGetSamplerStateAssignmentParameter",
            CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetSamplerStateAssignmentParameter(StateAssignment stateassignment);

        [DllImport(CgLib, EntryPoint = "cgGetSamplerStateAssignmentState",
            CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern State GetSamplerStateAssignmentState(StateAssignment stateassignment);

        [DllImport(CgLib, EntryPoint = "cgGetSamplerStateAssignmentValue",
            CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetSamplerStateAssignmentValue(StateAssignment stateassignment);

        [DllImport(CgLib, EntryPoint = "cgGetSemanticCasePolicy", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Enum GetSemanticCasePolicy();

        [DllImport(CgLib, EntryPoint = "cgGetStateAssignmentIndex", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetStateAssignmentIndex(StateAssignment stateassignment);

        [DllImport(CgLib, EntryPoint = "cgGetStateAssignmentPass", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Pass GetStateAssignmentPass(StateAssignment stateassignment);

        [DllImport(CgLib, EntryPoint = "cgGetStateAssignmentState", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern State GetStateAssignmentState(StateAssignment stateassignment);

        [DllImport(CgLib, EntryPoint = "cgGetStateContext", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Context GetStateContext(State state);

        [DllImport(CgLib, EntryPoint = "cgGetStateEnumerant", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern string GetStateEnumerant(State state, int index, out int value);

        [DllImport(CgLib, EntryPoint = "cgGetStateEnumerantName", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern string GetStateEnumerantName(State state, int value);

        [DllImport(CgLib, EntryPoint = "cgGetStateEnumerantValue", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetStateEnumerantValue(State state, [In] string name);

        [DllImport(CgLib, EntryPoint = "cgGetStateLatestProfile", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Profile GetStateLatestProfile(State state);

        [DllImport(CgLib, EntryPoint = "cgGetStateName", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern string GetStateName(State state);

        [DllImport(CgLib, EntryPoint = "cgGetStateResetCallback", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern StateCallbackDelegate GetStateResetCallback(State state);

        [DllImport(CgLib, EntryPoint = "cgGetStateSetCallback", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern StateCallbackDelegate GetStateSetCallback(State state);

        [DllImport(CgLib, EntryPoint = "cgGetStateType", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Type GetStateType(State state);

        [DllImport(CgLib, EntryPoint = "cgGetStateValidateCallback", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern StateCallbackDelegate GetStateValidateCallback(State state);

        [DllImport(CgLib, EntryPoint = "cgGetString", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetString(Enum sname);

        [DllImport(CgLib, EntryPoint = "cgGetStringAnnotationValue", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern string GetStringAnnotationValue(Annotation annotation);

        [DllImport(CgLib, EntryPoint = "cgGetStringAnnotationValues", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetStringAnnotationValues(Annotation ann, out int nvalues);

        [DllImport(CgLib, EntryPoint = "cgGetStringParameterValue", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern string GetStringParameterValue(Parameter param);

        [DllImport(CgLib, EntryPoint = "cgGetStringStateAssignmentValue", CallingConvention = CallingConvention.Cdecl
            ), SuppressUnmanagedCodeSecurity]
        public static extern string GetStringStateAssignmentValue(StateAssignment stateassignment);

        [DllImport(CgLib, EntryPoint = "cgGetSupportedProfile", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Profile GetSupportedProfile(int index);

        [DllImport(CgLib, EntryPoint = "cgGetTechniqueEffect", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Effect GetTechniqueEffect(Technique technique);

        [DllImport(CgLib, EntryPoint = "cgGetTechniqueName", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetTechniqueName(Technique technique);

        [DllImport(CgLib, EntryPoint = "cgGetTextureStateAssignmentValue",
            CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetTextureStateAssignmentValue(StateAssignment stateassignment);

        [DllImport(CgLib, EntryPoint = "cgGetType", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Type GetType([In] string type_string);

        [DllImport(CgLib, EntryPoint = "cgGetTypeBase", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Type GetTypeBase(Type type);

        [DllImport(CgLib, EntryPoint = "cgGetTypeClass", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern ParameterClass GetTypeClass(Type type);

        [DllImport(CgLib, EntryPoint = "cgGetTypeSizes", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool GetTypeSizes(Type type, out int nrows, out int ncols);

        [DllImport(CgLib, EntryPoint = "cgGetTypeString", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetTypeString(Type type);

        [DllImport(CgLib, EntryPoint = "cgGetUserType", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Type GetUserType(Handle handle, int index);

        [DllImport(CgLib, EntryPoint = "cgIsAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool IsAnnotation(Annotation annotation);

        [DllImport(CgLib, EntryPoint = "cgIsContext", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool IsContext(Context context);

        [DllImport(CgLib, EntryPoint = "cgIsEffect", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool IsEffect(Effect effect);

        [DllImport(CgLib, EntryPoint = "cgIsInterfaceType", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool IsInterfaceType(Type type);

        [DllImport(CgLib, EntryPoint = "cgIsParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool IsParameter(Parameter param);

        [DllImport(CgLib, EntryPoint = "cgIsParameterGlobal", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool IsParameterGlobal(Parameter param);

        [DllImport(CgLib, EntryPoint = "cgIsParameterReferenced", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool IsParameterReferenced(Parameter param);

        [DllImport(CgLib, EntryPoint = "cgIsParameterUsed", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool IsParameterUsed(Parameter param, Handle handle);

        [DllImport(CgLib, EntryPoint = "cgIsParentType", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool IsParentType(Type parent, Type child);

        [DllImport(CgLib, EntryPoint = "cgIsPass", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool IsPass(Pass pass);

        [DllImport(CgLib, EntryPoint = "cgIsProfileSupported", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool IsProfileSupported(Profile profile);

        [DllImport(CgLib, EntryPoint = "cgIsProgram", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool IsProgram(Program program);

        [DllImport(CgLib, EntryPoint = "cgIsProgramCompiled", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool IsProgramCompiled(Program program);

        [DllImport(CgLib, EntryPoint = "cgIsState", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool IsState(State state);

        [DllImport(CgLib, EntryPoint = "cgIsStateAssignment", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool IsStateAssignment(StateAssignment stateassignment);

        [DllImport(CgLib, EntryPoint = "cgIsTechnique", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool IsTechnique(Technique technique);

        [DllImport(CgLib, EntryPoint = "cgIsTechniqueValidated", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool IsTechniqueValidated(Technique technique);

        [DllImport(CgLib, EntryPoint = "cgMapBuffer", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr MapBuffer(Buffer buffer, BufferAccess access);

        [DllImport(CgLib, EntryPoint = "cgResetPassState", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void ResetPassState(Pass pass);

        [DllImport(CgLib, EntryPoint = "cgSetArraySize", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetArraySize(Parameter param, int size);

        [DllImport(CgLib, EntryPoint = "cgSetAutoCompile", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetAutoCompile(Context context, Enum autoCompileMode);

        [DllImport(CgLib, EntryPoint = "cgSetBoolAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool SetBoolAnnotation(Annotation annotation, Bool value);

        [DllImport(CgLib, EntryPoint = "cgSetBoolArrayStateAssignment", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool SetBoolArrayStateAssignment(StateAssignment stateassignment, [In] Bool[] vals);

        [DllImport(CgLib, EntryPoint = "cgSetBoolStateAssignment", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool SetBoolStateAssignment(StateAssignment stateassignment, Bool value);

        [DllImport(CgLib, EntryPoint = "cgSetBufferData", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetBufferData(Buffer buffer, int size, [In] IntPtr data);

        [DllImport(CgLib, EntryPoint = "cgSetBufferSubData", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetBufferSubData(Buffer buffer, int offset, int size, [In] IntPtr data);

        [DllImport(CgLib, EntryPoint = "cgSetCompilerIncludeCallback", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetCompilerIncludeCallback(Context context, IncludeCallbackFuncDelegate func);

        [DllImport(CgLib, EntryPoint = "cgSetCompilerIncludeFile", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetCompilerIncludeFile(Context context, string name, string filename);

        [DllImport(CgLib, EntryPoint = "cgSetCompilerIncludeString", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetCompilerIncludeString(Context context, string name, string source);

        [DllImport(CgLib, EntryPoint = "cgSetContextBehavior", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetContextBehavior(Context context, Behavior behavior);

        [DllImport(CgLib, EntryPoint = "cgSetEffectName", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool SetEffectName(Effect effect, [In] string name);

        [DllImport(CgLib, EntryPoint = "cgSetEffectParameterBuffer", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetEffectParameterBuffer(Parameter param, Buffer buffer);

        [DllImport(CgLib, EntryPoint = "cgSetErrorCallback", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetErrorCallback(ErrorCallbackFuncDelegate func);

        [DllImport(CgLib, EntryPoint = "cgSetErrorHandler", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetErrorHandler(ErrorHandlerFuncDelegate func, IntPtr data);

        [DllImport(CgLib, EntryPoint = "cgSetFloatAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool SetFloatAnnotation(Annotation annotation, float value);

        [DllImport(CgLib, EntryPoint = "cgSetFloatArrayStateAssignment", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool SetFloatArrayStateAssignment(StateAssignment stateassignment, [In] float[] vals);

        [DllImport(CgLib, EntryPoint = "cgSetFloatStateAssignment", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool SetFloatStateAssignment(StateAssignment stateassignment, float value);

        [DllImport(CgLib, EntryPoint = "cgSetIntAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool SetIntAnnotation(Annotation annotation, int value);

        [DllImport(CgLib, EntryPoint = "cgSetIntArrayStateAssignment", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool SetIntArrayStateAssignment(StateAssignment stateassignment, [In] int[] vals);

        [DllImport(CgLib, EntryPoint = "cgSetIntStateAssignment", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool SetIntStateAssignment(StateAssignment stateassignment, int value);

        [DllImport(CgLib, EntryPoint = "cgSetLastListing", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetLastListing(Handle handle, string listing);

        [DllImport(CgLib, EntryPoint = "cgSetLockingPolicy", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Enum SetLockingPolicy(Enum lockingPolicy);

        [DllImport(CgLib, EntryPoint = "cgSetMatrixParameterdc", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetMatrixParameterdc(Parameter param, [In] double[] matrix);

        [DllImport(CgLib, EntryPoint = "cgSetMatrixParameterdr", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetMatrixParameterdr(Parameter param, [In] double[] matrix);

        [DllImport(CgLib, EntryPoint = "cgSetMatrixParameterfc", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetMatrixParameterfc(Parameter param, [In] float[] matrix);

        [DllImport(CgLib, EntryPoint = "cgSetMatrixParameterfr", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetMatrixParameterfr(Parameter param, [In] Matrix matrix);

        [DllImport(CgLib, EntryPoint = "cgSetMatrixParameteric", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetMatrixParameteric(Parameter param, [In] int[] matrix);

        [DllImport(CgLib, EntryPoint = "cgSetMatrixParameterir", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetMatrixParameterir(Parameter param, [In] int[] matrix);

        [DllImport(CgLib, EntryPoint = "cgSetMultiDimArraySize", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetMultiDimArraySize(Parameter param, int[] sizes);

        [DllImport(CgLib, EntryPoint = "cgSetParameter1d", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter1d(Parameter param, double x);

        [DllImport(CgLib, EntryPoint = "cgSetParameter1dv", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter1dv(Parameter param, [In] double[] v);

        [DllImport(CgLib, EntryPoint = "cgSetParameter1f", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter1f(Parameter param, float x);

        [DllImport(CgLib, EntryPoint = "cgSetParameter1fv", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter1fv(Parameter param, [In] float[] v);

        [DllImport(CgLib, EntryPoint = "cgSetParameter1i", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter1i(Parameter param, int x);

        [DllImport(CgLib, EntryPoint = "cgSetParameter1iv", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter1iv(Parameter param, [In] int[] v);

        [DllImport(CgLib, EntryPoint = "cgSetParameter2d", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter2d(Parameter param, double x, double y);

        [DllImport(CgLib, EntryPoint = "cgSetParameter2dv", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter2dv(Parameter param, [In] double[] v);

        [DllImport(CgLib, EntryPoint = "cgSetParameter2f", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter2f(Parameter param, float x, float y);

        [DllImport(CgLib, EntryPoint = "cgSetParameter2fv", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter2fv(Parameter param, [In] float[] v);

        [DllImport(CgLib, EntryPoint = "cgSetParameter2i", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter2i(Parameter param, int x, int y);

        [DllImport(CgLib, EntryPoint = "cgSetParameter2iv", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter2iv(Parameter param, [In] int[] v);

        [DllImport(CgLib, EntryPoint = "cgSetParameter3d", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter3d(Parameter param, double x, double y, double z);

        [DllImport(CgLib, EntryPoint = "cgSetParameter3dv", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter3dv(Parameter param, [In] double[] v);

        [DllImport(CgLib, EntryPoint = "cgSetParameter3f", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter3f(Parameter param, float x, float y, float z);

        [DllImport(CgLib, EntryPoint = "cgSetParameter3fv", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter3fv(Parameter param, [In] float[] v);

        [DllImport(CgLib, EntryPoint = "cgSetParameter3i", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter3i(Parameter param, int x, int y, int z);

        [DllImport(CgLib, EntryPoint = "cgSetParameter3iv", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter3iv(Parameter param, [In] int[] v);

        [DllImport(CgLib, EntryPoint = "cgSetParameter4d", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter4d(Parameter param, double x, double y, double z, double w);

        [DllImport(CgLib, EntryPoint = "cgSetParameter4dv", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter4dv(Parameter param, [In] double[] v);

        [DllImport(CgLib, EntryPoint = "cgSetParameter4f", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter4f(Parameter param, float x, float y, float z, float w);

        [DllImport(CgLib, EntryPoint = "cgSetParameter4fv", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter4fv(Parameter param, [In] float[] v);

        [DllImport(CgLib, EntryPoint = "cgSetParameter4i", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter4i(Parameter param, int x, int y, int z, int w);

        [DllImport(CgLib, EntryPoint = "cgSetParameter4iv", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter4iv(Parameter param, [In] int[] v);

        [DllImport(CgLib, EntryPoint = "cgSetParameterSemantic", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameterSemantic(Parameter param, [In] string semantic);

        [DllImport(CgLib, EntryPoint = "cgSetParameterSettingMode", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameterSettingMode(Context context, Enum parameterSettingMode);

        [DllImport(CgLib, EntryPoint = "cgSetParameterValuedc", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameterValuedc(Parameter param, int nelements, [In] double[] vals);

        [DllImport(CgLib, EntryPoint = "cgSetParameterValuedr", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameterValuedr(Parameter param, int nelements, [In] double[] vals);

        [DllImport(CgLib, EntryPoint = "cgSetParameterValuefc", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameterValuefc(Parameter param, int nelements, [In] float[] vals);

        [DllImport(CgLib, EntryPoint = "cgSetParameterValuefr", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameterValuefr(Parameter param, int nelements, [In] float[] vals);

        [DllImport(CgLib, EntryPoint = "cgSetParameterValueic", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameterValueic(Parameter param, int nelements, [In] int[] vals);

        [DllImport(CgLib, EntryPoint = "cgSetParameterValueir", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameterValueir(Parameter param, int nelements, [In] int[] vals);

        [DllImport(CgLib, EntryPoint = "cgSetParameterVariability", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameterVariability(Parameter param, Enum vary);

        [DllImport(CgLib, EntryPoint = "cgSetPassProgramParameters", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetPassProgramParameters(Program prog);

        [DllImport(CgLib, EntryPoint = "cgSetProgramBuffer", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetProgramBuffer(Program program, int bufferIndex, Buffer buffer);

        [DllImport(CgLib, EntryPoint = "cgSetProgramProfile", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetProgramProfile(Program prog, Profile profile);

        [DllImport(CgLib, EntryPoint = "cgSetProgramStateAssignment", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool SetProgramStateAssignment(StateAssignment stateassignment, Program program);

        [DllImport(CgLib, EntryPoint = "cgSetSamplerState", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetSamplerState(Parameter param);

        [DllImport(CgLib, EntryPoint = "cgSetSamplerStateAssignment", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool SetSamplerStateAssignment(StateAssignment stateassignment, Parameter parameter);

        [DllImport(CgLib, EntryPoint = "cgSetSemanticCasePolicy", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Enum SetSemanticCasePolicy(Enum casePolicy);

        [DllImport(CgLib, EntryPoint = "cgSetStateCallbacks", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetStateCallbacks(State state, StateCallbackDelegate set, StateCallbackDelegate reset,
            StateCallbackDelegate validate);

        [DllImport(CgLib, EntryPoint = "cgSetStateLatestProfile", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetStateLatestProfile(State state, Profile profile);

        [DllImport(CgLib, EntryPoint = "cgSetStringAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool SetStringAnnotation(Annotation annotation, [In] string value);

        [DllImport(CgLib, EntryPoint = "cgSetStringParameterValue", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetStringParameterValue(Parameter param, [In] string str);

        [DllImport(CgLib, EntryPoint = "cgSetStringStateAssignment", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool SetStringStateAssignment(StateAssignment stateassignment, [In] string name);

        [DllImport(CgLib, EntryPoint = "cgSetTextureStateAssignment", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool SetTextureStateAssignment(StateAssignment stateassignment, Parameter parameter);

        [DllImport(CgLib, EntryPoint = "cgUnmapBuffer", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void UnmapBuffer(Buffer buffer);

        [DllImport(CgLib, EntryPoint = "cgUpdatePassParameters", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void UpdatePassParameters(Pass pass);

        [DllImport(CgLib, EntryPoint = "cgUpdateProgramParameters", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void UpdateProgramParameters(Program program);

        [DllImport(CgLib, EntryPoint = "cgValidateTechnique", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool ValidateTechnique(Technique technique);

        #endregion Cg

        #region Dx
        public class DX11
        {
            [DllImport(CgDxLib, EntryPoint = "cgD3D11GetDevice", CallingConvention = CallingConvention.ThisCall)]
            public static extern IntPtr GetDevice(Context context);

            [DllImport(CgDxLib, EntryPoint = "cgD3D11SetDevice", CallingConvention = CallingConvention.ThisCall)]
            public static extern int SetDevice(Context context, IntPtr pDevice);

            [DllImport(CgDxLib, EntryPoint = "cgD3D11SetTextureParameter", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetTextureParameter(Parameter parameter, IntPtr pTexture);


            [DllImport(CgDxLib, EntryPoint = "cgD3D11SetSamplerStateParameter",
                CallingConvention = CallingConvention.ThisCall)]
            public static extern void SetSamplerStateParameter(Parameter parameter, IntPtr pSamplerState);

            [DllImport(CgDxLib, EntryPoint = "cgD3D11SetTextureSamplerStateParameter",
                CallingConvention = CallingConvention.ThisCall)]
            public static extern void SetTextureSamplerStateParameter(Parameter parameter, IntPtr pTexture,
                IntPtr pSamplerState);

            [DllImport(CgDxLib, EntryPoint = "cgD3D11LoadProgram", CallingConvention = CallingConvention.ThisCall)
            ]
            public static extern int LoadProgram(Program Program, uint Flags);

            [DllImport(CgDxLib, EntryPoint = "cgD3D11GetCompiledProgram",
                CallingConvention = CallingConvention.ThisCall)]
            public static extern IntPtr GetCompiledProgram(Program Program);

            [DllImport(CgDxLib, EntryPoint = "cgD3D11GetProgramErrors",
                CallingConvention = CallingConvention.ThisCall
                )]
            public static extern IntPtr GetProgramErrors(Program Program);

            [DllImport(CgDxLib, EntryPoint = "cgD3D11IsProgramLoaded",
                CallingConvention = CallingConvention.ThisCall)
            ]
            public static extern Bool IsProgramLoaded(Program Program);

            [DllImport(CgDxLib, EntryPoint = "cgD3D11BindProgram", CallingConvention = CallingConvention.ThisCall)
            ]
            public static extern int BindProgram(Program Program);

            [DllImport(CgDxLib, EntryPoint = "cgD3D11UnloadProgram",
                CallingConvention = CallingConvention.ThisCall)]
            public static extern void UnloadProgram(Program Program);

            [DllImport(CgDxLib, EntryPoint = "cgD3D11GetBufferByIndex",
                CallingConvention = CallingConvention.ThisCall
                )]
            public static extern IntPtr GetBufferByIndex(Program Program, uint Index);

            [DllImport(CgDxLib, EntryPoint = "cgD3D11RegisterStates",
                CallingConvention = CallingConvention.ThisCall)]
            public static extern void RegisterStates(Context Context);

            [DllImport(CgDxLib, EntryPoint = "cgD3D11SetManageTextureParameters",
                CallingConvention = CallingConvention.ThisCall)]
            public static extern void SetManageTextureParameters(Context Context, Bool Flag);

            [DllImport(CgDxLib, EntryPoint = "cgD3D11GetManageTextureParameters",
                CallingConvention = CallingConvention.ThisCall)]
            public static extern Bool GetManageTextureParameters(Context Context);

            [DllImport(CgDxLib, EntryPoint = "cgD3D11GetIASignatureByPass",
                CallingConvention = CallingConvention.ThisCall)]
            public static extern IntPtr GetIASignatureByPass(Pass Pass);

            [DllImport(CgDxLib, EntryPoint = "cgD3D11GetLatestVertexProfile")]
            public static extern Profile GetLatestVertexProfile();

            [DllImport(CgDxLib, EntryPoint = "cgD3D11GetLatestGeometryProfile")]
            public static extern Profile GetLatestGeometryProfile();

            [DllImport(CgDxLib, EntryPoint = "cgD3D11GetLatestPixelProfile")]
            public static extern Profile GetLatestPixelProfile();

            [DllImport(CgDxLib, EntryPoint = "cgD3D11GetLatestHullProfile")]
            public static extern Profile GetLatestHullProfile();

            [DllImport(CgDxLib, EntryPoint = "cgD3D11GetLatestDomainProfile")]
            public static extern Profile GetLatestDomainProfile();

            [DllImport(CgDxLib, EntryPoint = "cgD3D11IsProfileSupported",
                CallingConvention = CallingConvention.ThisCall)]
            public static extern Bool IsProfileSupported(Profile profile);

            [DllImport(CgDxLib, EntryPoint = "cgD3D11TypeToSize", CallingConvention = CallingConvention.ThisCall)]
            public static extern int TypeToSize(Type type);

            [DllImport(CgDxLib, EntryPoint = "cgD3D11GetLastError")]
            public static extern int GetLastError();

            [DllImport(CgDxLib, EntryPoint = "cgD3D11GetOptimalOptions",
                CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
            public static extern IntPtr GetOptimalOptions(Profile profile);

            [DllImport(CgDxLib, EntryPoint = "cgD3D11TranslateCGerror",
                CallingConvention = CallingConvention.ThisCall,
                CharSet = CharSet.Ansi)]
            public static extern string TranslateCGerror(Error error);

            [DllImport(CgDxLib, EntryPoint = "cgD3D11TranslateHRESULT",
                CallingConvention = CallingConvention.ThisCall,
                CharSet = CharSet.Ansi)]
            public static extern IntPtr TranslateHRESULT(int hr);

            [DllImport(CgDxLib, EntryPoint = "cgD3D11UnbindProgram",
                CallingConvention = CallingConvention.ThisCall)]
            public static extern void UnbindProgram(Program program);

            [DllImport(CgDxLib, EntryPoint = "cgD3D11CreateBuffer", CallingConvention = CallingConvention.ThisCall
                )]
            public static extern Buffer CreateBuffer(Context context, int size, IntPtr data, ResourceUsage bufferUsage);

            [DllImport(CgDxLib, EntryPoint = "cgD3D11CreateBufferFromObject",
                CallingConvention = CallingConvention.ThisCall)]
            public static extern Buffer CreateBufferFromObject(Context context, IntPtr obj, Bool manageObject);

            [DllImport(CgDxLib, EntryPoint = "cgD3D11GetBufferObject",
                CallingConvention = CallingConvention.ThisCall)
            ]
            public static extern IntPtr GetBufferObject(Buffer buffer);
        }

        #endregion Dx

        #region GL

        public class GL
        {
            [DllImport(CgGlLib, EntryPoint = "cgGLBindProgram", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void BindProgram(Program program);

            [DllImport(CgGlLib, EntryPoint = "cgGLCreateBuffer", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern Buffer CreateBuffer(Context context, int size, IntPtr data, int bufferUsage);

            [DllImport(CgGlLib, EntryPoint = "cgGLDisableClientState", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void DisableClientState(Parameter param);

            [DllImport(CgGlLib, EntryPoint = "cgGLDisableProfile", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void DisableProfile(Profile profile);

            [DllImport(CgGlLib, EntryPoint = "cgGLDisableProgramProfiles",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void DisableProgramProfiles(Program program);

            [DllImport(CgGlLib, EntryPoint = "cgGLDisableTextureParameter",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void DisableTextureParameter(Parameter param);

            [DllImport(CgGlLib, EntryPoint = "cgGLEnableClientState", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void EnableClientState(Parameter param);

            [DllImport(CgGlLib, EntryPoint = "cgGLEnableProfile", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void EnableProfile(Profile profile);

            [DllImport(CgGlLib, EntryPoint = "cgGLEnableProgramProfiles", CallingConvention = CallingConvention.Cdecl
                ), SuppressUnmanagedCodeSecurity]
            public static extern void EnableProgramProfiles(Program program);

            [DllImport(CgGlLib, EntryPoint = "cgGLEnableTextureParameter",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void EnableTextureParameter(Parameter param);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetBufferObject", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern int GetBufferObject(Buffer buffer);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetLatestProfile", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern Profile GetLatestProfile(GlEnum profileType);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetManageTextureParameters",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern int GetManageTextureParameters(Context context);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetMatrixParameterArraydc",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetMatrixParameterArraydc(Parameter param, long offset, long nelements,
                [Out] double* v);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetMatrixParameterArraydc",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void GetMatrixParameterArraydc(Parameter param, long offset, long nelements,
                [Out] double[] v);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetMatrixParameterArraydc",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void GetMatrixParameterArraydc(Parameter param, long offset, long nelements,
                [Out] IntPtr v);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetMatrixParameterArraydr",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetMatrixParameterArraydr(Parameter param, long offset, long nelements,
                [Out] double* v);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetMatrixParameterArraydr",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void GetMatrixParameterArraydr(Parameter param, long offset, long nelements,
                [Out] double[] v);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetMatrixParameterArraydr",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void GetMatrixParameterArraydr(Parameter param, long offset, long nelements,
                [Out] IntPtr v);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetMatrixParameterArrayfc",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetMatrixParameterArrayfc(Parameter param, long offset, long nelements,
                [Out] float* v);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetMatrixParameterArrayfc",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void GetMatrixParameterArrayfc(Parameter param, long offset, long nelements,
                [Out] float[] v);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetMatrixParameterArrayfc",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void GetMatrixParameterArrayfc(Parameter param, long offset, long nelements,
                [Out] IntPtr v);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetMatrixParameterArrayfr",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetMatrixParameterArrayfr(Parameter param, long offset, long nelements,
                [Out] float* v);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetMatrixParameterArrayfr",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void GetMatrixParameterArrayfr(Parameter param, long offset, long nelements,
                [Out] float[] v);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetMatrixParameterArrayfr",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void GetMatrixParameterArrayfr(Parameter param, long offset, long nelements,
                [Out] IntPtr v);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetMatrixParameterdc", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetMatrixParameterdc(Parameter param, [In] double* matrix);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetMatrixParameterdc", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetMatrixParameterdc(Parameter param, [In] double[] matrix);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetMatrixParameterdc", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetMatrixParameterdc(Parameter param, [In] IntPtr matrix);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetMatrixParameterdr", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetMatrixParameterdr(Parameter param, [In] double* matrix);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetMatrixParameterdr", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetMatrixParameterdr(Parameter param, [In] double[] matrix);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetMatrixParameterdr", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetMatrixParameterdr(Parameter param, [In] IntPtr matrix);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetMatrixParameterfc", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetMatrixParameterfc(Parameter param, [In] float* matrix);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetMatrixParameterfc", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetMatrixParameterfc(Parameter param, [In] float[] matrix);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetMatrixParameterfc", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetMatrixParameterfc(Parameter param, [In] IntPtr matrix);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetMatrixParameterfr", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetMatrixParameterfr(Parameter param, [In] float* matrix);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetMatrixParameterfr", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetMatrixParameterfr(Parameter param, [In] float[] matrix);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetMatrixParameterfr", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetMatrixParameterfr(Parameter param, [In] IntPtr matrix);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetOptimalOptions", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern IntPtr GetOptimalOptions(Profile profile);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameter1d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetParameter1d(Parameter param, [Out] double* values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameter1d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameter1d(Parameter param, [Out] double[] values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameter1d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameter1d(Parameter param, [Out] IntPtr values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameter1f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetParameter1f(Parameter param, [Out] float* values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameter1f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameter1f(Parameter param, [Out] float[] values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameter1f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameter1f(Parameter param, [Out] IntPtr values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameter2d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetParameter2d(Parameter param, [Out] double* values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameter2d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameter2d(Parameter param, [Out] double[] values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameter2d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameter2d(Parameter param, [Out] IntPtr values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameter2f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetParameter2f(Parameter param, [Out] float* values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameter2f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameter2f(Parameter param, [Out] float[] values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameter2f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameter2f(Parameter param, [Out] IntPtr values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameter3d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetParameter3d(Parameter param, [Out] double* values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameter3d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameter3d(Parameter param, [Out] double[] values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameter3d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameter3d(Parameter param, [Out] IntPtr values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameter3f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetParameter3f(Parameter param, [Out] float* values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameter3f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameter3f(Parameter param, [Out] float[] values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameter3f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameter3f(Parameter param, [Out] IntPtr values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameter4d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetParameter4d(Parameter param, [Out] double* values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameter4d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameter4d(Parameter param, [Out] double[] values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameter4d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameter4d(Parameter param, [Out] IntPtr values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameter4f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetParameter4f(Parameter param, [Out] float* values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameter4f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameter4f(Parameter param, [Out] float[] values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameter4f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameter4f(Parameter param, [Out] IntPtr values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameterArray1d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetParameterArray1d(Parameter param, long offset, long nelements,
                [Out] double* values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameterArray1d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameterArray1d(Parameter param, long offset, long nelements,
                [Out] double[] values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameterArray1d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameterArray1d(Parameter param, long offset, long nelements,
                [Out] IntPtr values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameterArray1f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetParameterArray1f(Parameter param, long offset, long nelements,
                [Out] float* values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameterArray1f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameterArray1f(Parameter param, long offset, long nelements,
                [Out] float[] values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameterArray1f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameterArray1f(Parameter param, long offset, long nelements,
                [Out] IntPtr values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameterArray2d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetParameterArray2d(Parameter param, long offset, long nelements,
                [Out] double* values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameterArray2d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameterArray2d(Parameter param, long offset, long nelements,
                [Out] double[] values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameterArray2d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameterArray2d(Parameter param, long offset, long nelements,
                [Out] IntPtr values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameterArray2f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetParameterArray2f(Parameter param, long offset, long nelements,
                [Out] float* values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameterArray2f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameterArray2f(Parameter param, long offset, long nelements,
                [Out] float[] values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameterArray2f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameterArray2f(Parameter param, long offset, long nelements,
                [Out] IntPtr values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameterArray3d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetParameterArray3d(Parameter param, long offset, long nelements,
                [Out] double* values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameterArray3d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameterArray3d(Parameter param, long offset, long nelements,
                [Out] double[] values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameterArray3d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameterArray3d(Parameter param, long offset, long nelements,
                [Out] IntPtr values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameterArray3f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetParameterArray3f(Parameter param, long offset, long nelements,
                [Out] float* values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameterArray3f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameterArray3f(Parameter param, long offset, long nelements,
                [Out] float[] values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameterArray3f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameterArray3f(Parameter param, long offset, long nelements,
                [Out] IntPtr values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameterArray4d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetParameterArray4d(Parameter param, long offset, long nelements,
                [Out] double* values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameterArray4d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameterArray4d(Parameter param, long offset, long nelements,
                [Out] double[] values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameterArray4d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameterArray4d(Parameter param, long offset, long nelements,
                [Out] IntPtr values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameterArray4f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetParameterArray4f(Parameter param, long offset, long nelements,
                [Out] float* values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameterArray4f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameterArray4f(Parameter param, long offset, long nelements,
                [Out] float[] values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetParameterArray4f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameterArray4f(Parameter param, long offset, long nelements,
                [Out] IntPtr values);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetProgramID", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern int GetProgramID(Program program);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetDebugMode", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetDebugMode(Bool debug);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetManageTextureParameters",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void SetManageTextureParameters(Context context, bool flag);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetTextureEnum", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern int GetTextureEnum(Parameter param);

            [DllImport(CgGlLib, EntryPoint = "cgGLGetTextureParameter", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern int GetTextureParameter(Parameter param);

            [DllImport(CgGlLib, EntryPoint = "cgGLIsProfileSupported", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern Bool IsProfileSupported(Profile profile);

            [DllImport(CgGlLib, EntryPoint = "cgGLIsProgramLoaded", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern Bool IsProgramLoaded(Program program);

            [DllImport(CgGlLib, EntryPoint = "cgGLLoadProgram", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void LoadProgram(Program program);

            [DllImport(CgGlLib, EntryPoint = "cgGLRegisterStates", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void RegisterStates(Context context);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetMatrixParameterArraydc",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetMatrixParameterArraydc(Parameter param, long offset, long nelements,
                [In] double* v);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetMatrixParameterArraydc",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void SetMatrixParameterArraydc(Parameter param, long offset, long nelements,
                [In] double[] v);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetMatrixParameterArraydc",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void SetMatrixParameterArraydc(Parameter param, long offset, long nelements,
                [In] IntPtr v);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetMatrixParameterArraydr",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetMatrixParameterArraydr(Parameter param, long offset, long nelements,
                [In] double* v);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetMatrixParameterArraydr",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void SetMatrixParameterArraydr(Parameter param, long offset, long nelements,
                [In] double[] v);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetMatrixParameterArraydr",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void SetMatrixParameterArraydr(Parameter param, long offset, long nelements,
                [In] IntPtr v);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetMatrixParameterArrayfc",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetMatrixParameterArrayfc(Parameter param, long offset, long nelements,
                [In] float* v);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetMatrixParameterArrayfc",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void SetMatrixParameterArrayfc(Parameter param, long offset, long nelements,
                [In] float[] v);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetMatrixParameterArrayfc",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void SetMatrixParameterArrayfc(Parameter param, long offset, long nelements,
                [In] IntPtr v);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetMatrixParameterArrayfr",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetMatrixParameterArrayfr(Parameter param, long offset, long nelements,
                [In] float* v);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetMatrixParameterArrayfr",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void SetMatrixParameterArrayfr(Parameter param, long offset, long nelements,
                [In] float[] v);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetMatrixParameterArrayfr",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void SetMatrixParameterArrayfr(Parameter param, long offset, long nelements,
                [In] IntPtr v);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetMatrixParameterdc", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetMatrixParameterdc(Parameter param, [In] double* matrix);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetMatrixParameterdc", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetMatrixParameterdc(Parameter param, [In] double[] matrix);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetMatrixParameterdc", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetMatrixParameterdc(Parameter param, [In] IntPtr matrix);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetMatrixParameterdr", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetMatrixParameterdr(Parameter param, [In] double* matrix);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetMatrixParameterdr", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetMatrixParameterdr(Parameter param, [In] double[] matrix);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetMatrixParameterdr", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetMatrixParameterdr(Parameter param, [In] IntPtr matrix);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetMatrixParameterfc", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetMatrixParameterfc(Parameter param, [In] float* matrix);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetMatrixParameterfc", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetMatrixParameterfc(Parameter param, [In] float[] matrix);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetMatrixParameterfc", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetMatrixParameterfc(Parameter param, [In] IntPtr matrix);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetMatrixParameterfr", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetMatrixParameterfr(Parameter param, [In] float* matrix);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetMatrixParameterfr", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetMatrixParameterfr(Parameter param, [In] Matrix matrix);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetMatrixParameterfr", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetMatrixParameterfr(Parameter param, [In] IntPtr matrix);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetOptimalOptions", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetOptimalOptions(Profile profile);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameter1d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter1d(Parameter param, double x);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameter1dv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter1dv(Parameter param, [In] double[] values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameter1dv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter1dv(Parameter param, [In] IntPtr values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameter1dv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetParameter1dv(Parameter param, [In] double* values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameter1f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter1f(Parameter param, float x);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameter1fv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetParameter1fv(Parameter param, [In] float* values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameter1fv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter1fv(Parameter param, [In] float[] values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameter1fv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter1fv(Parameter param, [In] IntPtr values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameter2d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter2d(Parameter param, double x, double y);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameter2dv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetParameter2dv(Parameter param, [In] double* values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameter2dv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter2dv(Parameter param, [In] double[] values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameter2dv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter2dv(Parameter param, [In] IntPtr values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameter2f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter2f(Parameter param, float x, float y);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameter2fv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetParameter2fv(Parameter param, [In] float* values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameter2fv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter2fv(Parameter param, [In] float[] values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameter2fv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter2fv(Parameter param, [In] IntPtr values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameter3d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter3d(Parameter param, double x, double y, double z);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameter3dv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter3dv(Parameter param, [In] double[] values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameter3dv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter3dv(Parameter param, [In] IntPtr values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameter3dv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetParameter3dv(Parameter param, [In] double* values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameter3f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter3f(Parameter param, float x, float y, float z);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameter3fv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetParameter3fv(Parameter param, [In] float* values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameter3fv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter3fv(Parameter param, [In] float[] values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameter3fv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter3fv(Parameter param, [In] IntPtr values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameter4d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter4d(Parameter param, double x, double y, double z, double w);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameter4dv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetParameter4dv(Parameter param, [In] double* values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameter4dv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter4dv(Parameter param, [In] double[] values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameter4dv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter4dv(Parameter param, [In] IntPtr values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameter4f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter4f(Parameter param, float x, float y, float z, float w);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameter4fv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter4fv(Parameter param, [In] float[] values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameter4fv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetParameter4fv(Parameter param, [In] float* values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameter4fv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter4fv(Parameter param, [In] IntPtr values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameterArray1d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetParameterArray1d(Parameter param, long offset, long nelements,
                [In] double* values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameterArray1d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameterArray1d(Parameter param, long offset, long nelements,
                [In] double[] values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameterArray1d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameterArray1d(Parameter param, long offset, long nelements,
                [In] IntPtr values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameterArray1f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetParameterArray1f(Parameter param, long offset, long nelements,
                [In] float* values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameterArray1f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameterArray1f(Parameter param, long offset, long nelements,
                [In] float[] values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameterArray1f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameterArray1f(Parameter param, long offset, long nelements,
                [In] IntPtr values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameterArray2d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetParameterArray2d(Parameter param, long offset, long nelements,
                [In] double* values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameterArray2d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameterArray2d(Parameter param, long offset, long nelements,
                [In] double[] values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameterArray2d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameterArray2d(Parameter param, long offset, long nelements,
                [In] IntPtr values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameterArray2f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetParameterArray2f(Parameter param, long offset, long nelements,
                [In] float* values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameterArray2f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameterArray2f(Parameter param, long offset, long nelements,
                [In] float[] values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameterArray2f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameterArray2f(Parameter param, long offset, long nelements,
                [In] IntPtr values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameterArray3d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetParameterArray3d(Parameter param, long offset, long nelements,
                [In] double* values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameterArray3d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameterArray3d(Parameter param, long offset, long nelements,
                [In] double[] values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameterArray3d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameterArray3d(Parameter param, long offset, long nelements,
                [In] IntPtr values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameterArray3f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetParameterArray3f(Parameter param, long offset, long nelements,
                [In] float* values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameterArray3f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameterArray3f(Parameter param, long offset, long nelements,
                [In] float[] values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameterArray3f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameterArray3f(Parameter param, long offset, long nelements,
                [In] IntPtr values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameterArray4d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetParameterArray4d(Parameter param, long offset, long nelements,
                [In] double* values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameterArray4d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameterArray4d(Parameter param, long offset, long nelements,
                [In] double[] values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameterArray4d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameterArray4d(Parameter param, long offset, long nelements,
                [In] IntPtr values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameterArray4f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameterArray4f(Parameter param, long offset, long nelements,
                [In] IntPtr values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameterArray4f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetParameterArray4f(Parameter param, long offset, long nelements,
                [In] float* values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameterArray4f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameterArray4f(Parameter param, long offset, long nelements,
                [In] float[] values);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameterPointer", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetParameterPointer(Parameter param, int fsize, int type, int stride,
                [In] void* pointer);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetParameterPointer", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameterPointer(Parameter param, int fsize, int type, int stride,
                [In] IntPtr pointer);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetStateMatrixParameter",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void SetStateMatrixParameter(Parameter param, int matrix, int transform);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetTextureParameter", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetTextureParameter(Parameter param, int texobj);

            [DllImport(CgGlLib, EntryPoint = "cgGLSetupSampler", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetupSampler(Parameter param, int texobj);

            [DllImport(CgGlLib, EntryPoint = "cgGLUnbindProgram", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void UnbindProgram(Program profile); //TODO ���� cgGLUnbindProgram Profile profile)

            [DllImport(CgGlLib, EntryPoint = "cgGLUnloadProgram", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void UnloadProgram(Program program);
        }

        #endregion GL

        #region Wrapper

        public static IntPtr GetOptimalOptions(Profile profile)
        {
            return (App.RenderType == RenderType.OpenGl3
                ? GL.GetOptimalOptions(profile)
                : DX11.GetOptimalOptions(profile));
        }

        public static unsafe string[] ToStrArr(this IntPtr p)
        {
            var byteArray = (byte**)p;
            var buffer = new List<byte>();
            var lines = new List<string>();
            try
            {
                for (; *byteArray != null; byteArray++)
                {
                    for (byte* b = *byteArray; *b != '\0'; b++)
                        buffer.Add(*b);

                    lines.Add(new string(Encoding.ASCII.GetChars(buffer.ToArray())));
                    buffer.Clear();
                }
                return lines.Count > 0 ? lines.ToArray() : null;
            }
            catch
            {
                return lines.Count > 0 ? lines.ToArray() : null;
            }
        }

        public static string ToStr(this IntPtr s)
        {
            return Marshal.PtrToStringAnsi(s);
        }

        #endregion Wrapper
    }
}