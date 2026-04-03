namespace Framework.Database.NHibernate.DBGenerator;

[Flags]
public enum DatabaseScriptGeneratorMode
{
    None,
    AutoGenerateUpdateChangeTypeScript,
    RemoveObsoleteColumns
}
