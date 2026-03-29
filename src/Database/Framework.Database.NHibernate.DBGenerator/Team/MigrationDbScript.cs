namespace Framework.Database.NHibernate.DBGenerator.Team;

public struct MigrationDbScript(
    string name,
    bool runAlways,
    ApplyMigrationDbScriptMode applyCustomScriptMode,
    string scheme,
    string fileVersion,
    Lazy<string> scriptLazy)
{
    private readonly MigrationDbScriptHeader header = new(name, scheme, fileVersion);

    public MigrationDbScript(string name, bool runAlways, ApplyMigrationDbScriptMode applyCustomScriptMode, string scheme, string version, string script)
            : this(name, runAlways, applyCustomScriptMode, scheme, version, new Lazy<string>(() => script))
    {
    }

    public ApplyMigrationDbScriptMode ApplyCustomScriptMode
    {
        get
        {
            return applyCustomScriptMode;
        }
    }

    public MigrationDbScriptHeader Header
    {
        get
        {
            return this.header;
        }
    }

    public bool RunAlways
    {
        get
        {
            return runAlways;
        }
    }

    public string Script
    {
        get
        {
            return scriptLazy.Value;
        }
    }
}
