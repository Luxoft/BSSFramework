namespace Framework.DomainDriven.NHibernate;

public interface INHibSessionEnvironmentSettings
{
    TimeSpan TransactionTimeout { get; }
}
