using System;
using System.Collections.Generic;
using System.Linq;

using Framework.DomainDriven.Generation;
using Framework.DomainDriven.Generation.Domain;
using Framework.Projection;
using Framework.Security;

namespace Framework.DomainDriven.DTOGenerator
{
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

        protected virtual ICodeFileFactory<RoleFileType> GetDomainObjectSecurityOperationCodeFileFactory(Type domainType, IEnumerable<Enum> securityOperations)
        {
            if (domainType == null) throw new ArgumentNullException(nameof(domainType));
            if (securityOperations == null) throw new ArgumentNullException(nameof(securityOperations));

            return new DomainObjectSecurityOperationCodeFileFactory<TConfiguration>(this.Configuration, domainType, securityOperations);
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
            foreach (var fileFactory in this.GetDomainObjectSecurityOperationCodeFileGenerators())
            {
                yield return fileFactory;
            }

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


        private IEnumerable<ICodeFileFactory<RoleFileType>> GetDomainObjectSecurityOperationCodeFileGenerators()
        {
            if (this.Configuration.Environment.SecurityOperationCodeType.IsEnum)
            {
                foreach (var pair in this.Configuration.TypesWithSecondarySecurityOperations)
                {
                    if (!pair.Key.IsProjection())
                    {
                        yield return this.GetDomainObjectSecurityOperationCodeFileFactory(pair.Key, pair.Value);
                    }
                }
            }
        }
    }
}
