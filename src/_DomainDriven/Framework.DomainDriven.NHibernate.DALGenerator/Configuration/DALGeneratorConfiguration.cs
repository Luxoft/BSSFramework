using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven.Generation.Domain;
using Framework.DomainDriven.Metadata;

using JetBrains.Annotations;

namespace Framework.DomainDriven.NHibernate.DALGenerator;

public class GeneratorConfigurationBase<TEnvironment> : GeneratorConfiguration<TEnvironment>, IGeneratorConfigurationBase<TEnvironment>
        where TEnvironment : class, IGenerationEnvironmentBase
{
    public GeneratorConfigurationBase(TEnvironment environment)
            : base(environment)
    {
        this.DatabaseName = new DatabaseName(this.Environment.TargetSystemName);
    }

    public virtual DatabaseName DatabaseName { get; }

    public virtual bool UseSmartUpdate { get; } = false;

    protected override string NamespacePostfix => "DAL.NHibernate";

    public IEnumerable<IMappingGenerator> GetMappingGenerators()
    {
        return from assembly in this.GetAssemblies() select this.GetMappingGenerator(assembly);
    }

    protected virtual IEnumerable<IAssemblyInfo> GetAssemblies()
    {
        var baseRequest = from domainType in this.DomainTypes
                          group domainType by domainType.Assembly into assemblyGroup
                          let assembly = assemblyGroup.Key
                          select new AssemblyInfo(assembly.GetName().Name, assembly.FullName, new TypeSource(assemblyGroup.ToList()));

        foreach (var assemblyInfo in baseRequest)
        {
            yield return assemblyInfo;
        }

        foreach (var projectionEnvironment in this.Environment.ProjectionEnvironments)
        {
            yield return projectionEnvironment.Assembly;
        }
    }

    protected virtual MappingGenerator CreateMappingGenerator(IAssemblyInfo assembly, AssemblyMetadata metadata) => new MappingGenerator(assembly.ToGroup(metadata.DomainTypes), this.DatabaseName, this.UseSmartUpdate);

    protected virtual AssemblyMetadata CreateAssemblyMetadata(IAssemblyInfo assembly) => MetadataReader.GetAssemblyMetadata(this.Environment.PersistentDomainObjectBaseType, assembly);

    private IMappingGenerator GetMappingGenerator([NotNull] IAssemblyInfo assembly)
    {
        if (assembly == null) throw new ArgumentNullException(nameof(assembly));

        var metadata = this.CreateAssemblyMetadata(assembly);

        return this.CreateMappingGenerator(assembly, metadata);
    }
}
