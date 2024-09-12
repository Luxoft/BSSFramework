namespace Framework.DomainDriven.ServiceModel.IAD;

public interface IListenerSetupObject
{
    IListenerSetupObject Add<TListener>()
        where TListener : class, IDALListener;
}
