using Framework.Database.DALListener;

namespace Framework.Application.DependencyInjection;

public interface IDALListenerBuilder
{
    IDALListenerBuilder Add<TListener>()
        where TListener : class, IDALListener;
}
