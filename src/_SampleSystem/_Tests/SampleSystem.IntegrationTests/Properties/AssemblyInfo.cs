using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("SampleSystem.IntegrationTests")]
[assembly: ComVisible(false)]

#if !DEBUG
[assembly: Parallelize(Scope = ExecutionScope.ClassLevel)]
#endif
