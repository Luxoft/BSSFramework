using Framework.Application.DALListener;

namespace Framework.Application.DALListeners;

public interface IDALListenerSetupObject
{
    IDALListenerSetupObject Add<TListener>()
        where TListener : class, IdalListener;
}
