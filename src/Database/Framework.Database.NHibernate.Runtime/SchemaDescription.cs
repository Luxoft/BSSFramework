namespace Framework.Database.NHibernate;

public struct SchemaDescription(string initialCatalog, string dataSource)
{
    public string InitialCatalog { get; private set; } = initialCatalog;

    public string DataSource { get; private set; } = dataSource;
}
