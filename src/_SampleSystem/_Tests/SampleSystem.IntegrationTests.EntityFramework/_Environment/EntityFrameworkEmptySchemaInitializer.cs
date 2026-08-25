using Anch.Testing.Database.ConnectionStringManagement;

using Framework.AutomationCore.Extensions;

using SampleSystem.DbGenerate.NHibernate;

namespace SampleSystem.IntegrationTests._Environment;

public class EntityFrameworkEmptySchemaInitializer(IActualTestConnectionStringSource actualTestConnectionStringSource)
    : EmptySchemaInitializer(actualTestConnectionStringSource)
{
    public override async Task Initialize(CancellationToken ct)
    {


        await base.Initialize(ct);
    }
}
