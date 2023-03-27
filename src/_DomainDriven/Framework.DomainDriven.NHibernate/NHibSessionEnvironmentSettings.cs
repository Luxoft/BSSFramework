namespace Framework.DomainDriven.NHibernate;

public class NHibSessionEnvironmentSettings : INHibSessionEnvironmentSettings
{
    public TimeSpan TransactionTimeout { get; } = new TimeSpan(0, 20, 0);
}
