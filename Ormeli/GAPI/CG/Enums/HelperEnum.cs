namespace Ormeli.Cg
{
    public partial class CG
    {
        public enum AutoCompileMode
        {
            CompileManual = 4114,
            CompileImmediate = 4115,
            CompileLazy = 4116,
        }

        public enum CasePolicy
        {
            ForceUpperCasePolicy = 4136,
            UnchangedCasePolicy = 4137,
        }

        public enum LockingPolicy
        {
            NoLocksPolicy = 4134,
            ThreadSafePolicy = 4135,
        }

        public enum Order
        {
            ColumnMajor = 4121,
            RowMajor = 4120
        }

        public enum ParameterSettingMode
        {
            Immediate = 4132,
            Deferred = 4133
        }

        public enum ProgramInput
        {
            Fragment = 4122,
            Vertex = 4123,
            Point = 4124,
            Line = 4125,
            LineAdj = 4126,
            Triangle = 4127,
            TriangleAdj = 4128,
            PointOut = 4129,
            LineOut = 4130,
            TriangleOut = 4131,
            Patch = 4152,
        }

        public enum ProgramNamespace
        {
            Global = 4108,
            Program = 4109,
        }

        public enum ProgramOutput
        {
            Fragment = 4122,
            Vertex = 4123,
            Point = 4124,
            Line = 4125,
            LineAdj = 4126,
            Triangle = 4127,
            TriangleAdj = 4128,
            PointOut = 4129,
            LineOut = 4130,
            TriangleOut = 4131,
            Patch = 4152,
        }

        public enum ProgramType
        {
            Source = 4112,
            Object = 4113
        }

        public enum Query
        {
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
        }

        public enum SourceType
        {
            ProgramSource = 4104,
            ProgramEntry = 4105,
            CompiledProgram = 4106,
            ProgramProfile = 4107,
        }
    }
}