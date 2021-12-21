using System.Reflection;

// TODO Move settings to Directory.Build.props (https://docs.microsoft.com/en-us/dotnet/core/tools/csproj)

[assembly: AssemblyProduct("BSS Framework")]
[assembly: AssemblyCompany("Luxoft")]
[assembly: AssemblyCopyright("Copyright © Luxoft 2009-2019")]

[assembly: AssemblyVersion("10.7.8.0")]
[assembly: AssemblyFileVersion("10.7.8.0")]
[assembly: AssemblyInformationalVersion("10.7.8")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
