namespace Framework.DomainDriven.ApplicationCore.DALListeners;

public interface IDALListenerSetupObject
{
    IDALListenerSetupObject Add<TListener>()
        where TListener : class, IDALListener;
}
