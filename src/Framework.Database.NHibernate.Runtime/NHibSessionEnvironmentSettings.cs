namespace Framework.Database.NHibernate;

public class NHibSessionEnvironmentSettings : InHibSessionEnvironmentSettings
{
    public TimeSpan TransactionTimeout { get; } = new TimeSpan(0, 20, 0);
}
