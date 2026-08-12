namespace Framework.Database.NHibernate;

public record NHibSessionEnvironmentSettings(TimeSpan TransactionTimeout)
{
    public static NHibSessionEnvironmentSettings Default { get; } = new(new TimeSpan(0, 20, 0));
}
