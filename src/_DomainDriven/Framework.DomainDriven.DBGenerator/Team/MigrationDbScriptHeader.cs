namespace Framework.DomainDriven.DBGenerator.Team;

public struct MigrationDbScriptHeader
{
    private readonly string _name;
    private readonly string _scheme;
    private readonly string _version;

    public MigrationDbScriptHeader(string name, string scheme, string _version)
            : this()
    {
        this._name = name;
        this._scheme = scheme;
        this._version = _version;
    }

    public string Name
    {
        get
        {
            return this._name;
        }
    }

    public string Scheme
    {
        get
        {
            return this._scheme;
        }
    }

    public string Version
    {
        get
        {
            return this._version;
        }
    }
}
