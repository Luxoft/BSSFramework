namespace Framework.Database.NHibernate.DBGenerator.Team;

public struct MigrationDbScript(
    string name,
    bool runAlways,
    ApplyMigrationDbScriptMode applyCustomScriptMode,
    string scheme,
    string fileVersion,
    Lazy<string> scriptLazy)
{
    public MigrationDbScript(string name, bool runAlways, ApplyMigrationDbScriptMode applyCustomScriptMode, string scheme, string version, string script)
            : this(name, runAlways, applyCustomScriptMode, scheme, version, new Lazy<string>(() => script))
    {
    }

    public ApplyMigrationDbScriptMode ApplyCustomScriptMode => applyCustomScriptMode;

    public MigrationDbScriptHeader Header { get; } = new(name, scheme, fileVersion);

    public bool RunAlways => runAlways;

    public string Script => scriptLazy.Value;
}
