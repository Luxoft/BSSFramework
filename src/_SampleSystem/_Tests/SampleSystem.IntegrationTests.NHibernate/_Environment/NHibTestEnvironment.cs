using Anch.Testing.Database.DependencyInjection;
using Anch.Testing.Xunit;

using Framework.Infrastructure.DependencyInjection;

using SampleSystem.IntegrationTests._Environment;
using SampleSystem.ServiceEnvironment.DependencyInjection;

[assembly: AnchTestFramework<NHibTestEnvironment>]

namespace SampleSystem.IntegrationTests._Environment;

public class NHibTestEnvironment : TestEnvironment
{
    protected override void SetInitializers(IDatabaseTestingSetup setup)
    {
        setup.SetEmptySchemaInitializer<NHibEmptySchemaInitializer>();

        base.SetInitializers(setup);
    }

    protected override IBssFrameworkExtension BssFrameworkExtension { get; } = new SampleSystemNHibernateExtension();
}
