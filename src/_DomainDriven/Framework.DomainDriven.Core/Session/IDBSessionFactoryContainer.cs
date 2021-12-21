namespace Framework.DomainDriven.BLL
{
    public interface IDBSessionFactoryContainer
    {
        IDBSessionFactory SessionFactory { get; }
    }
}