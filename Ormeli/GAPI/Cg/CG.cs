using Ormeli.Math;
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

        [DllImport("cg.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "cgSetPassState"),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetPassState(Pass pass);

        [DllImport("cg.dll", EntryPoint = "cgAddStateEnumerant", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void AddStateEnumerant(State state, [In] string name, int value);

        [DllImport("cg.dll", EntryPoint = "cgCallStateResetCallback", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool CallStateResetCallback(StateAssignment stateassignment);

        [DllImport("cg.dll", EntryPoint = "cgCallStateSetCallback", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool CallStateSetCallback(StateAssignment stateassignment);

        [DllImport("cg.dll", EntryPoint = "cgCallStateValidateCallback", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool CallStateValidateCallback(StateAssignment stateassignment);

        [DllImport("cg.dll", EntryPoint = "cgCombinePrograms", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Program CombinePrograms(int n, [In] Program[] exeList);

        [DllImport("cg.dll", EntryPoint = "cgCombinePrograms2", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Program CombinePrograms2([In] Program program1, [In] Program program2);

        [DllImport("cg.dll", EntryPoint = "cgCombinePrograms3", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Program CombinePrograms3([In] Program program1, [In] Program program2,
            [In] Program program3);

        [DllImport("cg.dll", EntryPoint = "cgCombinePrograms4", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Program CombinePrograms4([In] Program program1, [In] Program program2,
            [In] Program program3, [In] Program program4);

        [DllImport("cg.dll", EntryPoint = "cgCombinePrograms5", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Program CombinePrograms5([In] Program program1, [In] Program program2,
            [In] Program program3, [In] Program program4, [In] Program program5);

        [DllImport("cg.dll", EntryPoint = "cgCompileProgram", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void CompileProgram(Program program);

        [DllImport("cg.dll", EntryPoint = "cgConnectParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void ConnectParameter(Parameter from, Parameter to);

        [DllImport("cg.dll", EntryPoint = "cgCopyEffect", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Effect CopyEffect(Effect effect);

        [DllImport("cg.dll", EntryPoint = "cgCopyProgram", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Program CopyProgram(Program program);

        [DllImport("cg.dll", EntryPoint = "cgCreateArraySamplerState", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern State CreateArraySamplerState(Context context, [In] string name, Type type, int nelems);

        [DllImport("cg.dll", EntryPoint = "cgCreateArrayState", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern State CreateArrayState(Context context, [In] string name, Type type, int nelems);

        [DllImport("cg.dll", EntryPoint = "cgCreateBuffer", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Buffer CreateBuffer(Context context, int size, [In] IntPtr data, BufferUsage bufferUsage);

        [DllImport("cg.dll", EntryPoint = "cgCreateContext", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Context CreateContext();

        [DllImport("cg.dll", EntryPoint = "cgCreateEffect", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi), SuppressUnmanagedCodeSecurity]
        public static extern Effect CreateEffect(Context context, [In] string code, [In] string[] args);

        [DllImport("cg.dll", EntryPoint = "cgCreateEffectAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Annotation CreateEffectAnnotation(Effect effect, [In] string name, Type type);

        [DllImport("cg.dll", EntryPoint = "cgCreateEffectFromFile", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Effect CreateEffectFromFile(Context context, [In] string filename, [In] string[] args);

        [DllImport("cg.dll", EntryPoint = "cgCreateEffectParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter CreateEffectParameter(Effect effect, [In] string name, Type type);

        [DllImport("cg.dll", EntryPoint = "cgCreateEffectParameterArray", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter CreateEffectParameterArray(Effect effect, [In] string name, Type type, int length);

        [DllImport("cg.dll", EntryPoint = "cgCreateEffectParameterMultiDimArray",
            CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern Parameter CreateEffectParameterMultiDimArray(Effect effect, [In] string name, Type type,
            int dim, [In] int[] lengths);

        [DllImport("cg.dll", EntryPoint = "cgCreateObj", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Obj CreateObj(Context context, Enum programType, [In] string source, Profile profile,
            [In] string[] args);

        [DllImport("cg.dll", EntryPoint = "cgCreateObjFromFile", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Obj CreateObjFromFile(Context context, Enum programType, [In] string sourceFile,
            Profile profile, [In] string[] args);

        [DllImport("cg.dll", EntryPoint = "cgCreateParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter CreateParameter(Context context, Type type);

        [DllImport("cg.dll", EntryPoint = "cgCreateParameterAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Annotation CreateParameterAnnotation(Parameter parameter, [In] string name, Type type);

        [DllImport("cg.dll", EntryPoint = "cgCreateParameterArray", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter CreateParameterArray(Context context, Type type, int length);

        [DllImport("cg.dll", EntryPoint = "cgCreateParameterMultiDimArray", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter CreateParameterMultiDimArray(Context context, Type type, int dim,
            [In] int[] lengths);

        [DllImport("cg.dll", EntryPoint = "cgCreatePass", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Pass CreatePass(Technique technique, [In] string name);

        [DllImport("cg.dll", EntryPoint = "cgCreatePassAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Annotation CreatePassAnnotation(Pass pass, [In] string name, Type type);

        [DllImport("cg.dll", EntryPoint = "cgCreateProgram", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Program CreateProgram(Context context, Enum programType, [In] string program,
            Profile profile, [In] string entry, [In] string[] args);

        [DllImport("cg.dll", EntryPoint = "cgCreateProgramAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Annotation CreateProgramAnnotation(Program program, [In] string name, Type type);

        [DllImport("cg.dll", EntryPoint = "cgCreateProgramFromEffect", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr CreateProgramFromEffect(Effect effect, Profile profile, [In] string entry,
            [In] string[] args);

        [DllImport("cg.dll", EntryPoint = "cgCreateProgramFromFile", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Program CreateProgramFromFile(Context context, Enum programType, [In] string programFile,
            Profile profile, [In] string entry, IntPtr args);

        [DllImport("cg.dll", EntryPoint = "cgCreateSamplerState", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern State CreateSamplerState(Context context, [In] string name, Type type);

        [DllImport("cg.dll", EntryPoint = "cgCreateSamplerStateAssignment", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern StateAssignment CreateSamplerStateAssignment(Parameter parameter, State state);

        [DllImport("cg.dll", EntryPoint = "cgCreateState", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern State CreateState(Context context, [In] string name, Type type);

        [DllImport("cg.dll", EntryPoint = "cgCreateStateAssignment", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern StateAssignment CreateStateAssignment(Pass pass, State state);

        [DllImport("cg.dll", EntryPoint = "cgCreateStateAssignmentIndex", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern StateAssignment CreateStateAssignmentIndex(Pass pass, State state, int index);

        [DllImport("cg.dll", EntryPoint = "cgCreateTechnique", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Technique CreateTechnique(Effect effect, [In] string name);

        [DllImport("cg.dll", EntryPoint = "cgCreateTechniqueAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Annotation CreateTechniqueAnnotation(Technique technique, [In] string name, Type type);

        [DllImport("cg.dll", EntryPoint = "cgDestroyBuffer", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void DestroyBuffer(Buffer buffer);

        [DllImport("cg.dll", EntryPoint = "cgDestroyContext", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void DestroyContext(Context context);

        [DllImport("cg.dll", EntryPoint = "cgDestroyEffect", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void DestroyEffect(Effect effect);

        [DllImport("cg.dll", EntryPoint = "cgDestroyObj", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void DestroyObj(Obj obj);

        [DllImport("cg.dll", EntryPoint = "cgDestroyParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void DestroyParameter(Parameter param);

        [DllImport("cg.dll", EntryPoint = "cgDestroyProgram", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void DestroyProgram(Program program);

        [DllImport("cg.dll", EntryPoint = "cgDisconnectParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void DisconnectParameter(Parameter param);

        [DllImport("cg.dll", EntryPoint = "cgEvaluateProgram", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void EvaluateProgram(Program program, [In] [Out] float[] values, int ncomps, int nx, int ny,
            int nz);

        [DllImport("cg.dll", EntryPoint = "cgGetAnnotationName", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetAnnotationName(Annotation annotation);

        [DllImport("cg.dll", EntryPoint = "cgGetAnnotationType", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Type GetAnnotationType(Annotation annotation);

        [DllImport("cg.dll", EntryPoint = "cgGetArrayDimension", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetArrayDimension(Parameter param);

        [DllImport("cg.dll", EntryPoint = "cgGetArrayParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetArrayParameter(Parameter aparam, int index);

        [DllImport("cg.dll", EntryPoint = "cgGetArraySize", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetArraySize(Parameter param, int dimension);

        [DllImport("cg.dll", EntryPoint = "cgGetArrayTotalSize", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetArrayTotalSize(Parameter param);

        [DllImport("cg.dll", EntryPoint = "cgGetArrayType", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Type GetArrayType(Parameter param);

        [DllImport("cg.dll", EntryPoint = "cgGetAutoCompile", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Enum GetAutoCompile(Context context);

        [DllImport("cg.dll", EntryPoint = "cgGetBehavior", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Behavior GetBehavior(string behaviorString);

        [DllImport("cg.dll", EntryPoint = "cgGetBehaviorString", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetBehaviorString(Behavior behavior);

        [DllImport("cg.dll", EntryPoint = "cgGetBoolAnnotationValues", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetBoolAnnotationValues(Annotation annotation, out int nvalues);

        [DllImport("cg.dll", EntryPoint = "cgGetBoolStateAssignmentValues", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool[] GetBoolStateAssignmentValues(StateAssignment stateassignment, int[] nVals);

        [DllImport("cg.dll", EntryPoint = "cgGetBooleanAnnotationValues", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int[] GetBooleanAnnotationValues(Annotation annotation, out int nvalues);

        [DllImport("cg.dll", EntryPoint = "cgGetBufferSize", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetBufferSize(Buffer buffer);

        [DllImport("cg.dll", EntryPoint = "cgGetCompilerIncludeCallback", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IncludeCallbackFuncDelegate GetCompilerIncludeCallback(Context context);

        [DllImport("cg.dll", EntryPoint = "cgGetConnectedParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetConnectedParameter(Parameter param);

        [DllImport("cg.dll", EntryPoint = "cgGetConnectedStateAssignmentParameter",
            CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetConnectedStateAssignmentParameter(StateAssignment stateassignment);

        [DllImport("cg.dll", EntryPoint = "cgGetConnectedToParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetConnectedToParameter(Parameter param, int index);

        [DllImport("cg.dll", EntryPoint = "cgGetContextBehavior", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Behavior GetContextBehavior(Context context);

        [DllImport("cg.dll", EntryPoint = "cgGetDependentAnnotationParameter",
            CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetDependentAnnotationParameter(Annotation annotation, int index);

        [DllImport("cg.dll", EntryPoint = "cgGetDependentProgramArrayStateAssignmentParameter",
            CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetDependentProgramArrayStateAssignmentParameter(StateAssignment sa, int index);

        [DllImport("cg.dll", EntryPoint = "cgGetDependentStateAssignmentParameter",
            CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetDependentStateAssignmentParameter(StateAssignment stateassignment, int index);

        [DllImport("cg.dll", EntryPoint = "cgGetDomain", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Domain GetDomain(string domainString);

        [DllImport("cg.dll", EntryPoint = "cgGetDomainString", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetDomainString(Domain domain);

        [DllImport("cg.dll", EntryPoint = "cgGetEffectContext", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Context GetEffectContext(Effect effect);

        [DllImport("cg.dll", EntryPoint = "cgGetEffectName", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern string GetEffectName(Effect effect);

        [DllImport("cg.dll", EntryPoint = "cgGetEffectParameterBuffer", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Buffer GetEffectParameterBuffer(Parameter param);

        [DllImport("cg.dll", EntryPoint = "cgGetEffectParameterBySemantic", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetEffectParameterBySemantic(Effect effect, [In] string name);

        [DllImport("cg.dll", EntryPoint = "cgGetEnum", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Enum GetEnum([In] string enumString);

        [DllImport("cg.dll", EntryPoint = "cgGetEnumString", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetEnumString(Enum en);

        [DllImport("cg.dll", EntryPoint = "cgGetError", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Error GetError();

        [DllImport("cg.dll", EntryPoint = "cgGetErrorCallback", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern ErrorCallbackFuncDelegate GetErrorCallback();

        [DllImport("cg.dll", EntryPoint = "cgGetErrorHandler", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern ErrorHandlerFuncDelegate GetErrorHandler(out IntPtr data);

        [DllImport("cg.dll", EntryPoint = "cgGetErrorString", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetErrorString(Error error);

        [DllImport("cg.dll", EntryPoint = "cgGetFirstDependentParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetFirstDependentParameter(Parameter param);

        [DllImport("cg.dll", EntryPoint = "cgGetFirstEffect", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Effect GetFirstEffect(Context context);

        [DllImport("cg.dll", EntryPoint = "cgGetFirstEffectAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Annotation GetFirstEffectAnnotation(Effect effect);

        [DllImport("cg.dll", EntryPoint = "cgGetFirstEffectParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetFirstEffectParameter(Effect effect);

        [DllImport("cg.dll", EntryPoint = "cgGetFirstError", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Error GetFirstError();

        [DllImport("cg.dll", EntryPoint = "cgGetFirstLeafEffectParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetFirstLeafEffectParameter(Effect effect);

        [DllImport("cg.dll", EntryPoint = "cgGetFirstLeafParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetFirstLeafParameter(Program program, Enum nameSpace);

        [DllImport("cg.dll", EntryPoint = "cgGetFirstParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetFirstParameter(Program prog, Enum nameSpace);

        [DllImport("cg.dll", EntryPoint = "cgGetFirstParameterAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Annotation GetFirstParameterAnnotation(Parameter param);

        [DllImport("cg.dll", EntryPoint = "cgGetFirstPass", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Pass GetFirstPass(Technique technique);

        [DllImport("cg.dll", EntryPoint = "cgGetFirstPassAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Annotation GetFirstPassAnnotation(Pass pass);

        [DllImport("cg.dll", EntryPoint = "cgGetFirstProgram", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Program GetFirstProgram(Context context);

        [DllImport("cg.dll", EntryPoint = "cgGetFirstProgramAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Annotation GetFirstProgramAnnotation(Program prog);

        [DllImport("cg.dll", EntryPoint = "cgGetFirstSamplerState", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern State GetFirstSamplerState(Context context);

        [DllImport("cg.dll", EntryPoint = "cgGetFirstSamplerStateAssignment",
            CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern StateAssignment GetFirstSamplerStateAssignment(Parameter param);

        [DllImport("cg.dll", EntryPoint = "cgGetFirstState", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern State GetFirstState(Context context);

        [DllImport("cg.dll", EntryPoint = "cgGetFirstStateAssignment", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern StateAssignment GetFirstStateAssignment(Pass pass);

        [DllImport("cg.dll", EntryPoint = "cgGetFirstStructParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetFirstStructParameter(Parameter param);

        [DllImport("cg.dll", EntryPoint = "cgGetFirstTechnique", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Technique GetFirstTechnique(Effect effect);

        [DllImport("cg.dll", EntryPoint = "cgGetFirstTechniqueAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Annotation GetFirstTechniqueAnnotation(Technique technique);

        [DllImport("cg.dll", EntryPoint = "cgGetFloatAnnotationValues", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern float[] GetFloatAnnotationValues(Annotation annotation, out int nvalues);

        [DllImport("cg.dll", EntryPoint = "cgGetFloatStateAssignmentValues", CallingConvention = CallingConvention.Cdecl
            ), SuppressUnmanagedCodeSecurity]
        public static extern float[] GetFloatStateAssignmentValues(StateAssignment stateassignment, int[] nvalues);

        [DllImport("cg.dll", EntryPoint = "cgGetIntAnnotationValues", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int[] GetIntAnnotationValues(Annotation annotation, out int nvalues);

        [DllImport("cg.dll", EntryPoint = "cgGetIntStateAssignmentValues", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int[] GetIntStateAssignmentValues(StateAssignment stateassignment, int[] nvalues);

        [DllImport("cg.dll", EntryPoint = "cgGetLastErrorString", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetLastErrorString(out Error error);

        [DllImport("cg.dll", EntryPoint = "cgGetLastListing", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetLastListing(Context context);

        [DllImport("cg.dll", EntryPoint = "cgGetLockingPolicy", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Enum GetLockingPolicy();

        [DllImport("cg.dll", EntryPoint = "cgGetMatrixParameterOrder", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Enum GetMatrixParameterOrder(Parameter param);

        [DllImport("cg.dll", EntryPoint = "cgGetMatrixParameterdc", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void GetMatrixParameterdc(Parameter param, [In] double[] matrix);

        [DllImport("cg.dll", EntryPoint = "cgGetMatrixParameterdr", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void GetMatrixParameterdr(Parameter param, [In] double[] matrix);

        [DllImport("cg.dll", EntryPoint = "cgGetMatrixParameterfc", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void GetMatrixParameterfc(Parameter param, [In] float[] matrix);

        [DllImport("cg.dll", EntryPoint = "cgGetMatrixParameterfr", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void GetMatrixParameterfr(Parameter param, [In] float[] matrix);

        [DllImport("cg.dll", EntryPoint = "cgGetMatrixParameteric", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void GetMatrixParameteric(Parameter param, [In] int[] matrix);

        [DllImport("cg.dll", EntryPoint = "cgGetMatrixParameterir", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void GetMatrixParameterir(Parameter param, [In] int[] matrix);

        [DllImport("cg.dll", EntryPoint = "cgGetMatrixSize", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void GetMatrixSize(Type type, out int nrows, out int ncols);

        [DllImport("cg.dll", EntryPoint = "cgGetNamedEffect", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Effect GetNamedEffect(Context context, [In] string name);

        [DllImport("cg.dll", EntryPoint = "cgGetNamedEffectAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Annotation GetNamedEffectAnnotation(Effect effect, [In] string name);

        [DllImport("cg.dll", EntryPoint = "cgGetNamedEffectParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetNamedEffectParameter(Effect effect, [In] string name);

        [DllImport("cg.dll", EntryPoint = "cgGetNamedParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetNamedParameter(Program program, [In] string parameter);

        [DllImport("cg.dll", EntryPoint = "cgGetNamedParameterAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Annotation GetNamedParameterAnnotation(Parameter param, [In] string name);

        [DllImport("cg.dll", EntryPoint = "cgGetNamedPass", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Pass GetNamedPass(Technique technique, [In] string name);

        [DllImport("cg.dll", EntryPoint = "cgGetNamedPassAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Annotation GetNamedPassAnnotation(Pass pass, [In] string name);

        [DllImport("cg.dll", EntryPoint = "cgGetNamedProgramAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Annotation GetNamedProgramAnnotation(Program prog, [In] string name);

        [DllImport("cg.dll", EntryPoint = "cgGetNamedProgramParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetNamedProgramParameter(Program program, Enum nameSpace, [In] string name);

        [DllImport("cg.dll", EntryPoint = "cgGetNamedSamplerState", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern State GetNamedSamplerState(Context context, [In] string name);

        [DllImport("cg.dll", EntryPoint = "cgGetNamedSamplerStateAssignment",
            CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern StateAssignment GetNamedSamplerStateAssignment(Parameter param, [In] string name);

        [DllImport("cg.dll", EntryPoint = "cgGetNamedState", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern State GetNamedState(Context context, [In] string name);

        [DllImport("cg.dll", EntryPoint = "cgGetNamedStateAssignment", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern StateAssignment GetNamedStateAssignment(Pass pass, [In] string name);

        [DllImport("cg.dll", EntryPoint = "cgGetNamedStructParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetNamedStructParameter(Parameter param, [In] string name);

        [DllImport("cg.dll", EntryPoint = "cgGetNamedSubParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetNamedSubParameter(Parameter param, [In] string name);

        [DllImport("cg.dll", EntryPoint = "cgGetNamedTechnique", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Technique GetNamedTechnique(Effect effect, [In] string name);

        [DllImport("cg.dll", EntryPoint = "cgGetNamedTechniqueAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Annotation GetNamedTechniqueAnnotation(Technique technique, [In] string name);

        [DllImport("cg.dll", EntryPoint = "cgGetNamedUserType", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Type GetNamedUserType(Handle handle, [In] string name);

        [DllImport("cg.dll", EntryPoint = "cgGetNextAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Annotation GetNextAnnotation(Annotation annotation);

        [DllImport("cg.dll", EntryPoint = "cgGetNextEffect", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Effect GetNextEffect(Effect effect);

        [DllImport("cg.dll", EntryPoint = "cgGetNextLeafParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetNextLeafParameter(Parameter current);

        [DllImport("cg.dll", EntryPoint = "cgGetNextParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetNextParameter(Parameter current);

        [DllImport("cg.dll", EntryPoint = "cgGetNextPass", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Pass GetNextPass(Pass pass);

        [DllImport("cg.dll", EntryPoint = "cgGetNextProgram", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Program GetNextProgram(Program program);

        [DllImport("cg.dll", EntryPoint = "cgGetNextState", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern State GetNextState(State state);

        [DllImport("cg.dll", EntryPoint = "cgGetNextStateAssignment", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern StateAssignment GetNextStateAssignment(StateAssignment stateassignment);

        [DllImport("cg.dll", EntryPoint = "cgGetNextTechnique", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Technique GetNextTechnique(Technique technique);

        [DllImport("cg.dll", EntryPoint = "cgGetNumConnectedToParameters", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetNumConnectedToParameters(Parameter param);

        [DllImport("cg.dll", EntryPoint = "cgGetNumDependentAnnotationParameters",
            CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int GetNumDependentAnnotationParameters(Annotation annotation);

        [DllImport("cg.dll", EntryPoint = "cgGetNumDependentProgramArrayStateAssignmentParameters",
            CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int GetNumDependentProgramArrayStateAssignmentParameters(StateAssignment sa);

        [DllImport("cg.dll", EntryPoint = "cgGetNumDependentStateAssignmentParameters",
            CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int GetNumDependentStateAssignmentParameters(StateAssignment stateassignment);

        [DllImport("cg.dll", EntryPoint = "cgGetNumParentTypes", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetNumParentTypes(Type type);

        [DllImport("cg.dll", EntryPoint = "cgGetNumProgramDomains", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetNumProgramDomains(Program program);

        [DllImport("cg.dll", EntryPoint = "cgGetNumStateEnumerants", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetNumStateEnumerants(State state);

        [DllImport("cg.dll", EntryPoint = "cgGetNumSupportedProfiles", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetNumSupportedProfiles();

        [DllImport("cg.dll", EntryPoint = "cgGetNumUserTypes", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetNumUserTypes(Handle handle);

        [DllImport("cg.dll", EntryPoint = "cgGetParameterBaseResource", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Resource GetParameterBaseResource(Parameter param);

        [DllImport("cg.dll", EntryPoint = "cgGetParameterBaseType", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Type GetParameterBaseType(Parameter param);

        [DllImport("cg.dll", EntryPoint = "cgGetParameterBufferIndex", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterBufferIndex(Parameter parameter);

        [DllImport("cg.dll", EntryPoint = "cgGetParameterBufferOffset", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterBufferOffset(Parameter parameter);

        [DllImport("cg.dll", EntryPoint = "cgGetParameterClass", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern ParameterClass GetParameterClass(Parameter param);

        [DllImport("cg.dll", EntryPoint = "cgGetParameterClassEnum", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern ParameterClass GetParameterClassEnum(string pString);

        [DllImport("cg.dll", EntryPoint = "cgGetParameterClassString", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetParameterClassString(ParameterClass pc);

        [DllImport("cg.dll", EntryPoint = "cgGetParameterColumns", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterColumns(Parameter param);

        [DllImport("cg.dll", EntryPoint = "cgGetParameterContext", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Context GetParameterContext(Parameter param);

        [DllImport("cg.dll", EntryPoint = "cgGetParameterDefaultValuedc", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterDefaultValuedc(Parameter param, int nelements, [Out] double[] vals);

        [DllImport("cg.dll", EntryPoint = "cgGetParameterDefaultValuedr", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterDefaultValuedr(Parameter param, int nelements, [Out] double[] vals);

        [DllImport("cg.dll", EntryPoint = "cgGetParameterDefaultValuefc", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterDefaultValuefc(Parameter param, int nelements, [Out] float[] vals);

        [DllImport("cg.dll", EntryPoint = "cgGetParameterDefaultValuefr", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterDefaultValuefr(Parameter param, int nelements, [Out] float[] vals);

        [DllImport("cg.dll", EntryPoint = "cgGetParameterDefaultValueic", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterDefaultValueic(Parameter param, int nelements, [Out] int[] vals);

        [DllImport("cg.dll", EntryPoint = "cgGetParameterDefaultValueir", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterDefaultValueir(Parameter param, int nelements, [Out] int[] vals);

        [DllImport("cg.dll", EntryPoint = "cgGetParameterDirection", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Enum GetParameterDirection(Parameter param);

        [DllImport("cg.dll", EntryPoint = "cgGetParameterEffect", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Effect GetParameterEffect(Parameter param);

        [DllImport("cg.dll", EntryPoint = "cgGetParameterIndex", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterIndex(Parameter param);

        [DllImport("cg.dll", EntryPoint = "cgGetParameterName", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetParameterName(Parameter param);

        [DllImport("cg.dll", EntryPoint = "cgGetParameterNamedType", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterNamedType(Parameter param);

        [DllImport("cg.dll", EntryPoint = "cgGetParameterOrdinalNumber", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterOrdinalNumber(Parameter param);

        [DllImport("cg.dll", EntryPoint = "cgGetParameterProgram", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Program GetParameterProgram(Parameter param);

        [DllImport("cg.dll", EntryPoint = "cgGetParameterResource", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Resource GetParameterResource(Parameter param);

        [DllImport("cg.dll", EntryPoint = "cgGetParameterResourceIndex", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterResourceIndex(Parameter param);

        [DllImport("cg.dll", EntryPoint = "cgGetParameterResourceName", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern string GetParameterResourceName(Parameter param);

        [DllImport("cg.dll", EntryPoint = "cgGetParameterResourceSize", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterResourceSize(Parameter parameter);

        [DllImport("cg.dll", EntryPoint = "cgGetParameterResourceType", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Type GetParameterResourceType(Parameter parameter);

        [DllImport("cg.dll", EntryPoint = "cgGetParameterRows", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterRows(Parameter param);

        [DllImport("cg.dll", EntryPoint = "cgGetParameterSemantic", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetParameterSemantic(Parameter param);

        [DllImport("cg.dll", EntryPoint = "cgGetParameterSettingMode", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Enum GetParameterSettingMode(Context context);

        [DllImport("cg.dll", EntryPoint = "cgGetParameterType", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Type GetParameterType(Parameter param);

        [DllImport("cg.dll", EntryPoint = "cgGetParameterValuedc", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterValuedc(Parameter param, int nelements, [Out] double[] vals);

        [DllImport("cg.dll", EntryPoint = "cgGetParameterValuedr", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterValuedr(Parameter param, int nelements, [Out] double[] vals);

        [DllImport("cg.dll", EntryPoint = "cgGetParameterValuefc", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterValuefc(Parameter param, int nelements, [Out] float[] vals);

        [DllImport("cg.dll", EntryPoint = "cgGetParameterValuefr", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterValuefr(Parameter param, int nelements, [Out] float[] vals);

        [DllImport("cg.dll", EntryPoint = "cgGetParameterValueic", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterValueic(Parameter param, int nelements, [Out] int[] vals);

        [DllImport("cg.dll", EntryPoint = "cgGetParameterValueir", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetParameterValueir(Parameter param, int nelements, [Out] int[] vals);

        [DllImport("cg.dll", EntryPoint = "cgGetParameterValues", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern unsafe double* GetParameterValues(Parameter param, Enum value_type, int* nvalues);

        [DllImport("cg.dll", EntryPoint = "cgGetParameterValues", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern double[] GetParameterValues(Parameter param, Enum value_type, [In] int[] nvalues);

        [DllImport("cg.dll", EntryPoint = "cgGetParameterVariability", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Enum GetParameterVariability(Parameter param);

        [DllImport("cg.dll", EntryPoint = "cgGetParentType", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Type GetParentType(Type type, int index);

        [DllImport("cg.dll", EntryPoint = "cgGetPassName", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern string GetPassName(Pass pass);

        [DllImport("cg.dll", EntryPoint = "cgGetPassProgram", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Program GetPassProgram(Pass pass, Domain domain);

        [DllImport("cg.dll", EntryPoint = "cgGetPassTechnique", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Technique GetPassTechnique(Pass pass);

        [DllImport("cg.dll", EntryPoint = "cgGetProfile", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Profile GetProfile([In] string profile_string);

        [DllImport("cg.dll", EntryPoint = "cgGetProfileDomain", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Domain GetProfileDomain(Profile profile);

        [DllImport("cg.dll", EntryPoint = "cgGetProfileProperty", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool GetProfileProperty(Profile profile, Enum query);

        [DllImport("cg.dll", EntryPoint = "cgGetProfileString", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern string GetProfileString(Profile profile);

        [DllImport("cg.dll", EntryPoint = "cgGetProgramBuffer", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Buffer GetProgramBuffer(Program program, int bufferIndex);

        [DllImport("cg.dll", EntryPoint = "cgGetProgramBufferMaxIndex", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetProgramBufferMaxIndex(Profile profile);

        [DllImport("cg.dll", EntryPoint = "cgGetProgramBufferMaxSize", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetProgramBufferMaxSize(Profile profile);

        [DllImport("cg.dll", EntryPoint = "cgGetProgramContext", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Context GetProgramContext(Program program);

        [DllImport("cg.dll", EntryPoint = "cgGetProgramDomain", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Domain GetProgramDomain(Program program);

        [DllImport("cg.dll", EntryPoint = "cgGetProgramDomainProfile", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Profile GetProgramDomainProfile(Program program, int index);

        [DllImport("cg.dll", EntryPoint = "cgGetProgramDomainProgram", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Program GetProgramDomainProgram(Program program, int index);

        [DllImport("cg.dll", EntryPoint = "cgGetProgramInput", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Enum GetProgramInput(Program program);

        [DllImport("cg.dll", EntryPoint = "cgGetProgramOptions", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetProgramOptions(Program prog);

        [DllImport("cg.dll", EntryPoint = "cgGetProgramOutput", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Enum GetProgramOutput(Program program);

        [DllImport("cg.dll", EntryPoint = "cgGetProgramProfile", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Profile GetProgramProfile(Program program);

        [DllImport("cg.dll", EntryPoint = "cgGetProgramStateAssignmentValue",
            CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern Program GetProgramStateAssignmentValue(StateAssignment stateassignment);

        [DllImport("cg.dll", EntryPoint = "cgGetProgramString", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetProgramString(Program program, Enum sourceType);

        [DllImport("cg.dll", EntryPoint = "cgGetResource", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Resource GetResource([In] string resource_string);

        [DllImport("cg.dll", EntryPoint = "cgGetResourceString", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetResourceString(Resource resource);

        [DllImport("cg.dll", EntryPoint = "cgGetSamplerStateAssignmentParameter",
            CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetSamplerStateAssignmentParameter(StateAssignment stateassignment);

        [DllImport("cg.dll", EntryPoint = "cgGetSamplerStateAssignmentState",
            CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern State GetSamplerStateAssignmentState(StateAssignment stateassignment);

        [DllImport("cg.dll", EntryPoint = "cgGetSamplerStateAssignmentValue",
            CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetSamplerStateAssignmentValue(StateAssignment stateassignment);

        [DllImport("cg.dll", EntryPoint = "cgGetSemanticCasePolicy", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Enum GetSemanticCasePolicy();

        [DllImport("cg.dll", EntryPoint = "cgGetStateAssignmentIndex", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetStateAssignmentIndex(StateAssignment stateassignment);

        [DllImport("cg.dll", EntryPoint = "cgGetStateAssignmentPass", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Pass GetStateAssignmentPass(StateAssignment stateassignment);

        [DllImport("cg.dll", EntryPoint = "cgGetStateAssignmentState", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern State GetStateAssignmentState(StateAssignment stateassignment);

        [DllImport("cg.dll", EntryPoint = "cgGetStateContext", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Context GetStateContext(State state);

        [DllImport("cg.dll", EntryPoint = "cgGetStateEnumerant", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern string GetStateEnumerant(State state, int index, out int value);

        [DllImport("cg.dll", EntryPoint = "cgGetStateEnumerantName", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern string GetStateEnumerantName(State state, int value);

        [DllImport("cg.dll", EntryPoint = "cgGetStateEnumerantValue", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern int GetStateEnumerantValue(State state, [In] string name);

        [DllImport("cg.dll", EntryPoint = "cgGetStateLatestProfile", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Profile GetStateLatestProfile(State state);

        [DllImport("cg.dll", EntryPoint = "cgGetStateName", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern string GetStateName(State state);

        [DllImport("cg.dll", EntryPoint = "cgGetStateResetCallback", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern StateCallbackDelegate GetStateResetCallback(State state);

        [DllImport("cg.dll", EntryPoint = "cgGetStateSetCallback", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern StateCallbackDelegate GetStateSetCallback(State state);

        [DllImport("cg.dll", EntryPoint = "cgGetStateType", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Type GetStateType(State state);

        [DllImport("cg.dll", EntryPoint = "cgGetStateValidateCallback", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern StateCallbackDelegate GetStateValidateCallback(State state);

        [DllImport("cg.dll", EntryPoint = "cgGetString", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetString(Enum sname);

        [DllImport("cg.dll", EntryPoint = "cgGetStringAnnotationValue", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern string GetStringAnnotationValue(Annotation annotation);

        [DllImport("cg.dll", EntryPoint = "cgGetStringAnnotationValues", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetStringAnnotationValues(Annotation ann, out int nvalues);

        [DllImport("cg.dll", EntryPoint = "cgGetStringParameterValue", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern string GetStringParameterValue(Parameter param);

        [DllImport("cg.dll", EntryPoint = "cgGetStringStateAssignmentValue", CallingConvention = CallingConvention.Cdecl
            ), SuppressUnmanagedCodeSecurity]
        public static extern string GetStringStateAssignmentValue(StateAssignment stateassignment);

        [DllImport("cg.dll", EntryPoint = "cgGetSupportedProfile", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Profile GetSupportedProfile(int index);

        [DllImport("cg.dll", EntryPoint = "cgGetTechniqueEffect", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Effect GetTechniqueEffect(Technique technique);

        [DllImport("cg.dll", EntryPoint = "cgGetTechniqueName", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetTechniqueName(Technique technique);

        [DllImport("cg.dll", EntryPoint = "cgGetTextureStateAssignmentValue",
            CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern Parameter GetTextureStateAssignmentValue(StateAssignment stateassignment);

        [DllImport("cg.dll", EntryPoint = "cgGetType", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Type GetType([In] string type_string);

        [DllImport("cg.dll", EntryPoint = "cgGetTypeBase", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Type GetTypeBase(Type type);

        [DllImport("cg.dll", EntryPoint = "cgGetTypeClass", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern ParameterClass GetTypeClass(Type type);

        [DllImport("cg.dll", EntryPoint = "cgGetTypeSizes", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool GetTypeSizes(Type type, out int nrows, out int ncols);

        [DllImport("cg.dll", EntryPoint = "cgGetTypeString", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetTypeString(Type type);

        [DllImport("cg.dll", EntryPoint = "cgGetUserType", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Type GetUserType(Handle handle, int index);

        [DllImport("cg.dll", EntryPoint = "cgIsAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool IsAnnotation(Annotation annotation);

        [DllImport("cg.dll", EntryPoint = "cgIsContext", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool IsContext(Context context);

        [DllImport("cg.dll", EntryPoint = "cgIsEffect", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool IsEffect(Effect effect);

        [DllImport("cg.dll", EntryPoint = "cgIsInterfaceType", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool IsInterfaceType(Type type);

        [DllImport("cg.dll", EntryPoint = "cgIsParameter", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool IsParameter(Parameter param);

        [DllImport("cg.dll", EntryPoint = "cgIsParameterGlobal", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool IsParameterGlobal(Parameter param);

        [DllImport("cg.dll", EntryPoint = "cgIsParameterReferenced", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool IsParameterReferenced(Parameter param);

        [DllImport("cg.dll", EntryPoint = "cgIsParameterUsed", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool IsParameterUsed(Parameter param, Handle handle);

        [DllImport("cg.dll", EntryPoint = "cgIsParentType", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool IsParentType(Type parent, Type child);

        [DllImport("cg.dll", EntryPoint = "cgIsPass", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool IsPass(Pass pass);

        [DllImport("cg.dll", EntryPoint = "cgIsProfileSupported", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool IsProfileSupported(Profile profile);

        [DllImport("cg.dll", EntryPoint = "cgIsProgram", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool IsProgram(Program program);

        [DllImport("cg.dll", EntryPoint = "cgIsProgramCompiled", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool IsProgramCompiled(Program program);

        [DllImport("cg.dll", EntryPoint = "cgIsState", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool IsState(State state);

        [DllImport("cg.dll", EntryPoint = "cgIsStateAssignment", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool IsStateAssignment(StateAssignment stateassignment);

        [DllImport("cg.dll", EntryPoint = "cgIsTechnique", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool IsTechnique(Technique technique);

        [DllImport("cg.dll", EntryPoint = "cgIsTechniqueValidated", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool IsTechniqueValidated(Technique technique);

        [DllImport("cg.dll", EntryPoint = "cgMapBuffer", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern IntPtr MapBuffer(Buffer buffer, BufferAccess access);

        [DllImport("cg.dll", EntryPoint = "cgResetPassState", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void ResetPassState(Pass pass);

        [DllImport("cg.dll", EntryPoint = "cgSetArraySize", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetArraySize(Parameter param, int size);

        [DllImport("cg.dll", EntryPoint = "cgSetAutoCompile", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetAutoCompile(Context context, Enum autoCompileMode);

        [DllImport("cg.dll", EntryPoint = "cgSetBoolAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool SetBoolAnnotation(Annotation annotation, Bool value);

        [DllImport("cg.dll", EntryPoint = "cgSetBoolArrayStateAssignment", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool SetBoolArrayStateAssignment(StateAssignment stateassignment, [In] Bool[] vals);

        [DllImport("cg.dll", EntryPoint = "cgSetBoolStateAssignment", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool SetBoolStateAssignment(StateAssignment stateassignment, Bool value);

        [DllImport("cg.dll", EntryPoint = "cgSetBufferData", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetBufferData(Buffer buffer, int size, [In] IntPtr data);

        [DllImport("cg.dll", EntryPoint = "cgSetBufferSubData", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetBufferSubData(Buffer buffer, int offset, int size, [In] IntPtr data);

        [DllImport("cg.dll", EntryPoint = "cgSetCompilerIncludeCallback", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetCompilerIncludeCallback(Context context, IncludeCallbackFuncDelegate func);

        [DllImport("cg.dll", EntryPoint = "cgSetCompilerIncludeFile", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetCompilerIncludeFile(Context context, string name, string filename);

        [DllImport("cg.dll", EntryPoint = "cgSetCompilerIncludeString", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetCompilerIncludeString(Context context, string name, string source);

        [DllImport("cg.dll", EntryPoint = "cgSetContextBehavior", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetContextBehavior(Context context, Behavior behavior);

        [DllImport("cg.dll", EntryPoint = "cgSetEffectName", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool SetEffectName(Effect effect, [In] string name);

        [DllImport("cg.dll", EntryPoint = "cgSetEffectParameterBuffer", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetEffectParameterBuffer(Parameter param, Buffer buffer);

        [DllImport("cg.dll", EntryPoint = "cgSetErrorCallback", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetErrorCallback(ErrorCallbackFuncDelegate func);

        [DllImport("cg.dll", EntryPoint = "cgSetErrorHandler", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetErrorHandler(ErrorHandlerFuncDelegate func, IntPtr data);

        [DllImport("cg.dll", EntryPoint = "cgSetFloatAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool SetFloatAnnotation(Annotation annotation, float value);

        [DllImport("cg.dll", EntryPoint = "cgSetFloatArrayStateAssignment", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool SetFloatArrayStateAssignment(StateAssignment stateassignment, [In] float[] vals);

        [DllImport("cg.dll", EntryPoint = "cgSetFloatStateAssignment", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool SetFloatStateAssignment(StateAssignment stateassignment, float value);

        [DllImport("cg.dll", EntryPoint = "cgSetIntAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool SetIntAnnotation(Annotation annotation, int value);

        [DllImport("cg.dll", EntryPoint = "cgSetIntArrayStateAssignment", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool SetIntArrayStateAssignment(StateAssignment stateassignment, [In] int[] vals);

        [DllImport("cg.dll", EntryPoint = "cgSetIntStateAssignment", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool SetIntStateAssignment(StateAssignment stateassignment, int value);

        [DllImport("cg.dll", EntryPoint = "cgSetLastListing", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetLastListing(Handle handle, string listing);

        [DllImport("cg.dll", EntryPoint = "cgSetLockingPolicy", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Enum SetLockingPolicy(Enum lockingPolicy);

        [DllImport("cg.dll", EntryPoint = "cgSetMatrixParameterdc", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetMatrixParameterdc(Parameter param, [In] double[] matrix);

        [DllImport("cg.dll", EntryPoint = "cgSetMatrixParameterdr", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetMatrixParameterdr(Parameter param, [In] double[] matrix);

        [DllImport("cg.dll", EntryPoint = "cgSetMatrixParameterfc", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetMatrixParameterfc(Parameter param, [In] float[] matrix);

        [DllImport("cg.dll", EntryPoint = "cgSetMatrixParameterfr", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetMatrixParameterfr(Parameter param, [In] Matrix matrix);

        [DllImport("cg.dll", EntryPoint = "cgSetMatrixParameteric", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetMatrixParameteric(Parameter param, [In] int[] matrix);

        [DllImport("cg.dll", EntryPoint = "cgSetMatrixParameterir", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetMatrixParameterir(Parameter param, [In] int[] matrix);

        [DllImport("cg.dll", EntryPoint = "cgSetMultiDimArraySize", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetMultiDimArraySize(Parameter param, int[] sizes);

        [DllImport("cg.dll", EntryPoint = "cgSetParameter1d", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter1d(Parameter param, double x);

        [DllImport("cg.dll", EntryPoint = "cgSetParameter1dv", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter1dv(Parameter param, [In] double[] v);

        [DllImport("cg.dll", EntryPoint = "cgSetParameter1f", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter1f(Parameter param, float x);

        [DllImport("cg.dll", EntryPoint = "cgSetParameter1fv", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter1fv(Parameter param, [In] float[] v);

        [DllImport("cg.dll", EntryPoint = "cgSetParameter1i", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter1i(Parameter param, int x);

        [DllImport("cg.dll", EntryPoint = "cgSetParameter1iv", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter1iv(Parameter param, [In] int[] v);

        [DllImport("cg.dll", EntryPoint = "cgSetParameter2d", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter2d(Parameter param, double x, double y);

        [DllImport("cg.dll", EntryPoint = "cgSetParameter2dv", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter2dv(Parameter param, [In] double[] v);

        [DllImport("cg.dll", EntryPoint = "cgSetParameter2f", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter2f(Parameter param, float x, float y);

        [DllImport("cg.dll", EntryPoint = "cgSetParameter2fv", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter2fv(Parameter param, [In] float[] v);

        [DllImport("cg.dll", EntryPoint = "cgSetParameter2i", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter2i(Parameter param, int x, int y);

        [DllImport("cg.dll", EntryPoint = "cgSetParameter2iv", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter2iv(Parameter param, [In] int[] v);

        [DllImport("cg.dll", EntryPoint = "cgSetParameter3d", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter3d(Parameter param, double x, double y, double z);

        [DllImport("cg.dll", EntryPoint = "cgSetParameter3dv", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter3dv(Parameter param, [In] double[] v);

        [DllImport("cg.dll", EntryPoint = "cgSetParameter3f", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter3f(Parameter param, float x, float y, float z);

        [DllImport("cg.dll", EntryPoint = "cgSetParameter3fv", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter3fv(Parameter param, [In] float[] v);

        [DllImport("cg.dll", EntryPoint = "cgSetParameter3i", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter3i(Parameter param, int x, int y, int z);

        [DllImport("cg.dll", EntryPoint = "cgSetParameter3iv", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter3iv(Parameter param, [In] int[] v);

        [DllImport("cg.dll", EntryPoint = "cgSetParameter4d", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter4d(Parameter param, double x, double y, double z, double w);

        [DllImport("cg.dll", EntryPoint = "cgSetParameter4dv", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter4dv(Parameter param, [In] double[] v);

        [DllImport("cg.dll", EntryPoint = "cgSetParameter4f", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter4f(Parameter param, float x, float y, float z, float w);

        [DllImport("cg.dll", EntryPoint = "cgSetParameter4fv", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter4fv(Parameter param, [In] float[] v);

        [DllImport("cg.dll", EntryPoint = "cgSetParameter4i", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter4i(Parameter param, int x, int y, int z, int w);

        [DllImport("cg.dll", EntryPoint = "cgSetParameter4iv", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameter4iv(Parameter param, [In] int[] v);

        [DllImport("cg.dll", EntryPoint = "cgSetParameterSemantic", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameterSemantic(Parameter param, [In] string semantic);

        [DllImport("cg.dll", EntryPoint = "cgSetParameterSettingMode", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameterSettingMode(Context context, Enum parameterSettingMode);

        [DllImport("cg.dll", EntryPoint = "cgSetParameterValuedc", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameterValuedc(Parameter param, int nelements, [In] double[] vals);

        [DllImport("cg.dll", EntryPoint = "cgSetParameterValuedr", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameterValuedr(Parameter param, int nelements, [In] double[] vals);

        [DllImport("cg.dll", EntryPoint = "cgSetParameterValuefc", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameterValuefc(Parameter param, int nelements, [In] float[] vals);

        [DllImport("cg.dll", EntryPoint = "cgSetParameterValuefr", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameterValuefr(Parameter param, int nelements, [In] float[] vals);

        [DllImport("cg.dll", EntryPoint = "cgSetParameterValueic", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameterValueic(Parameter param, int nelements, [In] int[] vals);

        [DllImport("cg.dll", EntryPoint = "cgSetParameterValueir", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameterValueir(Parameter param, int nelements, [In] int[] vals);

        [DllImport("cg.dll", EntryPoint = "cgSetParameterVariability", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetParameterVariability(Parameter param, Enum vary);

        [DllImport("cg.dll", EntryPoint = "cgSetPassProgramParameters", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetPassProgramParameters(Program prog);

        [DllImport("cg.dll", EntryPoint = "cgSetProgramBuffer", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetProgramBuffer(Program program, int bufferIndex, Buffer buffer);

        [DllImport("cg.dll", EntryPoint = "cgSetProgramProfile", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetProgramProfile(Program prog, Profile profile);

        [DllImport("cg.dll", EntryPoint = "cgSetProgramStateAssignment", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool SetProgramStateAssignment(StateAssignment stateassignment, Program program);

        [DllImport("cg.dll", EntryPoint = "cgSetSamplerState", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetSamplerState(Parameter param);

        [DllImport("cg.dll", EntryPoint = "cgSetSamplerStateAssignment", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool SetSamplerStateAssignment(StateAssignment stateassignment, Parameter parameter);

        [DllImport("cg.dll", EntryPoint = "cgSetSemanticCasePolicy", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Enum SetSemanticCasePolicy(Enum casePolicy);

        [DllImport("cg.dll", EntryPoint = "cgSetStateCallbacks", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetStateCallbacks(State state, StateCallbackDelegate set, StateCallbackDelegate reset,
            StateCallbackDelegate validate);

        [DllImport("cg.dll", EntryPoint = "cgSetStateLatestProfile", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetStateLatestProfile(State state, Profile profile);

        [DllImport("cg.dll", EntryPoint = "cgSetStringAnnotation", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool SetStringAnnotation(Annotation annotation, [In] string value);

        [DllImport("cg.dll", EntryPoint = "cgSetStringParameterValue", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void SetStringParameterValue(Parameter param, [In] string str);

        [DllImport("cg.dll", EntryPoint = "cgSetStringStateAssignment", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool SetStringStateAssignment(StateAssignment stateassignment, [In] string name);

        [DllImport("cg.dll", EntryPoint = "cgSetTextureStateAssignment", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool SetTextureStateAssignment(StateAssignment stateassignment, Parameter parameter);

        [DllImport("cg.dll", EntryPoint = "cgUnmapBuffer", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void UnmapBuffer(Buffer buffer);

        [DllImport("cg.dll", EntryPoint = "cgUpdatePassParameters", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void UpdatePassParameters(Pass pass);

        [DllImport("cg.dll", EntryPoint = "cgUpdateProgramParameters", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern void UpdateProgramParameters(Program program);

        [DllImport("cg.dll", EntryPoint = "cgValidateTechnique", CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity]
        public static extern Bool ValidateTechnique(Technique technique);

        #endregion Cg

        #region Dx
        public class DX11
        {
            [DllImport("cgD3D11.dll", EntryPoint = "cgD3D11GetDevice", CallingConvention = CallingConvention.ThisCall)]
            public static extern IntPtr GetDevice(Context context);

            [DllImport("cgD3D11.dll", EntryPoint = "cgD3D11SetDevice", CallingConvention = CallingConvention.ThisCall)]
            public static extern int SetDevice(Context context, IntPtr pDevice);

            [DllImport("cgD3D11.dll", EntryPoint = "cgD3D11SetTextureParameter", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetTextureParameter(Parameter parameter, IntPtr pTexture);


            [DllImport("cgD3D11.dll", EntryPoint = "cgD3D11SetSamplerStateParameter",
                CallingConvention = CallingConvention.ThisCall)]
            public static extern void SetSamplerStateParameter(Parameter parameter, IntPtr pSamplerState);

            [DllImport("cgD3D11.dll", EntryPoint = "cgD3D11SetTextureSamplerStateParameter",
                CallingConvention = CallingConvention.ThisCall)]
            public static extern void SetTextureSamplerStateParameter(Parameter parameter, IntPtr pTexture,
                IntPtr pSamplerState);

            [DllImport("cgD3D11.dll", EntryPoint = "cgD3D11LoadProgram", CallingConvention = CallingConvention.ThisCall)
            ]
            public static extern int LoadProgram(Program Program, uint Flags);

            [DllImport("cgD3D11.dll", EntryPoint = "cgD3D11GetCompiledProgram",
                CallingConvention = CallingConvention.ThisCall)]
            public static extern IntPtr GetCompiledProgram(Program Program);

            [DllImport("cgD3D11.dll", EntryPoint = "cgD3D11GetProgramErrors",
                CallingConvention = CallingConvention.ThisCall
                )]
            public static extern IntPtr GetProgramErrors(Program Program);

            [DllImport("cgD3D11.dll", EntryPoint = "cgD3D11IsProgramLoaded",
                CallingConvention = CallingConvention.ThisCall)
            ]
            public static extern Bool IsProgramLoaded(Program Program);

            [DllImport("cgD3D11.dll", EntryPoint = "cgD3D11BindProgram", CallingConvention = CallingConvention.ThisCall)
            ]
            public static extern int BindProgram(Program Program);

            [DllImport("cgD3D11.dll", EntryPoint = "cgD3D11UnloadProgram",
                CallingConvention = CallingConvention.ThisCall)]
            public static extern void UnloadProgram(Program Program);

            [DllImport("cgD3D11.dll", EntryPoint = "cgD3D11GetBufferByIndex",
                CallingConvention = CallingConvention.ThisCall
                )]
            public static extern IntPtr GetBufferByIndex(Program Program, uint Index);

            [DllImport("cgD3D11.dll", EntryPoint = "cgD3D11RegisterStates",
                CallingConvention = CallingConvention.ThisCall)]
            public static extern void RegisterStates(Context Context);

            [DllImport("cgD3D11.dll", EntryPoint = "cgD3D11SetManageTextureParameters",
                CallingConvention = CallingConvention.ThisCall)]
            public static extern void SetManageTextureParameters(Context Context, Bool Flag);

            [DllImport("cgD3D11.dll", EntryPoint = "cgD3D11GetManageTextureParameters",
                CallingConvention = CallingConvention.ThisCall)]
            public static extern Bool GetManageTextureParameters(Context Context);

            [DllImport("cgD3D11.dll", EntryPoint = "cgD3D11GetIASignatureByPass",
                CallingConvention = CallingConvention.ThisCall)]
            public static extern IntPtr GetIASignatureByPass(Pass Pass);

            [DllImport("cgD3D11.dll", EntryPoint = "cgD3D11GetLatestVertexProfile")]
            public static extern Profile GetLatestVertexProfile();

            [DllImport("cgD3D11.dll", EntryPoint = "cgD3D11GetLatestGeometryProfile")]
            public static extern Profile GetLatestGeometryProfile();

            [DllImport("cgD3D11.dll", EntryPoint = "cgD3D11GetLatestPixelProfile")]
            public static extern Profile GetLatestPixelProfile();

            [DllImport("cgD3D11.dll", EntryPoint = "cgD3D11GetLatestHullProfile")]
            public static extern Profile GetLatestHullProfile();

            [DllImport("cgD3D11.dll", EntryPoint = "cgD3D11GetLatestDomainProfile")]
            public static extern Profile GetLatestDomainProfile();

            [DllImport("cgD3D11.dll", EntryPoint = "cgD3D11IsProfileSupported",
                CallingConvention = CallingConvention.ThisCall)]
            public static extern Bool IsProfileSupported(Profile profile);

            [DllImport("cgD3D11.dll", EntryPoint = "cgD3D11TypeToSize", CallingConvention = CallingConvention.ThisCall)]
            public static extern int TypeToSize(Type type);

            [DllImport("cgD3D11.dll", EntryPoint = "cgD3D11GetLastError")]
            public static extern int GetLastError();

            [DllImport("cgD3D11.dll", EntryPoint = "cgD3D11GetOptimalOptions",
                CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
            public static extern IntPtr GetOptimalOptions(Profile profile);

            [DllImport("cgD3D11.dll", EntryPoint = "cgD3D11TranslateCGerror",
                CallingConvention = CallingConvention.ThisCall,
                CharSet = CharSet.Ansi)]
            public static extern string TranslateCGerror(Error error);

            [DllImport("cgD3D11.dll", EntryPoint = "cgD3D11TranslateHRESULT",
                CallingConvention = CallingConvention.ThisCall,
                CharSet = CharSet.Ansi)]
            public static extern IntPtr TranslateHRESULT(int hr);

            [DllImport("cgD3D11.dll", EntryPoint = "cgD3D11UnbindProgram",
                CallingConvention = CallingConvention.ThisCall)]
            public static extern void UnbindProgram(Program program);

            [DllImport("cgD3D11.dll", EntryPoint = "cgD3D11CreateBuffer", CallingConvention = CallingConvention.ThisCall
                )]
            public static extern Buffer CreateBuffer(Context context, int size, IntPtr data, ResourceUsage bufferUsage);

            [DllImport("cgD3D11.dll", EntryPoint = "cgD3D11CreateBufferFromObject",
                CallingConvention = CallingConvention.ThisCall)]
            public static extern Buffer CreateBufferFromObject(Context context, IntPtr obj, Bool manageObject);

            [DllImport("cgD3D11.dll", EntryPoint = "cgD3D11GetBufferObject",
                CallingConvention = CallingConvention.ThisCall)
            ]
            public static extern IntPtr GetBufferObject(Buffer buffer);
        }

        #endregion Dx

        #region GL

        public class GL
        {
            [DllImport("cgGL.dll", EntryPoint = "cgGLBindProgram", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void BindProgram(Program program);

            [DllImport("cgGL.dll", EntryPoint = "cgGLCreateBuffer", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern Buffer CreateBuffer(Context context, int size, IntPtr data, int bufferUsage);

            [DllImport("cgGL.dll", EntryPoint = "cgGLDisableClientState", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void DisableClientState(Parameter param);

            [DllImport("cgGL.dll", EntryPoint = "cgGLDisableProfile", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void DisableProfile(Profile profile);

            [DllImport("cgGL.dll", EntryPoint = "cgGLDisableProgramProfiles",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void DisableProgramProfiles(Program program);

            [DllImport("cgGL.dll", EntryPoint = "cgGLDisableTextureParameter",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void DisableTextureParameter(Parameter param);

            [DllImport("cgGL.dll", EntryPoint = "cgGLEnableClientState", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void EnableClientState(Parameter param);

            [DllImport("cgGL.dll", EntryPoint = "cgGLEnableProfile", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void EnableProfile(Profile profile);

            [DllImport("cgGL.dll", EntryPoint = "cgGLEnableProgramProfiles", CallingConvention = CallingConvention.Cdecl
                ), SuppressUnmanagedCodeSecurity]
            public static extern void EnableProgramProfiles(Program program);

            [DllImport("cgGL.dll", EntryPoint = "cgGLEnableTextureParameter",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void EnableTextureParameter(Parameter param);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetBufferObject", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern int GetBufferObject(Buffer buffer);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetLatestProfile", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern Profile GetLatestProfile(GlEnum profileType);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetManageTextureParameters",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern int GetManageTextureParameters(Context context);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetMatrixParameterArraydc",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetMatrixParameterArraydc(Parameter param, long offset, long nelements,
                [Out] double* v);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetMatrixParameterArraydc",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void GetMatrixParameterArraydc(Parameter param, long offset, long nelements,
                [Out] double[] v);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetMatrixParameterArraydc",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void GetMatrixParameterArraydc(Parameter param, long offset, long nelements,
                [Out] IntPtr v);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetMatrixParameterArraydr",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetMatrixParameterArraydr(Parameter param, long offset, long nelements,
                [Out] double* v);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetMatrixParameterArraydr",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void GetMatrixParameterArraydr(Parameter param, long offset, long nelements,
                [Out] double[] v);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetMatrixParameterArraydr",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void GetMatrixParameterArraydr(Parameter param, long offset, long nelements,
                [Out] IntPtr v);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetMatrixParameterArrayfc",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetMatrixParameterArrayfc(Parameter param, long offset, long nelements,
                [Out] float* v);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetMatrixParameterArrayfc",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void GetMatrixParameterArrayfc(Parameter param, long offset, long nelements,
                [Out] float[] v);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetMatrixParameterArrayfc",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void GetMatrixParameterArrayfc(Parameter param, long offset, long nelements,
                [Out] IntPtr v);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetMatrixParameterArrayfr",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetMatrixParameterArrayfr(Parameter param, long offset, long nelements,
                [Out] float* v);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetMatrixParameterArrayfr",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void GetMatrixParameterArrayfr(Parameter param, long offset, long nelements,
                [Out] float[] v);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetMatrixParameterArrayfr",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void GetMatrixParameterArrayfr(Parameter param, long offset, long nelements,
                [Out] IntPtr v);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetMatrixParameterdc", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetMatrixParameterdc(Parameter param, [In] double* matrix);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetMatrixParameterdc", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetMatrixParameterdc(Parameter param, [In] double[] matrix);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetMatrixParameterdc", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetMatrixParameterdc(Parameter param, [In] IntPtr matrix);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetMatrixParameterdr", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetMatrixParameterdr(Parameter param, [In] double* matrix);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetMatrixParameterdr", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetMatrixParameterdr(Parameter param, [In] double[] matrix);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetMatrixParameterdr", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetMatrixParameterdr(Parameter param, [In] IntPtr matrix);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetMatrixParameterfc", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetMatrixParameterfc(Parameter param, [In] float* matrix);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetMatrixParameterfc", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetMatrixParameterfc(Parameter param, [In] float[] matrix);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetMatrixParameterfc", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetMatrixParameterfc(Parameter param, [In] IntPtr matrix);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetMatrixParameterfr", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetMatrixParameterfr(Parameter param, [In] float* matrix);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetMatrixParameterfr", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetMatrixParameterfr(Parameter param, [In] float[] matrix);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetMatrixParameterfr", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetMatrixParameterfr(Parameter param, [In] IntPtr matrix);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetOptimalOptions", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern IntPtr GetOptimalOptions(Profile profile);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameter1d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetParameter1d(Parameter param, [Out] double* values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameter1d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameter1d(Parameter param, [Out] double[] values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameter1d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameter1d(Parameter param, [Out] IntPtr values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameter1f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetParameter1f(Parameter param, [Out] float* values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameter1f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameter1f(Parameter param, [Out] float[] values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameter1f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameter1f(Parameter param, [Out] IntPtr values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameter2d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetParameter2d(Parameter param, [Out] double* values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameter2d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameter2d(Parameter param, [Out] double[] values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameter2d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameter2d(Parameter param, [Out] IntPtr values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameter2f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetParameter2f(Parameter param, [Out] float* values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameter2f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameter2f(Parameter param, [Out] float[] values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameter2f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameter2f(Parameter param, [Out] IntPtr values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameter3d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetParameter3d(Parameter param, [Out] double* values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameter3d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameter3d(Parameter param, [Out] double[] values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameter3d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameter3d(Parameter param, [Out] IntPtr values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameter3f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetParameter3f(Parameter param, [Out] float* values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameter3f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameter3f(Parameter param, [Out] float[] values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameter3f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameter3f(Parameter param, [Out] IntPtr values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameter4d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetParameter4d(Parameter param, [Out] double* values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameter4d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameter4d(Parameter param, [Out] double[] values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameter4d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameter4d(Parameter param, [Out] IntPtr values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameter4f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetParameter4f(Parameter param, [Out] float* values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameter4f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameter4f(Parameter param, [Out] float[] values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameter4f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameter4f(Parameter param, [Out] IntPtr values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameterArray1d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetParameterArray1d(Parameter param, long offset, long nelements,
                [Out] double* values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameterArray1d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameterArray1d(Parameter param, long offset, long nelements,
                [Out] double[] values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameterArray1d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameterArray1d(Parameter param, long offset, long nelements,
                [Out] IntPtr values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameterArray1f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetParameterArray1f(Parameter param, long offset, long nelements,
                [Out] float* values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameterArray1f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameterArray1f(Parameter param, long offset, long nelements,
                [Out] float[] values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameterArray1f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameterArray1f(Parameter param, long offset, long nelements,
                [Out] IntPtr values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameterArray2d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetParameterArray2d(Parameter param, long offset, long nelements,
                [Out] double* values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameterArray2d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameterArray2d(Parameter param, long offset, long nelements,
                [Out] double[] values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameterArray2d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameterArray2d(Parameter param, long offset, long nelements,
                [Out] IntPtr values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameterArray2f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetParameterArray2f(Parameter param, long offset, long nelements,
                [Out] float* values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameterArray2f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameterArray2f(Parameter param, long offset, long nelements,
                [Out] float[] values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameterArray2f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameterArray2f(Parameter param, long offset, long nelements,
                [Out] IntPtr values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameterArray3d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetParameterArray3d(Parameter param, long offset, long nelements,
                [Out] double* values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameterArray3d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameterArray3d(Parameter param, long offset, long nelements,
                [Out] double[] values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameterArray3d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameterArray3d(Parameter param, long offset, long nelements,
                [Out] IntPtr values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameterArray3f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetParameterArray3f(Parameter param, long offset, long nelements,
                [Out] float* values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameterArray3f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameterArray3f(Parameter param, long offset, long nelements,
                [Out] float[] values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameterArray3f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameterArray3f(Parameter param, long offset, long nelements,
                [Out] IntPtr values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameterArray4d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetParameterArray4d(Parameter param, long offset, long nelements,
                [Out] double* values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameterArray4d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameterArray4d(Parameter param, long offset, long nelements,
                [Out] double[] values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameterArray4d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameterArray4d(Parameter param, long offset, long nelements,
                [Out] IntPtr values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameterArray4f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void GetParameterArray4f(Parameter param, long offset, long nelements,
                [Out] float* values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameterArray4f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameterArray4f(Parameter param, long offset, long nelements,
                [Out] float[] values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetParameterArray4f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void GetParameterArray4f(Parameter param, long offset, long nelements,
                [Out] IntPtr values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetProgramID", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern int GetProgramID(Program program);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetDebugMode", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetDebugMode(Bool debug);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetManageTextureParameters",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void SetManageTextureParameters(Context context, bool flag);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetTextureEnum", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern int GetTextureEnum(Parameter param);

            [DllImport("cgGL.dll", EntryPoint = "cgGLGetTextureParameter", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern int GetTextureParameter(Parameter param);

            [DllImport("cgGL.dll", EntryPoint = "cgGLIsProfileSupported", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern Bool IsProfileSupported(Profile profile);

            [DllImport("cgGL.dll", EntryPoint = "cgGLIsProgramLoaded", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern Bool IsProgramLoaded(Program program);

            [DllImport("cgGL.dll", EntryPoint = "cgGLLoadProgram", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void LoadProgram(Program program);

            [DllImport("cgGL.dll", EntryPoint = "cgGLRegisterStates", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void RegisterStates(Context context);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetMatrixParameterArraydc",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetMatrixParameterArraydc(Parameter param, long offset, long nelements,
                [In] double* v);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetMatrixParameterArraydc",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void SetMatrixParameterArraydc(Parameter param, long offset, long nelements,
                [In] double[] v);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetMatrixParameterArraydc",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void SetMatrixParameterArraydc(Parameter param, long offset, long nelements,
                [In] IntPtr v);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetMatrixParameterArraydr",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetMatrixParameterArraydr(Parameter param, long offset, long nelements,
                [In] double* v);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetMatrixParameterArraydr",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void SetMatrixParameterArraydr(Parameter param, long offset, long nelements,
                [In] double[] v);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetMatrixParameterArraydr",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void SetMatrixParameterArraydr(Parameter param, long offset, long nelements,
                [In] IntPtr v);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetMatrixParameterArrayfc",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetMatrixParameterArrayfc(Parameter param, long offset, long nelements,
                [In] float* v);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetMatrixParameterArrayfc",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void SetMatrixParameterArrayfc(Parameter param, long offset, long nelements,
                [In] float[] v);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetMatrixParameterArrayfc",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void SetMatrixParameterArrayfc(Parameter param, long offset, long nelements,
                [In] IntPtr v);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetMatrixParameterArrayfr",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetMatrixParameterArrayfr(Parameter param, long offset, long nelements,
                [In] float* v);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetMatrixParameterArrayfr",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void SetMatrixParameterArrayfr(Parameter param, long offset, long nelements,
                [In] float[] v);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetMatrixParameterArrayfr",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void SetMatrixParameterArrayfr(Parameter param, long offset, long nelements,
                [In] IntPtr v);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetMatrixParameterdc", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetMatrixParameterdc(Parameter param, [In] double* matrix);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetMatrixParameterdc", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetMatrixParameterdc(Parameter param, [In] double[] matrix);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetMatrixParameterdc", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetMatrixParameterdc(Parameter param, [In] IntPtr matrix);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetMatrixParameterdr", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetMatrixParameterdr(Parameter param, [In] double* matrix);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetMatrixParameterdr", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetMatrixParameterdr(Parameter param, [In] double[] matrix);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetMatrixParameterdr", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetMatrixParameterdr(Parameter param, [In] IntPtr matrix);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetMatrixParameterfc", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetMatrixParameterfc(Parameter param, [In] float* matrix);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetMatrixParameterfc", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetMatrixParameterfc(Parameter param, [In] float[] matrix);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetMatrixParameterfc", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetMatrixParameterfc(Parameter param, [In] IntPtr matrix);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetMatrixParameterfr", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetMatrixParameterfr(Parameter param, [In] float* matrix);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetMatrixParameterfr", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetMatrixParameterfr(Parameter param, [In] Matrix matrix);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetMatrixParameterfr", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetMatrixParameterfr(Parameter param, [In] IntPtr matrix);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetOptimalOptions", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetOptimalOptions(Profile profile);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameter1d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter1d(Parameter param, double x);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameter1dv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter1dv(Parameter param, [In] double[] values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameter1dv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter1dv(Parameter param, [In] IntPtr values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameter1dv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetParameter1dv(Parameter param, [In] double* values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameter1f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter1f(Parameter param, float x);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameter1fv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetParameter1fv(Parameter param, [In] float* values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameter1fv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter1fv(Parameter param, [In] float[] values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameter1fv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter1fv(Parameter param, [In] IntPtr values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameter2d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter2d(Parameter param, double x, double y);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameter2dv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetParameter2dv(Parameter param, [In] double* values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameter2dv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter2dv(Parameter param, [In] double[] values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameter2dv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter2dv(Parameter param, [In] IntPtr values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameter2f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter2f(Parameter param, float x, float y);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameter2fv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetParameter2fv(Parameter param, [In] float* values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameter2fv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter2fv(Parameter param, [In] float[] values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameter2fv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter2fv(Parameter param, [In] IntPtr values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameter3d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter3d(Parameter param, double x, double y, double z);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameter3dv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter3dv(Parameter param, [In] double[] values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameter3dv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter3dv(Parameter param, [In] IntPtr values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameter3dv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetParameter3dv(Parameter param, [In] double* values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameter3f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter3f(Parameter param, float x, float y, float z);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameter3fv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetParameter3fv(Parameter param, [In] float* values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameter3fv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter3fv(Parameter param, [In] float[] values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameter3fv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter3fv(Parameter param, [In] IntPtr values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameter4d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter4d(Parameter param, double x, double y, double z, double w);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameter4dv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetParameter4dv(Parameter param, [In] double* values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameter4dv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter4dv(Parameter param, [In] double[] values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameter4dv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter4dv(Parameter param, [In] IntPtr values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameter4f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter4f(Parameter param, float x, float y, float z, float w);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameter4fv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter4fv(Parameter param, [In] float[] values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameter4fv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetParameter4fv(Parameter param, [In] float* values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameter4fv", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameter4fv(Parameter param, [In] IntPtr values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameterArray1d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetParameterArray1d(Parameter param, long offset, long nelements,
                [In] double* values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameterArray1d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameterArray1d(Parameter param, long offset, long nelements,
                [In] double[] values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameterArray1d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameterArray1d(Parameter param, long offset, long nelements,
                [In] IntPtr values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameterArray1f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetParameterArray1f(Parameter param, long offset, long nelements,
                [In] float* values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameterArray1f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameterArray1f(Parameter param, long offset, long nelements,
                [In] float[] values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameterArray1f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameterArray1f(Parameter param, long offset, long nelements,
                [In] IntPtr values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameterArray2d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetParameterArray2d(Parameter param, long offset, long nelements,
                [In] double* values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameterArray2d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameterArray2d(Parameter param, long offset, long nelements,
                [In] double[] values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameterArray2d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameterArray2d(Parameter param, long offset, long nelements,
                [In] IntPtr values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameterArray2f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetParameterArray2f(Parameter param, long offset, long nelements,
                [In] float* values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameterArray2f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameterArray2f(Parameter param, long offset, long nelements,
                [In] float[] values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameterArray2f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameterArray2f(Parameter param, long offset, long nelements,
                [In] IntPtr values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameterArray3d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetParameterArray3d(Parameter param, long offset, long nelements,
                [In] double* values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameterArray3d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameterArray3d(Parameter param, long offset, long nelements,
                [In] double[] values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameterArray3d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameterArray3d(Parameter param, long offset, long nelements,
                [In] IntPtr values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameterArray3f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetParameterArray3f(Parameter param, long offset, long nelements,
                [In] float* values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameterArray3f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameterArray3f(Parameter param, long offset, long nelements,
                [In] float[] values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameterArray3f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameterArray3f(Parameter param, long offset, long nelements,
                [In] IntPtr values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameterArray4d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetParameterArray4d(Parameter param, long offset, long nelements,
                [In] double* values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameterArray4d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameterArray4d(Parameter param, long offset, long nelements,
                [In] double[] values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameterArray4d", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameterArray4d(Parameter param, long offset, long nelements,
                [In] IntPtr values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameterArray4f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameterArray4f(Parameter param, long offset, long nelements,
                [In] IntPtr values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameterArray4f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetParameterArray4f(Parameter param, long offset, long nelements,
                [In] float* values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameterArray4f", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameterArray4f(Parameter param, long offset, long nelements,
                [In] float[] values);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameterPointer", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern unsafe void SetParameterPointer(Parameter param, int fsize, int type, int stride,
                [In] void* pointer);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetParameterPointer", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetParameterPointer(Parameter param, int fsize, int type, int stride,
                [In] IntPtr pointer);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetStateMatrixParameter",
                CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public static extern void SetStateMatrixParameter(Parameter param, int matrix, int transform);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetTextureParameter", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetTextureParameter(Parameter param, int texobj);

            [DllImport("cgGL.dll", EntryPoint = "cgGLSetupSampler", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void SetupSampler(Parameter param, int texobj);

            [DllImport("cgGL.dll", EntryPoint = "cgGLUnbindProgram", CallingConvention = CallingConvention.Cdecl),
             SuppressUnmanagedCodeSecurity]
            public static extern void UnbindProgram(Program profile); //TODO ���� cgGLUnbindProgram Profile profile)

            [DllImport("cgGL.dll", EntryPoint = "cgGLUnloadProgram", CallingConvention = CallingConvention.Cdecl),
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