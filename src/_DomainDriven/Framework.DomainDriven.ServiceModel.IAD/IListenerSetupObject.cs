namespace Framework.DomainDriven.ServiceModel.IAD;

public interface IListenerSetupObject
{
    IListenerSetupObject AddListener<TListener>(bool registerSelf = false)
        where TListener : class, IDALListener;
}
