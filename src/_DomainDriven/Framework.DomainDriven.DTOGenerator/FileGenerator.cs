using Framework.DomainDriven.Generation;
using Framework.DomainDriven.Generation.Domain;
using Framework.Projection;

namespace Framework.DomainDriven.DTOGenerator;

public abstract class FileGenerator<TConfiguration> : CodeFileGenerator<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    protected FileGenerator(TConfiguration configuration)
            : base(configuration)
    {
    }



    protected virtual ICodeFileFactory<DTOFileType> GetIdentityDTOFileFactory(Type domainType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        return new DefaultIdentityDTOFileFactory<TConfiguration>(this.Configuration, domainType);
    }

    protected override IEnumerable<ICodeFile> GetInternalFileGenerators()
    {
        var generators = this.GetRoleFileGenerators().Where(fileGenerator => this.Configuration.GeneratePolicy.Used(fileGenerator.DomainType, fileGenerator.FileType)).ToList();

        var methodGenerators = generators.OfType<IClientMappingServiceExternalMethodGenerator>().ToList();

        foreach (var clientMappingGenerator in this.GetClientMappingFileGenerators(methodGenerators))
        {
            yield return clientMappingGenerator;
        }

        foreach (var generator in generators)
        {
            yield return generator;
        }
    }

    protected virtual IEnumerable<ICodeFile> GetClientMappingFileGenerators(IEnumerable<IClientMappingServiceExternalMethodGenerator> methodGenerators)
    {
        if (methodGenerators == null) throw new ArgumentNullException(nameof(methodGenerators));

        var methodGeneratorsCache = methodGenerators.ToList();

        yield return new ClientDTOMappingServiceInterfaceFileFactory<TConfiguration>(this.Configuration, methodGeneratorsCache);
        yield return new ClientPrimitiveDTOMappingServiceBaseFileFactory<TConfiguration>(this.Configuration, methodGeneratorsCache);
        yield return new ClientPrimitiveDTOMappingServiceFileFactory<TConfiguration>(this.Configuration);
    }


    protected virtual IEnumerable<ICodeFileFactory<RoleFileType>> GetRoleFileGenerators()
    {
        foreach (var fileFactory in this.GetDTOFileGenerators())
        {
            yield return fileFactory;
        }
    }

    protected virtual IEnumerable<ICodeFileFactory<DTOFileType>> GetDTOFileGenerators()
    {
        foreach (var domainType in this.Configuration.DomainTypes)
        {
            if (!domainType.IsProjection())
            {
                if (this.Configuration.IsPersistentObject(domainType))
                {
                    yield return this.GetIdentityDTOFileFactory(domainType);
                }
            }
        }
    }
}
