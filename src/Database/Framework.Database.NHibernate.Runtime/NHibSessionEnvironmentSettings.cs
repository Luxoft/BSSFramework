namespace Framework.Database.NHibernate;

public class NHibSessionEnvironmentSettings : INHibSessionEnvironmentSettings
{
    public TimeSpan TransactionTimeout { get; } = new(0, 20, 0);
}
