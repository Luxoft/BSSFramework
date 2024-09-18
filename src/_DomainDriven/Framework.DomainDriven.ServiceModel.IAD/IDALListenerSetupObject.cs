namespace Framework.DomainDriven.ServiceModel.IAD;

public interface IDALListenerSetupObject
{
    IDALListenerSetupObject Add<TListener>()
        where TListener : class, IDALListener;
}
