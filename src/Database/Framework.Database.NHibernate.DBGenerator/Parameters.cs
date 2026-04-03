namespace Framework.Database.NHibernate.DBGenerator;

public struct Parameters(
    string outputPath,
    string assemblyPath,
    string domainObjectBaseName,
    string databaseServerName,
    string databaseName,
    string generatedAssemblyName)
{
    /// <summary>
    /// ѕуть генерации cs файлов
    /// </summary>
    public string OutputPath { get; private set; } = outputPath;

    public string AssemblyPath { get; private set; } = assemblyPath;

    public string DomainObjectBaseName { get; private set; } = domainObjectBaseName;

    public string DatabaseServerName { get; private set; } = databaseServerName;

    public string DatabaseName { get; private set; } = databaseName;

    public string GeneratedAssemblyName { get; private set; } = generatedAssemblyName;
}
