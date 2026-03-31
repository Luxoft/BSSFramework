using System.CodeDom;
using System.Reflection;

using Framework.BLL.Domain.Serialization;
using Framework.BLL.Domain.Serialization.Extensions;
using Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService;
using Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService.Base;
using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.Extensions;
using Framework.CodeGeneration.DTOGenerator.FileFactory._Helpers;
using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.CodeGeneration.DTOGenerator.PropertyAssigner.__Base;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.CodeGeneration.DTOGenerator.Server.FileFactory.__Base.ByProperty;
using Framework.CodeGeneration.DTOGenerator.Server.FileFactory._Helpers;
using Framework.CodeGeneration.DTOGenerator.Server.PropertyAssigner;
using Framework.FileGeneration.Configuration;
using Framework.Projection;

namespace Framework.CodeGeneration.DTOGenerator.Server.FileFactory;

public class DefaultProjectionDTOFileFactory<TConfiguration> : DTOFileFactory<TConfiguration, DTOFileType>
        where TConfiguration : class, IServerDTOGeneratorConfiguration<IServerDTOGenerationEnvironment>
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

    public override DTOFileType FileType { get; } = BaseFileType.ProjectionDTO;

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

    protected override IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        foreach (var baseType in base.GetBaseTypes())
        {
            yield return baseType;
        }

        if (this.IsPersistent)
        {
            if (this.Configuration.GeneratePolicy.Used(this.sourceType, BaseFileType.IdentityDTO))
            {
                yield return this.GetIdentityObjectContainerTypeReference();
            }
        }
    }

    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        foreach (var baseMember in base.GetMembers())
        {
            yield return baseMember;
        }

        if (this.IsPersistent)
        {
            if (this.Configuration.GeneratePolicy.Used(this.sourceType, BaseFileType.IdentityDTO))
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
