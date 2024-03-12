namespace Framework.DomainDriven.ServiceModel.IAD;

public interface IListenerSetupObject
{
    IListenerSetupObject Add<TListener>(bool registerSelf = false)
        where TListener : class, IDALListener;
}
