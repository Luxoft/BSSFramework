using Framework.Database.EntityFramework.DependencyInjection;

namespace Framework.Database.EntityFramework;

public static class EntityFrameworkSetupObjectExtensions
{
    public static IEntityFrameworkSetup AddLegacyDatabaseSettings(this IEntityFrameworkSetup setupObject) => setupObject.AddExtension(new EntityFrameworkSetupExtension(services => services.AddLegacyEntityFrameworkSettings()));
}
