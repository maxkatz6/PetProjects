using System;
using System.Runtime.InteropServices;

namespace Ormeli.CG
{
    public struct PnString
    {
        private IntPtr IntPtr;

        public string String
        {
            get { return ToString(); }
        }
        public override string ToString()
        {
            return Marshal.PtrToStringAnsi(IntPtr);
        }
    }

    public class CgImports
    {
        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern CGenum cgSetLockingPolicy(CGenum lockingPolicy);

        [DllImport("cg.dll")]
        public static extern CGenum cgGetLockingPolicy();

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern CGenum cgSetSemanticCasePolicy(CGenum casePolicy);

        [DllImport("cg.dll")]
        public static extern CGenum cgGetSemanticCasePolicy();

        [DllImport("cg.dll")]
        public static extern IntPtr cgCreateContext();

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetContextBehavior(IntPtr context, CGbehavior behavior);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern CGbehavior cgGetContextBehavior(IntPtr context);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern PnString cgGetBehaviorString(CGbehavior behavior);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern CGbehavior cgGetBehavior(string behavior_string);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgDestroyContext(IntPtr context);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool cgIsContext(IntPtr context);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern PnString cgGetLastListing(IntPtr context);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern void cgSetLastListing(IntPtr handle, string listing);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetAutoCompile(IntPtr context, CGenum autoCompileMode);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern CGenum cgGetAutoCompile(IntPtr context);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetParameterSettingMode(IntPtr context, CGenum parameterSettingMode);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern CGenum cgGetParameterSettingMode(IntPtr context);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern void cgSetCompilerIncludeString(IntPtr context, string name, string source);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern void cgSetCompilerIncludeFile(IntPtr context, string name, string filename);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgCreateProgram(IntPtr context, CGenum program_type, string program,
            CGprofile profile, string entry, IntPtr args);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgCreateProgramFromFile(IntPtr context, CGenum program_type, string program_file,
            CGprofile profile, string entry, IntPtr args);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgCopyProgram(IntPtr program);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgDestroyProgram(IntPtr program);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetFirstProgram(IntPtr context);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetNextProgram(IntPtr current);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetProgramContext(IntPtr program);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool cgIsProgram(IntPtr program);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgCompileProgram(IntPtr program);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool cgIsProgramCompiled(IntPtr program);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern PnString cgGetProgramString(IntPtr program, CGenum pname);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern CGprofile cgGetProgramProfile(IntPtr program);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgGetProgramOptions(IntPtr program);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetProgramProfile(IntPtr program, CGprofile profile);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern CGenum cgGetProgramInput(IntPtr program);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern CGenum cgGetProgramOutput(IntPtr program);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgGetProgramOutputVertices(IntPtr program);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetProgramOutputVertices(IntPtr program, int vertices);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetPassProgramParameters(IntPtr program);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgUpdateProgramParameters(IntPtr program);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgUpdatePassParameters(IntPtr pass);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgCreateParameter(IntPtr context, CGtype type);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgCreateParameterArray(IntPtr context, CGtype type, int length);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgCreateParameterMultiDimArray(IntPtr context, CGtype type, int dim, IntPtr lengths);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgDestroyParameter(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgConnectParameter(IntPtr from, IntPtr to);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgDisconnectParameter(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetConnectedParameter(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgGetNumConnectedToParameters(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetConnectedToParameter(IntPtr param, int index);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgGetNamedParameter(IntPtr program, string name);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgGetNamedProgramParameter(IntPtr program, CGenum name_space, string name);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgGetNamedProgramUniformBuffer(IntPtr program, string blockName);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgGetNamedEffectUniformBuffer(IntPtr effect, string blockName);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern PnString cgGetUniformBufferBlockName(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetFirstParameter(IntPtr program, CGenum name_space);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetNextParameter(IntPtr current);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetFirstLeafParameter(IntPtr program, CGenum name_space);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetNextLeafParameter(IntPtr current);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetFirstStructParameter(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetFirstUniformBufferParameter(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgGetNamedStructParameter(IntPtr param, string name);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgGetNamedUniformBufferParameter(IntPtr param, string name);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetFirstDependentParameter(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetArrayParameter(IntPtr aparam, int index);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgGetArrayDimension(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern CGtype cgGetArrayType(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgGetArraySize(IntPtr param, int dimension);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgGetArrayTotalSize(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetArraySize(IntPtr param, int size);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetMultiDimArraySize(IntPtr param, IntPtr sizes);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetParameterProgram(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetParameterContext(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool cgIsParameter(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern PnString cgGetParameterName(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern CGtype cgGetParameterType(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern CGtype cgGetParameterBaseType(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern CGparameterclass cgGetParameterClass(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgGetParameterRows(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgGetParameterColumns(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern CGtype cgGetParameterNamedType(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern PnString cgGetParameterSemantic(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern CGresource cgGetParameterResource(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern CGresource cgGetParameterBaseResource(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern ulong cgGetParameterResourceIndex(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern CGenum cgGetParameterVariability(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern CGenum cgGetParameterDirection(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool cgIsParameterReferenced(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool cgIsParameterUsed(IntPtr param, IntPtr handle);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetParameterValues(IntPtr param, CGenum value_type, IntPtr nvalues);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetParameterValuedr(IntPtr param, int nelements, IntPtr vals);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetParameterValuedc(IntPtr param, int nelements, IntPtr vals);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetParameterValuefr(IntPtr param, int nelements, IntPtr vals);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetParameterValuefc(IntPtr param, int nelements, IntPtr vals);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetParameterValueir(IntPtr param, int nelements, IntPtr vals);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetParameterValueic(IntPtr param, int nelements, IntPtr vals);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgGetParameterValuedr(IntPtr param, int nelements, IntPtr vals);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgGetParameterValuedc(IntPtr param, int nelements, IntPtr vals);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgGetParameterValuefr(IntPtr param, int nelements, IntPtr vals);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgGetParameterValuefc(IntPtr param, int nelements, IntPtr vals);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgGetParameterValueir(IntPtr param, int nelements, IntPtr vals);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgGetParameterValueic(IntPtr param, int nelements, IntPtr vals);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgGetParameterDefaultValuedr(IntPtr param, int nelements, IntPtr vals);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgGetParameterDefaultValuedc(IntPtr param, int nelements, IntPtr vals);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgGetParameterDefaultValuefr(IntPtr param, int nelements, IntPtr vals);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgGetParameterDefaultValuefc(IntPtr param, int nelements, IntPtr vals);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgGetParameterDefaultValueir(IntPtr param, int nelements, IntPtr vals);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgGetParameterDefaultValueic(IntPtr param, int nelements, IntPtr vals);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern PnString cgGetStringParameterValue(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern void cgSetStringParameterValue(IntPtr param, string str);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgGetParameterOrdinalNumber(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool cgIsParameterGlobal(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgGetParameterIndex(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetParameterVariability(IntPtr param, CGenum vary);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern void cgSetParameterSemantic(IntPtr param, string semantic);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetParameter1f(IntPtr param, float x);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetParameter2f(IntPtr param, float x, float y);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetParameter3f(IntPtr param, float x, float y, float z);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetParameter4f(IntPtr param, float x, float y, float z, float w);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetParameter1d(IntPtr param, double x);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetParameter2d(IntPtr param, double x, double y);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetParameter3d(IntPtr param, double x, double y, double z);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetParameter4d(IntPtr param, double x, double y, double z, double w);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetParameter1i(IntPtr param, int x);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetParameter2i(IntPtr param, int x, int y);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetParameter3i(IntPtr param, int x, int y, int z);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetParameter4i(IntPtr param, int x, int y, int z, int w);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetParameter1iv(IntPtr param, IntPtr v);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetParameter2iv(IntPtr param, IntPtr v);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetParameter3iv(IntPtr param, IntPtr v);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetParameter4iv(IntPtr param, IntPtr v);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetParameter1fv(IntPtr param, IntPtr v);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetParameter2fv(IntPtr param, IntPtr v);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetParameter3fv(IntPtr param, IntPtr v);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetParameter4fv(IntPtr param, IntPtr v);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetParameter1dv(IntPtr param, IntPtr v);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetParameter2dv(IntPtr param, IntPtr v);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetParameter3dv(IntPtr param, IntPtr v);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetParameter4dv(IntPtr param, IntPtr v);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetMatrixParameterir(IntPtr param, IntPtr matrix);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetMatrixParameterdr(IntPtr param, IntPtr matrix);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetMatrixParameterfr(IntPtr param, IntPtr matrix);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetMatrixParameteric(IntPtr param, IntPtr matrix);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetMatrixParameterdc(IntPtr param, IntPtr matrix);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetMatrixParameterfc(IntPtr param, IntPtr matrix);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgGetMatrixParameterir(IntPtr param, IntPtr matrix);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgGetMatrixParameterdr(IntPtr param, IntPtr matrix);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgGetMatrixParameterfr(IntPtr param, IntPtr matrix);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgGetMatrixParameteric(IntPtr param, IntPtr matrix);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgGetMatrixParameterdc(IntPtr param, IntPtr matrix);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgGetMatrixParameterfc(IntPtr param, IntPtr matrix);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern CGenum cgGetMatrixParameterOrder(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgGetNamedSubParameter(IntPtr param, string name);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern PnString cgGetTypeString(CGtype type);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern CGtype cgGetType(string type_string);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern CGtype cgGetNamedUserType(IntPtr handle, string name);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgGetNumUserTypes(IntPtr handle);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern CGtype cgGetUserType(IntPtr handle, int index);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgGetNumParentTypes(CGtype type);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern CGtype cgGetParentType(CGtype type, int index);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool cgIsParentType(CGtype parent, CGtype child);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool cgIsInterfaceType(CGtype type);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern PnString cgGetResourceString(CGresource resource);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern CGresource cgGetResource(string resource_string);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern PnString cgGetEnumString(CGenum en);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern CGenum cgGetEnum(string enum_string);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern PnString cgGetProfileString(CGprofile profile);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern CGprofile cgGetProfile(string profile_string);

        [DllImport("cg.dll")]
        public static extern int cgGetNumSupportedProfiles();

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern CGprofile cgGetSupportedProfile(int index);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool cgIsProfileSupported(CGprofile profile);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool cgGetProfileProperty(CGprofile profile, CGenum query);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern PnString cgGetParameterClassString(CGparameterclass pc);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern CGparameterclass cgGetParameterClassEnum(string pString);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern PnString cgGetDomainString(CGdomain domain);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern CGdomain cgGetDomain(string domain_string);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern CGdomain cgGetProgramDomain(IntPtr program);

        [DllImport("cg.dll")]
        public static extern CGerror cgGetError();

        [DllImport("cg.dll")]
        public static extern CGerror cgGetFirstError();

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern PnString cgGetErrorString(CGerror error);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern PnString cgGetLastErrorString(IntPtr error);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetErrorCallback(IntPtr func);

        [DllImport("cg.dll")]
        public static extern IntPtr cgGetErrorCallback();

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetErrorHandler(IntPtr func, IntPtr data);

        [DllImport("cg.dll")]
        public static extern IntPtr cgGetErrorHandler();

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern PnString cgGetString(CGenum sname);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgCreateEffect(IntPtr context, string code, IntPtr args);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgCreateEffectFromFile(IntPtr context, string filename, IntPtr args);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgCopyEffect(IntPtr effect);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgDestroyEffect(IntPtr effect);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetEffectContext(IntPtr effect);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool cgIsEffect(IntPtr effect);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetFirstEffect(IntPtr context);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetNextEffect(IntPtr effect);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgCreateProgramFromEffect(IntPtr effect, CGprofile profile, string entry,
            IntPtr args);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetFirstTechnique(IntPtr effect);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetNextTechnique(IntPtr tech);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgGetNamedTechnique(IntPtr effect, string name);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern PnString cgGetTechniqueName(IntPtr tech);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool cgIsTechnique(IntPtr tech);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool cgValidateTechnique(IntPtr tech);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool cgIsTechniqueValidated(IntPtr tech);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetTechniqueEffect(IntPtr tech);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetFirstPass(IntPtr tech);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgGetNamedPass(IntPtr tech, string name);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetNextPass(IntPtr pass);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool cgIsPass(IntPtr pass);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern string cgGetPassName(IntPtr pass);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetPassTechnique(IntPtr pass);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetPassProgram(IntPtr pass, CGdomain domain);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetPassState(IntPtr pass);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgResetPassState(IntPtr pass);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetFirstStateAssignment(IntPtr pass);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgGetNamedStateAssignment(IntPtr pass, string name);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetNextStateAssignment(IntPtr sa);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool cgIsStateAssignment(IntPtr sa);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool cgCallStateSetCallback(IntPtr sa);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool cgCallStateValidateCallback(IntPtr sa);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool cgCallStateResetCallback(IntPtr sa);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetStateAssignmentPass(IntPtr sa);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetSamplerStateAssignmentParameter(IntPtr sa);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetFloatStateAssignmentValues(IntPtr sa, IntPtr nvalues);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetIntStateAssignmentValues(IntPtr sa, IntPtr nvalues);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetBoolStateAssignmentValues(IntPtr sa, IntPtr nvalues);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern PnString cgGetStringStateAssignmentValue(IntPtr sa);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetProgramStateAssignmentValue(IntPtr sa);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetTextureStateAssignmentValue(IntPtr sa);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetSamplerStateAssignmentValue(IntPtr sa);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgGetStateAssignmentIndex(IntPtr sa);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgGetNumDependentStateAssignmentParameters(IntPtr sa);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetDependentStateAssignmentParameter(IntPtr sa, int index);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgGetNumDependentProgramArrayStateAssignmentParameters(IntPtr sa);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetDependentProgramArrayStateAssignmentParameter(IntPtr sa, int index);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetConnectedStateAssignmentParameter(IntPtr sa);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetStateAssignmentState(IntPtr sa);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetSamplerStateAssignmentState(IntPtr sa);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgCreateState(IntPtr context, string name, CGtype type);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgCreateArrayState(IntPtr context, string name, CGtype type, int nelements);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetStateLatestProfile(IntPtr state, CGprofile profile);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern CGprofile cgGetStateLatestProfile(IntPtr state);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetStateContext(IntPtr state);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern CGtype cgGetStateType(IntPtr state);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern PnString cgGetStateName(IntPtr state);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgGetNamedState(IntPtr context, string name);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetFirstState(IntPtr context);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetNextState(IntPtr state);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool cgIsState(IntPtr state);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern void cgAddStateEnumerant(IntPtr state, string name, int value);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgCreateSamplerState(IntPtr context, string name, CGtype type);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgCreateArraySamplerState(IntPtr context, string name, CGtype type, int nelements);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgGetNamedSamplerState(IntPtr context, string name);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetFirstSamplerState(IntPtr context);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetFirstSamplerStateAssignment(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgGetNamedSamplerStateAssignment(IntPtr param, string name);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetSamplerState(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgGetNamedEffectParameter(IntPtr effect, string name);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetFirstLeafEffectParameter(IntPtr effect);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetFirstEffectParameter(IntPtr effect);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgGetEffectParameterBySemantic(IntPtr effect, string semantic);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetFirstTechniqueAnnotation(IntPtr tech);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetFirstPassAnnotation(IntPtr pass);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetFirstParameterAnnotation(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetFirstProgramAnnotation(IntPtr program);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetFirstEffectAnnotation(IntPtr effect);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetNextAnnotation(IntPtr ann);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgGetNamedTechniqueAnnotation(IntPtr tech, string name);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgGetNamedPassAnnotation(IntPtr pass, string name);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgGetNamedParameterAnnotation(IntPtr param, string name);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgGetNamedProgramAnnotation(IntPtr program, string name);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgGetNamedEffectAnnotation(IntPtr effect, string name);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool cgIsAnnotation(IntPtr ann);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern PnString cgGetAnnotationName(IntPtr ann);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern CGtype cgGetAnnotationType(IntPtr ann);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetFloatAnnotationValues(IntPtr ann, IntPtr nvalues);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetIntAnnotationValues(IntPtr ann, IntPtr nvalues);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern PnString cgGetStringAnnotationValue(IntPtr ann);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgGetStringAnnotationValues(IntPtr ann, IntPtr nvalues);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetBoolAnnotationValues(IntPtr ann, IntPtr nvalues);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetBooleanAnnotationValues(IntPtr ann, IntPtr nvalues);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgGetNumDependentAnnotationParameters(IntPtr ann);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetDependentAnnotationParameter(IntPtr ann, int index);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgEvaluateProgram(IntPtr program, IntPtr buf, int ncomps, int nx, int ny, int nz);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern bool cgSetEffectName(IntPtr effect, string name);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern PnString cgGetEffectName(IntPtr effect);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgGetNamedEffect(IntPtr context, string name);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgCreateEffectParameter(IntPtr effect, string name, CGtype type);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgCreateTechnique(IntPtr effect, string name);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgCreateEffectParameterArray(IntPtr effect, string name, CGtype type, int length);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgCreateEffectParameterMultiDimArray(IntPtr effect, string name, CGtype type,
            int dim, IntPtr lengths);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgCreatePass(IntPtr tech, string name);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgCreateStateAssignment(IntPtr pass, IntPtr state);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgCreateStateAssignmentIndex(IntPtr pass, IntPtr state, int index);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgCreateSamplerStateAssignment(IntPtr param, IntPtr state);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool cgSetFloatStateAssignment(IntPtr sa, float value);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool cgSetIntStateAssignment(IntPtr sa, int value);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool cgSetBoolStateAssignment(IntPtr sa, bool value);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern bool cgSetStringStateAssignment(IntPtr sa, string value);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool cgSetProgramStateAssignment(IntPtr sa, IntPtr program);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool cgSetSamplerStateAssignment(IntPtr sa, IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool cgSetTextureStateAssignment(IntPtr sa, IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool cgSetFloatArrayStateAssignment(IntPtr sa, IntPtr vals);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool cgSetIntArrayStateAssignment(IntPtr sa, IntPtr vals);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool cgSetBoolArrayStateAssignment(IntPtr sa, IntPtr vals);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgCreateTechniqueAnnotation(IntPtr tech, string name, CGtype type);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgCreatePassAnnotation(IntPtr pass, string name, CGtype type);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgCreateParameterAnnotation(IntPtr param, string name, CGtype type);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgCreateProgramAnnotation(IntPtr program, string name, CGtype type);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgCreateEffectAnnotation(IntPtr effect, string name, CGtype type);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool cgSetIntAnnotation(IntPtr ann, int value);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool cgSetFloatAnnotation(IntPtr ann, float value);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool cgSetBoolAnnotation(IntPtr ann, bool value);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern bool cgSetStringAnnotation(IntPtr ann, string value);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgGetNumStateEnumerants(IntPtr state);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern PnString cgGetStateEnumerant(IntPtr state, int index, IntPtr value);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern PnString cgGetStateEnumerantName(IntPtr state, int value);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern int cgGetStateEnumerantValue(IntPtr state, string name);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetParameterEffect(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern CGparameterclass cgGetTypeClass(CGtype type);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern CGtype cgGetTypeBase(CGtype type);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool cgGetTypeSizes(CGtype type, IntPtr nrows, IntPtr ncols);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgGetMatrixSize(CGtype type, IntPtr nrows, IntPtr ncols);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgGetNumProgramDomains(IntPtr program);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern CGdomain cgGetProfileDomain(CGprofile profile);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern CGprofile cgGetProfileSibling(CGprofile profile, CGdomain domain);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgCombinePrograms(int n, IntPtr exeList);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgCombinePrograms2(IntPtr exe1, IntPtr exe2);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgCombinePrograms3(IntPtr exe1, IntPtr exe2, IntPtr exe3);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgCombinePrograms4(IntPtr exe1, IntPtr exe2, IntPtr exe3, IntPtr exe4);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgCombinePrograms5(IntPtr exe1, IntPtr exe2, IntPtr exe3, IntPtr exe4, IntPtr exe5);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern CGprofile cgGetProgramDomainProfile(IntPtr program, int index);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetProgramDomainProgram(IntPtr program, int index);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgCreateObj(IntPtr context, CGenum program_type, string source, CGprofile profile,
            IntPtr args);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgCreateObjFromFile(IntPtr context, CGenum program_type, string source_file,
            CGprofile profile, IntPtr args);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgDestroyObj(IntPtr obj);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern long cgGetParameterResourceSize(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern CGtype cgGetParameterResourceType(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern PnString cgGetParameterResourceName(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgGetParameterBufferIndex(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgGetParameterBufferOffset(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgCreateBuffer(IntPtr context, int size, IntPtr data, CGbufferusage bufferUsage);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool cgIsBuffer(IntPtr buffer);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetBufferData(IntPtr buffer, int size, IntPtr data);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetBufferSubData(IntPtr buffer, int offset, int size, IntPtr data);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetProgramBuffer(IntPtr program, int bufferIndex, IntPtr buffer);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetUniformBufferParameter(IntPtr param, IntPtr buffer);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgMapBuffer(IntPtr buffer, CGbufferaccess access);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgUnmapBuffer(IntPtr buffer);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgDestroyBuffer(IntPtr buffer);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetProgramBuffer(IntPtr program, int bufferIndex);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetUniformBufferParameter(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgGetBufferSize(IntPtr buffer);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgGetProgramBufferMaxSize(CGprofile profile);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgGetProgramBufferMaxIndex(CGprofile profile);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetEffectParameterBuffer(IntPtr param);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetEffectParameterBuffer(IntPtr param, IntPtr buffer);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetCompilerIncludeCallback(IntPtr context, IntPtr func);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetCompilerIncludeCallback(IntPtr context);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgSetStateCallbacks(IntPtr state, IntPtr set, IntPtr reset, IntPtr validate);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetStateSetCallback(IntPtr state);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetStateResetCallback(IntPtr state);

        [DllImport("cg.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgGetStateValidateCallback(IntPtr state);

        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern PnString cgD3D11TranslateHresult(int hr);

        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgD3D11GetDevice(IntPtr Context);

        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgD3D11SetDevice(IntPtr Context, IntPtr pDevice);

        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgD3D11SetTextureParameter(IntPtr Parameter, IntPtr pTexture);

        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgD3D11SetSamplerStateParameter(IntPtr Parameter, IntPtr pSamplerState);

        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgD3D11SetTextureSamplerStateParameter(IntPtr Parameter, IntPtr pTexture,
            IntPtr pSamplerState);

        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgD3D11LoadProgram(IntPtr Program, uint Flags);

        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgD3D11GetCompiledProgram(IntPtr Program);

        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgD3D11GetProgramErrors(IntPtr Program);

        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool cgD3D11IsProgramLoaded(IntPtr Program);

        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgD3D11BindProgram(IntPtr Program);

        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgD3D11UnloadProgram(IntPtr Program);

        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgD3D11GetBufferByIndex(IntPtr Program, uint Index);

        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgD3D11RegisterStates(IntPtr Context);

        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgD3D11SetManageTextureParameters(IntPtr Context, bool Flag);

        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool cgD3D11GetManageTextureParameters(IntPtr Context);

        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgD3D11GetIASignatureByPass(IntPtr Pass);

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
        public static extern bool cgD3D11IsProfileSupported(CGprofile Profile);

        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern int cgD3D11TypeToSize(CGtype Type);

        [DllImport("cgD3D11.dll")]
        public static extern int cgD3D11GetLastError();

        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr cgD3D11GetOptimalOptions(CGprofile Profile);

        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public static extern PnString cgD3D11TranslateCGerror(CGerror Error);

        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern void cgD3D11UnbindProgram(IntPtr Program);

        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgD3D11CreateBuffer(IntPtr Context, int size, IntPtr data, uint bufferUsage);

        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgD3D11CreateBufferFromObject(IntPtr Context, IntPtr obj, bool manageObject);

        [DllImport("cgD3D11.dll", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr cgD3D11GetBufferObject(IntPtr buffer);
    }
}