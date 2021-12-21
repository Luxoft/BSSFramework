namespace Framework.DomainDriven.NHibernate
{
    public struct SchemaDescription
    {
        public string InitialCatalog { get; private set; }
        public string DataSource { get; private set; }
        public SchemaDescription(string initialCatalog, string dataSource) : this()
        {
            this.InitialCatalog = initialCatalog;
            this.DataSource = dataSource;
        }
    }
}