namespace Framework.Database.NHibernate;

public interface INHibSessionEnvironmentSettings
{
    TimeSpan TransactionTimeout { get; }
}
