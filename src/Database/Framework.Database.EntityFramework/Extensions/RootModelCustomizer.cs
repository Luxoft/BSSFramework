using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Database.EntityFramework.Extensions;

public class RootModelCustomizer(
    ModelCustomizerDependencies dependencies,
    [FromKeyedServices(RootModelCustomizer.ElementKey)] IEnumerable<IModelCustomizer> customizers) : ModelCustomizer(dependencies)
{
    public const string ElementKey = "Element";

    public override void Customize(ModelBuilder modelBuilder, DbContext context)
    {
        base.Customize(modelBuilder, context);

        foreach (var modelCustomizer in customizers)
        {
            modelCustomizer.Customize(modelBuilder, context);
        }
    }
}
