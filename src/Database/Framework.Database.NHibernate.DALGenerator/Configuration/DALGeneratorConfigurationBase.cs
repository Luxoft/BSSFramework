using System.Reflection;

using Framework.Core;
using Framework.Database.Metadata;
using Framework.Database.NHibernate.DALGenerator._Internal;

using Framework.FileGeneration.Configuration;

namespace Framework.Database.NHibernate.DALGenerator.Configuration;

public class DALGeneratorConfigurationBase<TEnvironment> : FileGeneratorConfiguration<TEnvironment>, IDALGeneratorConfiguration<TEnvironment>
    where TEnvironment : class, IDALGenerationEnvironment
{
    public DALGeneratorConfigurationBase(TEnvironment environment)
        : base(environment) =>
        this.DatabaseName = new DatabaseName(this.Environment.TargetSystemName);

    public virtual IEscapeWordService EscapeWordService { get; } = new EmptyEscapeWordService();

    public virtual DatabaseName DatabaseName { get; }

    public virtual bool UseSmartUpdate { get; } = false;

    public IEnumerable<IMappingGenerator> GetMappingGenerators() => from assembly in this.GetAssemblies() select this.GetMappingGenerator(assembly);

    protected virtual IEnumerable<Assembly> GetAssemblies()
    {
        foreach (var assembly in this.DomainTypes.Select(dt => dt.Assembly).Distinct())
        {
            yield return assembly;
        }

        foreach (var projectionEnvironment in this.Environment.ProjectionEnvironments)
        {
            yield return projectionEnvironment.Assembly;
        }
    }

    protected virtual MappingGenerator CreateMappingGenerator(Assembly assembly, AssemblyMetadata metadata) => new MappingGenerator(
        assembly.ToGroup(metadata.DomainTypes),
        this.EscapeWordService,
        this.DatabaseName,
        this.UseSmartUpdate);

    protected virtual AssemblyMetadata CreateAssemblyMetadata(Assembly assembly) =>
        MetadataReader.GetAssemblyMetadata(this.Environment.PersistentDomainObjectBaseType, assembly);

    private IMappingGenerator GetMappingGenerator(Assembly assembly)
    {
        if (assembly == null) throw new ArgumentNullException(nameof(assembly));

        var metadata = this.CreateAssemblyMetadata(assembly);

        return this.CreateMappingGenerator(assembly, metadata);
    }
}
