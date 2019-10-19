[assembly: Microsoft.VisualStudio.TestTools.UnitTesting.Parallelize(
    Workers = 4,
    Scope = Microsoft.VisualStudio.TestTools.UnitTesting.ExecutionScope.MethodLevel)]
namespace BlockchainNet.Test
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public static class TestAssemblyInitialize
    {
        [AssemblyInitialize]
        public static void Initialize(TestContext context)
        {
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Debugger.Break();
        }

        private static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            e.Exception.Handle(ex =>
            {
                Debugger.Break();
                return true;
            });
        }
    }
}
