namespace Framework.DomainDriven.DBGenerator;

[Flags]
public enum DatabaseScriptGeneratorMode
{
    None,
    AutoGenerateUpdateChangeTypeScript,
    RemoveObsoleteColumns
}
