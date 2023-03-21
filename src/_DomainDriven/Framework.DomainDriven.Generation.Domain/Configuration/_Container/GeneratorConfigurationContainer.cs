using System;

namespace Framework.DomainDriven.Generation.Domain;

public class GeneratorConfigurationContainer<TConfiguration> : IGeneratorConfigurationContainer<TConfiguration>
        where TConfiguration : class
{
    public GeneratorConfigurationContainer(TConfiguration configuration)
    {
        this.Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }


    public TConfiguration Configuration { get; }
}
