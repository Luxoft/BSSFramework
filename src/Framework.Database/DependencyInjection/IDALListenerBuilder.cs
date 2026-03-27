using Framework.Database.DALListener;

namespace Framework.Database.DependencyInjection;

public interface IDALListenerBuilder
{
    IDALListenerBuilder Add<TListener>()
        where TListener : class, IDALListener;
}
