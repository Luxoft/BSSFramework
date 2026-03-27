namespace Framework.Database.NHibernate;

public interface InHibSessionEnvironmentSettings
{
    TimeSpan TransactionTimeout { get; }
}
