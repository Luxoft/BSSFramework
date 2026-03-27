using Framework.Application.DALListener;

namespace Framework.Application.DependencyInjection;

public interface IDALListenerSetupObject
{
    IDALListenerSetupObject Add<TListener>()
        where TListener : class, IDALListener;
}
