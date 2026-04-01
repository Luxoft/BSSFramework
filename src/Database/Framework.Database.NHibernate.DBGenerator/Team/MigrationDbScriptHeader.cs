namespace Framework.Database.NHibernate.DBGenerator.Team;

public struct MigrationDbScriptHeader(string name, string scheme, string version)
{
    public string Name => name;

    public string Scheme => scheme;

    public string Version => version;
}
