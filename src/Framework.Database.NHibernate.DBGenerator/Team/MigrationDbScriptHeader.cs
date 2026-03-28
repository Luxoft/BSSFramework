namespace Framework.Database.NHibernate.DBGenerator.Team;

public struct MigrationDbScriptHeader
{
    private readonly string name;
    private readonly string scheme;
    private readonly string version;

    public MigrationDbScriptHeader(string name, string scheme, string version)
            : this()
    {
        this.name = name;
        this.scheme = scheme;
        this.version = version;
    }

    public string Name
    {
        get
        {
            return this.name;
        }
    }

    public string Scheme
    {
        get
        {
            return this.scheme;
        }
    }

    public string Version
    {
        get
        {
            return this.version;
        }
    }
}
