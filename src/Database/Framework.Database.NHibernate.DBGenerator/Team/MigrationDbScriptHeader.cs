namespace Framework.Database.NHibernate.DBGenerator.Team;

public struct MigrationDbScriptHeader(string name, string scheme, string version)
{
    public string Name
    {
        get
        {
            return name;
        }
    }

    public string Scheme
    {
        get
        {
            return scheme;
        }
    }

    public string Version
    {
        get
        {
            return version;
        }
    }
}
