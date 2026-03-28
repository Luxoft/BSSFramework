namespace Framework.Database.NHibernate.DBGenerator;

[Flags]
public enum ModifyMode
{
    None = 1,
    RemoveNotExistsTable = 2,
    RemoveNotExistsColumns = 4,
}
