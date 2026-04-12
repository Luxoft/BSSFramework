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

    protected virtual IEnumerable<IAssemblyInfo> GetAssemblies()
    {
        var baseRequest = from domainType in this.DomainTypes
                          group domainType by domainType.Assembly
                          into assemblyGroup
                          let assembly = assemblyGroup.Key
                          select new AssemblyInfo(assembly.GetName().Name!, assembly.FullName!, new TypeSource([.. assemblyGroup]));

        foreach (var assemblyInfo in baseRequest)
        {
            yield return assemblyInfo;
        }

        foreach (var projectionEnvironment in this.Environment.ProjectionEnvironments)
        {
            yield return projectionEnvironment.Assembly;
        }
    }

    protected virtual MappingGenerator CreateMappingGenerator(IAssemblyInfo assembly, AssemblyMetadata metadata) => new MappingGenerator(
        assembly.ToGroup(metadata.DomainTypes),
        this.EscapeWordService,
        this.DatabaseName,
        this.UseSmartUpdate);

    protected virtual AssemblyMetadata CreateAssemblyMetadata(IAssemblyInfo assembly) =>
        MetadataReader.GetAssemblyMetadata(this.Environment.PersistentDomainObjectBaseType, assembly);

    private IMappingGenerator GetMappingGenerator(IAssemblyInfo assembly)
    {
        if (assembly == null) throw new ArgumentNullException(nameof(assembly));

        var metadata = this.CreateAssemblyMetadata(assembly);

        return this.CreateMappingGenerator(assembly, metadata);
    }
}
