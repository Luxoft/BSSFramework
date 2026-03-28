namespace Framework.Database.NHibernate.DBGenerator.Team;

public struct MigrationDbScript
{
    private readonly ApplyMigrationDbScriptMode applyMigrationDbScriptMode;
    private readonly MigrationDbScriptHeader header;

    private readonly bool runAlways;
    private readonly Lazy<string> scriptLazy;

    public MigrationDbScript(
            string name,
            bool runAlways,
            ApplyMigrationDbScriptMode applyCustomScriptMode,
            string scheme,
            string fileVersion,
            Lazy<string> scriptLazy) : this()
    {
        this.header = new MigrationDbScriptHeader(name, scheme, fileVersion);

        this.runAlways = runAlways;
        this.applyMigrationDbScriptMode = applyCustomScriptMode;
        this.scriptLazy = scriptLazy;

    }

    public MigrationDbScript(string name, bool runAlways, ApplyMigrationDbScriptMode applyCustomScriptMode, string scheme, string version, string script)
            : this(name, runAlways, applyCustomScriptMode, scheme, version, new Lazy<string>(() => script))
    {
    }

    public ApplyMigrationDbScriptMode ApplyCustomScriptMode
    {
        get
        {
            return this.applyMigrationDbScriptMode;
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
            return this.runAlways;
        }
    }

    public string Script
    {
        get
        {
            return this.scriptLazy.Value;
        }
    }
}
