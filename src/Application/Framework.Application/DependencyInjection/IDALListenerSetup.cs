using Framework.Database.DALListener;

namespace Framework.Application.DependencyInjection;

public interface IDALListenerSetup
{
    IDALListenerSetup Add<TListener>()
        where TListener : class, IDALListener;
}
