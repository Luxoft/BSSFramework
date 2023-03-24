using System;
using System.Collections.Generic;
using System.Linq;

using Framework.CodeDom;
using Framework.CodeDom.TypeScript;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Base;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Custom;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Interfaces;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main.Observable;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Visual;
using Framework.DomainDriven.Generation;
using Framework.DomainDriven.Generation.Domain;
using Framework.DomainDriven.Serialization;
using Framework.Projection;

namespace Framework.DomainDriven.DTOGenerator.TypeScript;

/// <summary>
/// TypeScript file generator
/// </summary>
public class TypeScriptDTOFileGenerator : TypeScriptDTOFileGenerator<ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>>
{
    public TypeScriptDTOFileGenerator(ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase> configuration)
            : base(configuration)
    {
    }
}

/// <summary>
/// TypeScript file generator
/// </summary>
/// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
public class TypeScriptDTOFileGenerator<TConfiguration> : FileGenerator<TConfiguration>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
{
    public TypeScriptDTOFileGenerator(TConfiguration configuration)
            : base(configuration)
    {
    }

    public override CodeDomRenderer Renderer { get; } = TypeScriptCodeDomRenderer.Default;

    protected override ICodeFileFactory<DTOFileType> GetIdentityDTOFileFactory(Type domainType)
    {
        return new FileFactory.DefaultIdentityDTOFileFactory<TConfiguration>(this.Configuration, domainType);
    }

    protected override ICodeFileFactory<RoleFileType> GetDomainObjectSecurityOperationCodeFileFactory(Type domainType, IEnumerable<Enum> securityOperations)
    {
        return new ClientDomainObjectSecurityOperationCodeFileFactory<TConfiguration>(this.Configuration, domainType, securityOperations);
    }

    protected override IEnumerable<ICodeFileFactory<DTOFileType>> GetDTOFileGenerators()
    {
        foreach (var baseFile in base.GetDTOFileGenerators())
        {
            yield return baseFile;
        }

        yield return new DefaultBaseObservableAbstractDTOFileFactory<TConfiguration>(this.Configuration);

        // --------------------------------------------------------------------------------
        yield return new DefaultBaseObservablePersistentDTOFileFactory<TConfiguration>(this.Configuration);
        yield return new DefaultBaseObservableAuditPersistentDTOFileFactory<TConfiguration>(this.Configuration);

        foreach (var type in this.Configuration.DomainTypes)
        {
            if (!type.IsProjection())
            {
                yield return new DefaultObservableIdentityDTOFileFactory<TConfiguration>(this.Configuration, type);
                yield return new DefaultObservableSimpleDTOFileFactory<TConfiguration>(this.Configuration, type);
                yield return new DefaultObservableFullDTOFileFactory<TConfiguration>(this.Configuration, type);
                yield return new DefaultObservableRichDTOFileFactory<TConfiguration>(this.Configuration, type);

                if (type.HasVisualIdentityProperties())
                {
                    yield return new DefaultObservableVisualDTOFileFactory<TConfiguration>(this.Configuration, type);
                }
            }
            else
            {
                yield return new DefaultObservableProjectionDTOFileFactory<TConfiguration>(this.Configuration, type);
            }
        }


        yield return new DefaultBaseAbstractInterfaceDTOFileFactory<TConfiguration>(this.Configuration);
        yield return new DefaultBasePersistentInterfaceDTOFileFactory<TConfiguration>(this.Configuration);
        yield return new DefaultBaseAuditPersistentInterfaceDTOFileFactory<TConfiguration>(this.Configuration);

        //--------------------------------------------------------------------------------
        yield return new DefaultBaseAbstractDTOFileFactory<TConfiguration>(this.Configuration);
        yield return new DefaultBasePersistentDTOFileFactory<TConfiguration>(this.Configuration);
        yield return new DefaultBaseAuditPersistentDTOFileFactory<TConfiguration>(this.Configuration);

        foreach (var type in this.Configuration.DomainTypes.Where(x => !x.IsProjection()))
        {
            yield return new DefaultSimpleDTOFileFactory<TConfiguration>(this.Configuration, type);
            yield return new DefaultFullDTOFileFactory<TConfiguration>(this.Configuration, type);
            yield return new DefaultRichDTOFileFactory<TConfiguration>(this.Configuration, type);

            // TODO: fix
            if (!this.Configuration.Environment.PersistentDomainObjectBaseType.IsAssignableFrom(type))
            {
                yield return new FileFactory.DefaultIdentityDTOFileFactory<TConfiguration>(this.Configuration, type);
            }
        }

        foreach (var type in this.Configuration.DomainTypes.Where(x => !x.IsProjection()))
        {
            yield return new DefaultSimpleInterfaceDTOFileFactory<TConfiguration>(this.Configuration, type);
            yield return new DefaultFullInterfaceDTOFileFactory<TConfiguration>(this.Configuration, type);
            yield return new DefaultRichInterfaceDTOFileFactory<TConfiguration>(this.Configuration, type);

            if (type.HasVisualIdentityProperties())
            {
                yield return new DefaultVisualDTOFileFactory<TConfiguration>(this.Configuration, type);
            }
        }

        //--------------------------------------------------------------------------------
        foreach (var type in this.Configuration.DomainTypes.Where(x => !x.IsProjection()))
        {
            yield return new DefaultStrictDTOFileFactory<TConfiguration>(this.Configuration, type);

            if (this.Configuration.IsPersistentObject(type))
            {
                yield return new DefaultUpdateDTOFileFactory<TConfiguration>(this.Configuration, type);
            }
        }

        foreach (var type in this.Configuration.ClassTypes)
        {
            yield return new DefaultClassFileFactory<TConfiguration>(this.Configuration, type);
        }

        foreach (var type in this.Configuration.StructTypes)
        {
            yield return new DefaultStructFileFactory<TConfiguration>(this.Configuration, type);
        }

        foreach (var type in this.Configuration.DomainTypes.Where(x => x.IsProjection()))
        {
            yield return new DefaultProjectionDTOFileFactory<TConfiguration>(this.Configuration, type);
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

    protected override IEnumerable<ICodeFile> GetClientMappingFileGenerators(IEnumerable<IClientMappingServiceExternalMethodGenerator> methodGenerators)
    {
        if (this.Configuration.GenerateClientMappingService)
        {
            var methodGeneratorsCache = methodGenerators.ToList();

            yield return new ClientDTOMappingServiceInterfaceFileFactory<TConfiguration>(this.Configuration, methodGeneratorsCache);
            yield return new TypeScriptClientPrimitiveDTOMappingServiceFileFactory<TConfiguration>(this.Configuration, methodGeneratorsCache);
        }
    }
}
