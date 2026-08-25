using Anch.Testing.Database.DependencyInjection;
using Anch.Testing.Xunit;

using Framework.Infrastructure.DependencyInjection;

using SampleSystem.IntegrationTests._Environment;
using SampleSystem.ServiceEnvironment.DependencyInjection;

[assembly: AnchTestFramework<EntityFrameworkTestEnvironment>]

namespace SampleSystem.IntegrationTests._Environment;

public class EntityFrameworkTestEnvironment : TestEnvironment
{
    protected override void SetInitializers(IDatabaseTestingSetup setup)
    {
        setup.SetEmptySchemaInitializer<EntityFrameworkEmptySchemaInitializer>();

        base.SetInitializers(setup);
    }

    protected override IBssFrameworkExtension BssFrameworkExtension { get; } = new SampleSystemEntityFrameworkExtension();
}
