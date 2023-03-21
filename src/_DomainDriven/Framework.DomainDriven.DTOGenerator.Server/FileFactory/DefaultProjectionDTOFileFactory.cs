using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Framework.DomainDriven.Generation.Domain;
using Framework.DomainDriven.Serialization;
using Framework.Projection;

namespace Framework.DomainDriven.DTOGenerator.Server;

public class DefaultProjectionDTOFileFactory<TConfiguration> : DTOFileFactory<TConfiguration, DTOFileType>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    private readonly Type sourceType;

    private readonly bool ignoreIdProp;

    public DefaultProjectionDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
        this.CodeTypeReferenceService = new ProjectionCodeTypeReferenceService<TConfiguration>(this.Configuration);

        this.sourceType = this.DomainType.GetProjectionSourceType();

        this.ignoreIdProp = domainType.GetProperties().Any(prop => this.Configuration.IsIdentityProperty(prop) && prop.IsIgnored(DTORole.Client));
    }

    public override IPropertyCodeTypeReferenceService CodeTypeReferenceService { get; }

    public override DTOFileType FileType { get; } = DTOGenerator.FileType.ProjectionDTO;

    public override CodeTypeReference BaseReference =>

            this.IsPersistent ? this.Configuration.GetBasePersistentReference() : this.Configuration.GetBaseAbstractReference();

    private bool IsPersistent => this.IsPersistent() && !this.ignoreIdProp;

    protected override IPropertyAssigner MapDomainObjectToMappingObjectPropertyAssigner => this.Configuration.PropertyAssignerConfigurator.GetDomainObjectToSecurityPropertyAssigner(new DomainObjectToDTOPropertyAssigner<TConfiguration>(this));

    protected sealed override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration(this.Name)
               {
                       IsClass = true,
                       IsPartial = true,
                       TypeAttributes = TypeAttributes.Public
               };
    }

    protected override System.Collections.Generic.IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        foreach (var baseType in base.GetBaseTypes())
        {
            yield return baseType;
        }

        if (this.IsPersistent)
        {
            if (this.Configuration.GeneratePolicy.Used(this.sourceType, DTOGenerator.FileType.IdentityDTO))
            {
                yield return this.GetIdentityObjectContainerTypeReference();
            }
        }
    }

    protected override System.Collections.Generic.IEnumerable<CodeTypeMember> GetMembers()
    {
        foreach (var baseMember in base.GetMembers())
        {
            yield return baseMember;
        }

        if (this.IsPersistent)
        {
            if (this.Configuration.GeneratePolicy.Used(this.sourceType, DTOGenerator.FileType.IdentityDTO))
            {
                yield return this.GetIdentityObjectContainerImplementation();
            }
        }
    }

    protected override IEnumerable<CodeConstructor> GetConstructors()
    {
        foreach (var baseCtor in base.GetConstructors())
        {
            yield return baseCtor;
        }

        yield return this.GenerateDefaultConstructor();

        yield return this.GenerateFromDomainObjectConstructor(this.MapDomainObjectToMappingObjectPropertyAssigner);
    }
}
