using System.Reflection;
using System.Runtime.InteropServices;

using Microsoft.VisualStudio.TestTools.UnitTesting;

[assembly: AssemblyTitle("SampleSystem.IntegrationTests")]
[assembly: ComVisible(false)]
[assembly: Parallelize(Scope = ExecutionScope.ClassLevel)]
