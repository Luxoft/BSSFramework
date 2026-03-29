namespace Framework.Database.NHibernate;

public record DBSessionSettings(DBSessionMode DefaultSessionMode)
{
    public static DBSessionSettings Default { get; } = new(DBSessionMode.Write);
}
