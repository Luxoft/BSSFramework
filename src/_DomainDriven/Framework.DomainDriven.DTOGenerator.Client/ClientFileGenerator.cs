using System.Collections.Generic;
using Framework.DomainDriven.Generation;
using Framework.DomainDriven.Generation.Domain;
using Framework.DomainDriven.Serialization;
using Framework.Projection;

namespace Framework.DomainDriven.DTOGenerator.Client
{
    public class ClientFileGenerator : ClientFileGenerator<IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase>>
    {
        public ClientFileGenerator(IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase> configuration)
            : base(configuration)
        {
        }
    }

    public class ClientFileGenerator<TConfiguration> : FileGenerator<TConfiguration>
        where TConfiguration : class, IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase>
    {
        public ClientFileGenerator(TConfiguration configuration)
            : base(configuration)
        {
        }


        protected override IEnumerable<ICodeFileFactory<DTOFileType>> GetDTOFileGenerators()
        {
            //--------------------------------------------------------------------------------
            foreach (var baseFileGenerator in base.GetDTOFileGenerators())
            {
                yield return baseFileGenerator;
            }
            //--------------------------------------------------------------------------------
            {
                yield return new DefaultBaseAbstractInterfaceDTOFileFactory<TConfiguration>(this.Configuration);
                yield return new DefaultBasePersistentInterfaceDTOFileFactory<TConfiguration>(this.Configuration);
                yield return new DefaultBaseAuditPersistentInterfaceDTOFileFactory<TConfiguration>(this.Configuration);

                yield return new DefaultBaseAbstractDTOFileFactory<TConfiguration>(this.Configuration);
                yield return new DefaultBasePersistentDTOFileFactory<TConfiguration>(this.Configuration);
                yield return new DefaultBaseAuditPersistentDTOFileFactory<TConfiguration>(this.Configuration);

                foreach (var domainType in this.Configuration.DomainTypes)
                {
                    if (domainType.IsProjection())
                    {
                        yield return new DefaultProjectionDTOFileFactory<TConfiguration>(this.Configuration, domainType);
                    }
                    else
                    {
                        if (domainType.HasVisualIdentityProperties())
                        {
                            yield return new DefaultVisualDTOFileFactory<TConfiguration>(this.Configuration, domainType);
                        }

                        yield return new DefaultSimpleDTOFileFactory<TConfiguration>(this.Configuration, domainType);
                        yield return new DefaultFullDTOFileFactory<TConfiguration>(this.Configuration, domainType);
                        yield return new DefaultRichDTOFileFactory<TConfiguration>(this.Configuration, domainType);

                        yield return new DefaultStrictDTOFileFactory<TConfiguration>(this.Configuration, domainType);

                        if (this.Configuration.IsPersistentObject(domainType))
                        {
                            yield return new DefaultUpdateDTOFileFactory<TConfiguration>(this.Configuration, domainType);
                        }

                        yield return new DefaultSimpleInterfaceDTOFileFactory<TConfiguration>(this.Configuration, domainType);
                        yield return new DefaultFullInterfaceDTOFileFactory<TConfiguration>(this.Configuration, domainType);
                        yield return new DefaultRichInterfaceDTOFileFactory<TConfiguration>(this.Configuration, domainType);
                    }
                }
            }
            //--------------------------------------------------------------------------------

            foreach (var domainType in this.Configuration.ClassTypes)
            {
                yield return new DefaultClassFileFactory<TConfiguration>(this.Configuration, domainType);
            }

            foreach (var domainType in this.Configuration.StructTypes)
            {
                yield return new DefaultStructFileFactory<TConfiguration>(this.Configuration, domainType);
            }
        }

        protected override IEnumerable<ICodeFileFactory<RoleFileType>> GetRoleFileGenerators()
        {
            foreach (var fileGenerator in base.GetRoleFileGenerators())
            {
                yield return fileGenerator;
            }

            foreach (var type in this.Configuration.EnumTypes)
            {
                yield return new DefaultEnumFileFactory<TConfiguration>(this.Configuration, type);
            }
        }
    }
}
