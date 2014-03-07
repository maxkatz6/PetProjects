using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Ormeli.CG
{
    public class CgImports
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate CGbool StateCallbackDelegate(CGstateassignment CGstateassignment);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ErrorCallbackFuncDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ErrorHandlerFuncDelegate(CGcontext context, CGerror error, IntPtr appdata);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void IncludeCallbackFuncDelegate(CGcontext context, [In]string filename);

        #region Cg

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgAddStateEnumerant(CGstate state, [In] string name, int value);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbool cgCallStateResetCallback(CGstateassignment stateassignment);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbool cgCallStateSetCallback(CGstateassignment stateassignment);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbool cgCallStateValidateCallback(CGstateassignment stateassignment);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGprogram cgCombinePrograms(int n, [In] CGprogram[] exeList);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGprogram cgCombinePrograms2([In] CGprogram program1, [In] CGprogram program2);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGprogram cgCombinePrograms3([In] CGprogram program1, [In] CGprogram program2, [In] CGprogram program3);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGprogram cgCombinePrograms4([In] CGprogram program1, [In] CGprogram program2, [In] CGprogram program3, [In] CGprogram program4);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGprogram cgCombinePrograms5([In] CGprogram program1, [In] CGprogram program2, [In] CGprogram program3, [In] CGprogram program4, [In] CGprogram program5);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgCompileProgram(CGprogram program);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgConnectParameter(CGparameter from, CGparameter to);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGeffect cgCopyEffect(CGeffect effect);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGprogram cgCopyProgram(CGprogram program);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGstate cgCreateArraySamplerState(CGcontext context, [In] string name, CGtype type, int nelems);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGstate cgCreateArrayState(CGcontext context, [In] string name, CGtype type, int nelems);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbuffer cgCreateBuffer(CGcontext context, int size, [In] IntPtr data, CGbufferusage bufferUsage);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGcontext cgCreateContext();

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGeffect cgCreateEffect(CGcontext context, [In] string code, [In] string[] args);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGannotation cgCreateEffectAnnotation(CGeffect effect, [In]string name, CGtype type);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGeffect cgCreateEffectFromFile(CGcontext context, [In] string filename, [In] string[] args);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGparameter cgCreateEffectParameter(CGeffect effect, [In]string name, CGtype type);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGparameter cgCreateEffectParameterArray(CGeffect effect, [In] string name, CGtype type, int length);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGparameter cgCreateEffectParameterMultiDimArray(CGeffect effect, [In]string name, CGtype type, int dim, [In]int[] lengths);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGobj cgCreateObj(CGcontext context, CGenum program_type, [In] string source, CGprofile profile, [In] string[] args);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGobj cgCreateObjFromFile(CGcontext context, CGenum program_type, [In] string source_file, CGprofile profile, [In] string[] args);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGparameter cgCreateParameter(CGcontext context, CGtype type);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGannotation cgCreateParameterAnnotation(CGparameter parameter, [In]string name, CGtype type);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGparameter cgCreateParameterArray(CGcontext context, CGtype type, int length);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGparameter cgCreateParameterMultiDimArray(CGcontext context, CGtype type, int dim, [In] int[] lengths);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGpass cgCreatePass(CGtechnique technique, [In] string name);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGannotation cgCreatePassAnnotation(CGpass pass, [In]string name, CGtype type);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGprogram cgCreateProgram(CGcontext context, CGenum program_type, [In] string program, CGprofile profile, [In] string entry, [In]string[] args);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGannotation cgCreateProgramAnnotation(CGprogram program, [In]string name, CGtype type);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern IntPtr cgCreateProgramFromEffect(CGeffect effect, CGprofile profile, [In] string entry, [In] string[] args);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGprogram cgCreateProgramFromFile(CGcontext context, CGenum program_type, [In] string program_file, CGprofile profile, [In] string entry, [In] string[] args);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGstate cgCreateSamplerState(CGcontext context, [In] string name, CGtype type);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGstateassignment cgCreateSamplerStateAssignment(CGparameter parameter, CGstate state);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGstate cgCreateState(CGcontext context, [In] string name, CGtype type);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGstateassignment cgCreateStateAssignment(CGpass pass, CGstate state);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGstateassignment cgCreateStateAssignmentIndex(CGpass pass, CGstate state, int index);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGtechnique cgCreateTechnique(CGeffect effect, [In] string name);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGannotation cgCreateTechniqueAnnotation(CGtechnique technique, [In]string name, CGtype type);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgDestroyBuffer(CGbuffer buffer);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgDestroyContext(CGcontext context);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgDestroyEffect(CGeffect effect);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgDestroyObj(CGobj obj);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgDestroyParameter(CGparameter param);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgDestroyProgram(CGprogram program);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgDisconnectParameter(CGparameter param);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgEvaluateProgram(CGprogram program, [In][Out] float[] values, int ncomps, int nx, int ny, int nz);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern IntPtr cgGetAnnotationName(CGannotation annotation);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGtype cgGetAnnotationType(CGannotation annotation);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int cgGetArrayDimension(CGparameter param);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGparameter cgGetArrayParameter(CGparameter aparam, int index);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int cgGetArraySize(CGparameter param, int dimension);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int cgGetArrayTotalSize(CGparameter param);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGtype cgGetArrayType(CGparameter param);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGenum cgGetAutoCompile(CGcontext context);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbehavior cgGetBehavior(string behaviorString);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern IntPtr cgGetBehaviorString(CGbehavior behavior);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern IntPtr cgGetBoolAnnotationValues(CGannotation annotation, out int nvalues);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbool[] cgGetBoolStateAssignmentValues(CGstateassignment stateassignment, int[] nVals);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int[] cgGetBooleanAnnotationValues(CGannotation annotation, out int nvalues);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int cgGetBufferSize(CGbuffer buffer);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern IncludeCallbackFuncDelegate cgGetCompilerIncludeCallback(CGcontext context);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGparameter cgGetConnectedParameter(CGparameter param);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGparameter cgGetConnectedStateAssignmentParameter(CGstateassignment stateassignment);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGparameter cgGetConnectedToParameter(CGparameter param, int index);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbehavior cgGetContextBehavior(CGcontext context);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGparameter cgGetDependentAnnotationParameter(CGannotation annotation, int index);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGparameter cgGetDependentProgramArrayStateAssignmentParameter(CGstateassignment sa, int index);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGparameter cgGetDependentStateAssignmentParameter(CGstateassignment stateassignment, int index);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGdomain cgGetDomain(string domainString);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern IntPtr cgGetDomainString(CGdomain domain);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGcontext cgGetEffectContext(CGeffect effect);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern string cgGetEffectName(CGeffect effect);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbuffer cgGetEffectParameterBuffer(CGparameter param);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGparameter cgGetEffectParameterBySemantic(CGeffect effect, [In] string name);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGenum cgGetEnum([In] string enum_string);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern IntPtr cgGetEnumString(CGenum en);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGerror cgGetError();

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern ErrorCallbackFuncDelegate cgGetErrorCallback();

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern ErrorHandlerFuncDelegate cgGetErrorHandler(out IntPtr data);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern IntPtr cgGetErrorString(CGerror error);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGparameter cgGetFirstDependentParameter(CGparameter param);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGeffect cgGetFirstEffect(CGcontext context);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGannotation cgGetFirstEffectAnnotation(CGeffect effect);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGparameter cgGetFirstEffectParameter(CGeffect effect);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGerror cgGetFirstError();

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGparameter cgGetFirstLeafEffectParameter(CGeffect effect);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGparameter cgGetFirstLeafParameter(CGprogram program, CGenum name_space);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGparameter cgGetFirstParameter(CGprogram prog, CGenum name_space);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGannotation cgGetFirstParameterAnnotation(CGparameter param);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGpass cgGetFirstPass(CGtechnique technique);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGannotation cgGetFirstPassAnnotation(CGpass pass);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGprogram cgGetFirstProgram(CGcontext context);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGannotation cgGetFirstProgramAnnotation(CGprogram prog);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGstate cgGetFirstSamplerState(CGcontext context);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGstateassignment cgGetFirstSamplerStateAssignment(CGparameter param);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGstate cgGetFirstState(CGcontext context);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGstateassignment cgGetFirstStateAssignment(CGpass pass);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGparameter cgGetFirstStructParameter(CGparameter param);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGtechnique cgGetFirstTechnique(CGeffect effect);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGannotation cgGetFirstTechniqueAnnotation(CGtechnique technique);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern float[] cgGetFloatAnnotationValues(CGannotation annotation, out int nvalues);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern float[] cgGetFloatStateAssignmentValues(CGstateassignment stateassignment, int[] nvalues);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int[] cgGetIntAnnotationValues(CGannotation annotation, out int nvalues);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int[] cgGetIntStateAssignmentValues(CGstateassignment stateassignment, int[] nvalues);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern IntPtr cgGetLastErrorString(out CGerror error);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern IntPtr cgGetLastListing(CGcontext context);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGenum cgGetLockingPolicy();

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGenum cgGetMatrixParameterOrder(CGparameter param);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgGetMatrixParameterdc(CGparameter param, [In] double[] matrix);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgGetMatrixParameterdr(CGparameter param, [In] double[] matrix);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgGetMatrixParameterfc(CGparameter param, [In] float[] matrix);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgGetMatrixParameterfr(CGparameter param, [In] float[] matrix);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgGetMatrixParameteric(CGparameter param, [In] int[] matrix);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgGetMatrixParameterir(CGparameter param, [In] int[] matrix);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgGetMatrixSize(CGtype type, out int nrows, out int ncols);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGeffect cgGetNamedEffect(CGcontext context, [In] string name);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGannotation cgGetNamedEffectAnnotation(CGeffect effect, [In] string name);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGparameter cgGetNamedEffectParameter(CGeffect effect, [In] string name);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGparameter cgGetNamedParameter(CGprogram program, [In] string parameter);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGannotation cgGetNamedParameterAnnotation(CGparameter param, [In] string name);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGpass cgGetNamedPass(CGtechnique technique, [In] string name);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGannotation cgGetNamedPassAnnotation(CGpass pass, [In] string name);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGannotation cgGetNamedProgramAnnotation(CGprogram prog, [In] string name);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGparameter cgGetNamedProgramParameter(CGprogram program, CGenum name_space, [In] string name);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGstate cgGetNamedSamplerState(CGcontext context, [In] string name);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGstateassignment cgGetNamedSamplerStateAssignment(CGparameter param, [In]string name);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGstate cgGetNamedState(CGcontext context, [In] string name);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGstateassignment cgGetNamedStateAssignment(CGpass pass, [In]string name);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGparameter cgGetNamedStructParameter(CGparameter param, [In] string name);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGparameter cgGetNamedSubParameter(CGparameter param, [In] string name);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGtechnique cgGetNamedTechnique(CGeffect effect, [In] string name);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGannotation cgGetNamedTechniqueAnnotation(CGtechnique technique, [In] string name);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGtype cgGetNamedUserType(CGhandle handle, [In] string name);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGannotation cgGetNextAnnotation(CGannotation annotation);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGeffect cgGetNextEffect(CGeffect effect);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGparameter cgGetNextLeafParameter(CGparameter current);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGparameter cgGetNextParameter(CGparameter current);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGpass cgGetNextPass(CGpass pass);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGprogram cgGetNextProgram(CGprogram program);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGstate cgGetNextState(CGstate state);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGstateassignment cgGetNextStateAssignment(CGstateassignment stateassignment);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGtechnique cgGetNextTechnique(CGtechnique technique);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int cgGetNumConnectedToParameters(CGparameter param);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int cgGetNumDependentAnnotationParameters(CGannotation annotation);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int cgGetNumDependentProgramArrayStateAssignmentParameters(CGstateassignment sa);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int cgGetNumDependentStateAssignmentParameters(CGstateassignment stateassignment);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int cgGetNumParentTypes(CGtype type);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int cgGetNumProgramDomains(CGprogram program);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int cgGetNumStateEnumerants(CGstate state);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int cgGetNumSupportedProfiles();

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int cgGetNumUserTypes(CGhandle handle);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGresource cgGetParameterBaseResource(CGparameter param);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGtype cgGetParameterBaseType(CGparameter param);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int cgGetParameterBufferIndex(CGparameter parameter);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int cgGetParameterBufferOffset(CGparameter parameter);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGparameterclass cgGetParameterClass(CGparameter param);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGparameterclass cgGetParameterClassEnum(string pString);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern IntPtr cgGetParameterClassString(CGparameterclass pc);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int cgGetParameterColumns(CGparameter param);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGcontext cgGetParameterContext(CGparameter param);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int cgGetParameterDefaultValuedc(CGparameter param, int nelements, [Out]double[] vals);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int cgGetParameterDefaultValuedr(CGparameter param, int nelements, [Out] double[] vals);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int cgGetParameterDefaultValuefc(CGparameter param, int nelements, [Out] float[] vals);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int cgGetParameterDefaultValuefr(CGparameter param, int nelements, [Out] float[] vals);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int cgGetParameterDefaultValueic(CGparameter param, int nelements, [Out] int[] vals);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int cgGetParameterDefaultValueir(CGparameter param, int nelements, [Out] int[] vals);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGenum cgGetParameterDirection(CGparameter param);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGeffect cgGetParameterEffect(CGparameter param);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int cgGetParameterIndex(CGparameter param);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern IntPtr cgGetParameterName(CGparameter param);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int cgGetParameterNamedType(CGparameter param);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int cgGetParameterOrdinalNumber(CGparameter param);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGprogram cgGetParameterProgram(CGparameter param);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGresource cgGetParameterResource(CGparameter param);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int cgGetParameterResourceIndex(CGparameter param);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern string cgGetParameterResourceName(CGparameter param);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int cgGetParameterResourceSize(CGparameter parameter);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGtype cgGetParameterResourceType(CGparameter parameter);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int cgGetParameterRows(CGparameter param);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern IntPtr cgGetParameterSemantic(CGparameter param);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGenum cgGetParameterSettingMode(CGcontext context);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGtype cgGetParameterType(CGparameter param);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int cgGetParameterValuedc(CGparameter param, int nelements, [Out] double[] vals);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int cgGetParameterValuedr(CGparameter param, int nelements, [Out] double[] vals);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int cgGetParameterValuefc(CGparameter param, int nelements, [Out] float[] vals);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int cgGetParameterValuefr(CGparameter param, int nelements, [Out] float[] vals);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int cgGetParameterValueic(CGparameter param, int nelements, [Out] int[] vals);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int cgGetParameterValueir(CGparameter param, int nelements, [Out] int[] vals);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public unsafe static extern double* cgGetParameterValues(CGparameter param, CGenum value_type, int* nvalues);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern double[] cgGetParameterValues(CGparameter param, CGenum value_type, [In] int[] nvalues);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGenum cgGetParameterVariability(CGparameter param);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGtype cgGetParentType(CGtype type, int index);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern string cgGetPassName(CGpass pass);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGprogram cgGetPassProgram(CGpass pass, CGdomain domain);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGtechnique cgGetPassTechnique(CGpass pass);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGprofile cgGetProfile([In] string profile_string);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGdomain cgGetProfileDomain(CGprofile profile);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbool cgGetProfileProperty(CGprofile profile, CGenum query);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern string cgGetProfileString(CGprofile profile);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbuffer cgGetProgramBuffer(CGprogram program, int bufferIndex);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int cgGetProgramBufferMaxIndex(CGprofile profile);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int cgGetProgramBufferMaxSize(CGprofile profile);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGcontext cgGetProgramContext(CGprogram program);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGdomain cgGetProgramDomain(CGprogram program);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGprofile cgGetProgramDomainProfile(CGprogram program, int index);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGprogram cgGetProgramDomainProgram(CGprogram program, int index);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGenum cgGetProgramInput(CGprogram program);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern IntPtr cgGetProgramOptions(CGprogram prog);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGenum cgGetProgramOutput(CGprogram program);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGprofile cgGetProgramProfile(CGprogram program);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGprogram cgGetProgramStateAssignmentValue(CGstateassignment stateassignment);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern IntPtr cgGetProgramString(CGprogram program, CGenum sourceType);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGresource cgGetResource([In] string resource_string);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern IntPtr cgGetResourceString(CGresource resource);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGparameter cgGetSamplerStateAssignmentParameter(CGstateassignment stateassignment);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGstate cgGetSamplerStateAssignmentState(CGstateassignment stateassignment);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGparameter cgGetSamplerStateAssignmentValue(CGstateassignment stateassignment);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGenum cgGetSemanticCasePolicy();

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int cgGetStateAssignmentIndex(CGstateassignment stateassignment);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGpass cgGetStateAssignmentPass(CGstateassignment stateassignment);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGstate cgGetStateAssignmentState(CGstateassignment stateassignment);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGcontext cgGetStateContext(CGstate state);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern string cgGetStateEnumerant(CGstate state, int index, out int value);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern string cgGetStateEnumerantName(CGstate state, int value);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int cgGetStateEnumerantValue(CGstate state, [In] string name);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGprofile cgGetStateLatestProfile(CGstate state);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern string cgGetStateName(CGstate state);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern StateCallbackDelegate cgGetStateResetCallback(CGstate state);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern StateCallbackDelegate cgGetStateSetCallback(CGstate state);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGtype cgGetStateType(CGstate state);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern StateCallbackDelegate cgGetStateValidateCallback(CGstate state);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern IntPtr cgGetString(CGenum sname);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern string cgGetStringAnnotationValue(CGannotation annotation);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern IntPtr cgGetStringAnnotationValues(CGannotation ann, out int nvalues);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern string cgGetStringParameterValue(CGparameter param);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern string cgGetStringStateAssignmentValue(CGstateassignment stateassignment);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGprofile cgGetSupportedProfile(int index);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGeffect cgGetTechniqueEffect(CGtechnique technique);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern string cgGetTechniqueName(CGtechnique technique);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGparameter cgGetTextureStateAssignmentValue(CGstateassignment stateassignment);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGtype cgGetType([In] string type_string);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGtype cgGetTypeBase(CGtype type);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGparameterclass cgGetTypeClass(CGtype type);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbool cgGetTypeSizes(CGtype type, out int nrows, out int ncols);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern IntPtr cgGetTypeString(CGtype type);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGtype cgGetUserType(CGhandle handle, int index);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbool cgIsAnnotation(CGannotation annotation);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbool cgIsContext(CGcontext context);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbool cgIsEffect(CGeffect effect);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbool cgIsInterfaceType(CGtype type);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbool cgIsParameter(CGparameter param);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbool cgIsParameterGlobal(CGparameter param);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbool cgIsParameterReferenced(CGparameter param);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbool cgIsParameterUsed(CGparameter param, CGhandle handle);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbool cgIsParentType(CGtype parent, CGtype child);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbool cgIsPass(CGpass pass);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbool cgIsProfileSupported(CGprofile profile);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbool cgIsProgram(CGprogram program);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbool cgIsProgramCompiled(CGprogram program);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbool cgIsState(CGstate state);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbool cgIsStateAssignment(CGstateassignment stateassignment);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbool cgIsTechnique(CGtechnique technique);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbool cgIsTechniqueValidated(CGtechnique technique);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern IntPtr cgMapBuffer(CGbuffer buffer, CGbufferaccess access);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgResetPassState(CGpass pass);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetArraySize(CGparameter param, int size);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetAutoCompile(CGcontext context, CGenum autoCompileMode);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbool cgSetBoolAnnotation(CGannotation annotation, CGbool value);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbool cgSetBoolArrayStateAssignment(CGstateassignment stateassignment, [In]CGbool[] vals);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbool cgSetBoolStateAssignment(CGstateassignment stateassignment, CGbool value);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetBufferData(CGbuffer buffer, int size, [In] IntPtr data);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetBufferSubData(CGbuffer buffer, int offset, int size, [In] IntPtr data);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetCompilerIncludeCallback(CGcontext context, IncludeCallbackFuncDelegate func);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetCompilerIncludeFile(CGcontext context, string name, string filename);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetCompilerIncludeString(CGcontext context, string name, string source);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetContextBehavior(CGcontext context, CGbehavior behavior);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbool cgSetEffectName(CGeffect effect, [In] string name);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetEffectParameterBuffer(CGparameter param, CGbuffer buffer);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetErrorCallback(ErrorCallbackFuncDelegate func);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetErrorHandler(ErrorHandlerFuncDelegate func, IntPtr data);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbool cgSetFloatAnnotation(CGannotation annotation, float value);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbool cgSetFloatArrayStateAssignment(CGstateassignment stateassignment, [In]float[] vals);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbool cgSetFloatStateAssignment(CGstateassignment stateassignment, float value);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbool cgSetIntAnnotation(CGannotation annotation, int value);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbool cgSetIntArrayStateAssignment(CGstateassignment stateassignment, [In]int[] vals);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbool cgSetIntStateAssignment(CGstateassignment stateassignment, int value);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetLastListing(CGhandle handle, string listing);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGenum cgSetLockingPolicy(CGenum lockingPolicy);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetMatrixParameterdc(CGparameter param, [In] double[] matrix);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetMatrixParameterdr(CGparameter param, [In] double[] matrix);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetMatrixParameterfc(CGparameter param, [In] float[] matrix);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetMatrixParameterfr(CGparameter param, [In] float[] matrix);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetMatrixParameteric(CGparameter param, [In] int[] matrix);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetMatrixParameterir(CGparameter param, [In] int[] matrix);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetMultiDimArraySize(CGparameter param, int[] sizes);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetParameter1d(CGparameter param, double x);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetParameter1dv(CGparameter param, [In] double[] v);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetParameter1f(CGparameter param, float x);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetParameter1fv(CGparameter param, [In] float[] v);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetParameter1i(CGparameter param, int x);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetParameter1iv(CGparameter param, [In] int[] v);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetParameter2d(CGparameter param, double x, double y);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetParameter2dv(CGparameter param, [In] double[] v);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetParameter2f(CGparameter param, float x, float y);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetParameter2fv(CGparameter param, [In] float[] v);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetParameter2i(CGparameter param, int x, int y);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetParameter2iv(CGparameter param, [In] int[] v);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetParameter3d(CGparameter param, double x, double y, double z);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetParameter3dv(CGparameter param, [In] double[] v);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetParameter3f(CGparameter param, float x, float y, float z);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetParameter3fv(CGparameter param, [In] float[] v);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetParameter3i(CGparameter param, int x, int y, int z);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetParameter3iv(CGparameter param, [In] int[] v);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetParameter4d(CGparameter param, double x, double y, double z, double w);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetParameter4dv(CGparameter param, [In] double[] v);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetParameter4f(CGparameter param, float x, float y, float z, float w);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetParameter4fv(CGparameter param, [In] float[] v);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetParameter4i(CGparameter param, int x, int y, int z, int w);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetParameter4iv(CGparameter param, [In] int[] v);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetParameterSemantic(CGparameter param, [In] string semantic);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetParameterSettingMode(CGcontext context, CGenum parameterSettingMode);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetParameterValuedc(CGparameter param, int nelements, [In] double[] vals);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetParameterValuedr(CGparameter param, int nelements, [In] double[] vals);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetParameterValuefc(CGparameter param, int nelements, [In] float[] vals);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetParameterValuefr(CGparameter param, int nelements, [In] float[] vals);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetParameterValueic(CGparameter param, int nelements, [In] int[] vals);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetParameterValueir(CGparameter param, int nelements, [In] int[] vals);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetParameterVariability(CGparameter param, CGenum vary);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetPassProgramParameters(CGprogram prog);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetPassState(CGpass pass);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetProgramBuffer(CGprogram program, int bufferIndex, CGbuffer buffer);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetProgramProfile(CGprogram prog, CGprofile profile);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbool cgSetProgramStateAssignment(CGstateassignment stateassignment, CGprogram program);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetSamplerState(CGparameter param);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbool cgSetSamplerStateAssignment(CGstateassignment stateassignment, CGparameter parameter);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGenum cgSetSemanticCasePolicy(CGenum casePolicy);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetStateCallbacks(CGstate state, StateCallbackDelegate set, StateCallbackDelegate reset, StateCallbackDelegate validate);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetStateLatestProfile(CGstate state, CGprofile profile);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbool cgSetStringAnnotation(CGannotation annotation, [In] string value);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgSetStringParameterValue(CGparameter param, [In] string str);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbool cgSetStringStateAssignment(CGstateassignment stateassignment, [In]string name);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbool cgSetTextureStateAssignment(CGstateassignment stateassignment, CGparameter parameter);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgUnmapBuffer(CGbuffer buffer);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgUpdatePassParameters(CGpass pass);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void cgUpdateProgramParameters(CGprogram program);

        [DllImport("cg.dll", CallingConvention =  CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern CGbool cgValidateTechnique(CGtechnique technique);

        #endregion

        #region Dx
        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgD3D11GetDevice(CGcontext Context);
        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgD3D11SetDevice(CGcontext Context, IntPtr pDevice);
        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgD3D11SetTextureParameter(CGparameter Parameter, IntPtr pTexture);
        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgD3D11SetSamplerStateParameter(CGparameter Parameter, IntPtr pSamplerState);
        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgD3D11SetTextureSamplerStateParameter(CGparameter Parameter, IntPtr pTexture, IntPtr pSamplerState);
        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgD3D11LoadProgram(CGprogram Program, uint Flags);
        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgD3D11GetCompiledProgram(CGprogram Program);
        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgD3D11GetProgramErrors(CGprogram Program);
        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern CGbool cgD3D11IsProgramLoaded(CGprogram Program);
        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgD3D11BindProgram(CGprogram Program);
        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgD3D11UnloadProgram(CGprogram Program);
        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgD3D11GetBufferByIndex(CGprogram Program, uint Index);
        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgD3D11RegisterStates(CGcontext Context);
        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgD3D11SetManageTextureParameters(CGcontext Context, CGbool Flag);
        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern CGbool cgD3D11GetManageTextureParameters(CGcontext Context);
        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgD3D11GetIASignatureByPass(CGpass Pass);
        [DllImport("cgD3D11.dll")]
        public static extern CGprofile cgD3D11GetLatestVertexProfile();
        [DllImport("cgD3D11.dll")]
        public static extern CGprofile cgD3D11GetLatestGeometryProfile();
        [DllImport("cgD3D11.dll")]
        public static extern CGprofile cgD3D11GetLatestPixelProfile();
        [DllImport("cgD3D11.dll")]
        public static extern CGprofile cgD3D11GetLatestHullProfile();
        [DllImport("cgD3D11.dll")]
        public static extern CGprofile cgD3D11GetLatestDomainProfile();
        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern CGbool cgD3D11IsProfileSupported(CGprofile Profile);
        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgD3D11TypeToSize(CGtype Type);
        [DllImport("cgD3D11.dll")]
        public static extern int cgD3D11GetLastError();
        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        private static extern IntPtr cgD3D11GetOptimalOptions(CGprofile Profile);
        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern string cgD3D11TranslateCGerror(CGerror Error);
        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern string cgD3D11TranslateHRESULT(int hr);
        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgD3D11UnbindProgram(CGprogram Program);
        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern CGbuffer cgD3D11CreateBuffer(CGcontext Context, int size, IntPtr data, SharpDX.Direct3D11.ResourceUsage bufferUsage);
        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern CGbuffer cgD3D11CreateBufferFromObject(CGcontext Context, IntPtr obj, CGbool manageObject);
        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgD3D11GetBufferObject(CGbuffer buffer);
        #endregion

        #region GL
        [DllImport("cgGL.dll", CallingConvention = CallingConvention.ThisCall)]
        private static extern IntPtr cgGLGetOptimalOptions(CGprofile prog);
        #endregion

        #region Wrapper
        public unsafe static string[] GetOptimalOptions(CGprofile profile, RenderType renderType = RenderType.DirectX11)
        {
            try
            {
                var byteArray =
                    (byte**)
                        (renderType == RenderType.OpneGl3
                            ? cgGLGetOptimalOptions(profile)
                            : cgD3D11GetOptimalOptions(profile));
                var buffer = new List<byte>();
                var lines = new List<string>();

                for (; *byteArray != null; byteArray++)
                {
                    for (var b = *byteArray; *b != '\0'; b++)
                        buffer.Add(*b);

                    lines.Add(new string(Encoding.ASCII.GetChars(buffer.ToArray())));
                    buffer.Clear();
                }
                return lines.ToArray();
            }
            catch
            {
                return null;
            }
        }
        #endregion
    }
}