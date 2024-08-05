using System.Reflection;


[assembly: AssemblyProduct("BSS Framework")]
[assembly: AssemblyCompany("Luxoft")]
[assembly: AssemblyCopyright("Copyright © Luxoft 2009-2024")]

[assembly: AssemblyVersion(AssemblyVersionData.Value)]
[assembly: AssemblyFileVersion(AssemblyVersionData.Value)]
[assembly: AssemblyInformationalVersion(AssemblyVersionData.Value)]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

internal static class AssemblyVersionData
{
    public const string Value = "21.5.8.0";
}
