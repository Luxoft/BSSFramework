namespace Framework.DomainDriven.DBGenerator.Team;

public struct MigrationDbScript
{
    private readonly ApplyMigrationDbScriptMode _applyMigrationDbScriptMode;
    private readonly MigrationDbScriptHeader _header;

    private readonly bool _runAlways;
    private readonly Lazy<string> _scriptLazy;

    public MigrationDbScript(
            string name,
            bool runAlways,
            ApplyMigrationDbScriptMode applyCustomScriptMode,
            string scheme,
            string fileVersion,
            Lazy<string> scriptLazy) : this()
    {
        this._header = new MigrationDbScriptHeader(name, scheme, fileVersion);

        this._runAlways = runAlways;
        this._applyMigrationDbScriptMode = applyCustomScriptMode;
        this._scriptLazy = scriptLazy;

    }

    public MigrationDbScript(string name, bool runAlways, ApplyMigrationDbScriptMode applyCustomScriptMode, string scheme, string version, string script)
            : this(name, runAlways, applyCustomScriptMode, scheme, version, new Lazy<string>(() => script))
    {
    }

    public ApplyMigrationDbScriptMode ApplyCustomScriptMode
    {
        get
        {
            return this._applyMigrationDbScriptMode;
        }
    }

    public MigrationDbScriptHeader Header
    {
        get
        {
            return this._header;
        }
    }

    public bool RunAlways
    {
        get
        {
            return this._runAlways;
        }
    }

    public string Script
    {
        get
        {
            return this._scriptLazy.Value;
        }
    }
}
