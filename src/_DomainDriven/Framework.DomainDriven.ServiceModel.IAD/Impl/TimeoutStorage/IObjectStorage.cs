namespace Framework.DomainDriven.ServiceModel.IAD
{
    public interface IObjectStorage
    {
        void Push<T>(string key, T value);

        T Pop<T>(string key);
    }
}