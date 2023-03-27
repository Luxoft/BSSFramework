using System.CodeDom;
using System.Reflection;

using Framework.Core;

namespace Framework.DomainDriven.DTOGenerator.Server;

public abstract class MainDTOFileFactory<TConfiguration> : DTOFileFactory<TConfiguration, MainDTOFileType>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    protected MainDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
        this.CodeTypeReferenceService = new MainCodeTypeReferenceService<TConfiguration>(this.Configuration);
    }


    public virtual MainDTOFileType BaseType => this.FileType.GetBaseType(false);

    public override IPropertyCodeTypeReferenceService CodeTypeReferenceService { get; }


    public override CodeTypeReference BaseReference => this.BaseType.Maybe(baseType => this.Configuration.GetCodeTypeReference(this.DomainType, baseType));


    protected override IPropertyAssigner MapDomainObjectToMappingObjectPropertyAssigner => this.Configuration.PropertyAssignerConfigurator.GetDomainObjectToSecurityPropertyAssigner(new DomainObjectToDTOPropertyAssigner<TConfiguration>(this));


    protected override bool HasToDomainObjectMethod { get; } = false;

    protected override bool HasMapToDomainObjectMethod => this.Configuration.MapToDomainRole.HasFlag(ClientDTORole.Main) && !this.FileType.IsAbstract;

    protected override IPropertyAssigner MapMappingObjectToDomainObjectPropertyAssigner
    {
        get
        {
            if (this.HasMapToDomainObjectMethod)
            {
                return this.Configuration.PropertyAssignerConfigurator.GetStrictSecurityToDomainObjectPropertyAssigner(new DTOToDomainObjectPropertyAssigner<TConfiguration>(this));
            }
            else
            {
                return null;
            }
        }
    }

    protected virtual bool ConvertToStrict { get; } = true;


    protected sealed override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration(this.Name)
               {
                       IsClass = true,
                       IsPartial = true,
                       TypeAttributes = this.FileType.IsAbstract ? TypeAttributes.Abstract | TypeAttributes.Public : TypeAttributes.Public
               };
    }

    protected override IEnumerable<CodeAttributeDeclaration> GetCustomAttributes()
    {
        foreach (var customAttribute in base.GetCustomAttributes())
        {
            yield return customAttribute;
        }

        if (!this.FileType.IsAbstract)
        {
            foreach (var knownTypesAttribute in this.Configuration.GetKnownTypesAttributes(this.FileType, this.DomainType))
            {
                yield return knownTypesAttribute;
            }
        }
    }

    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        foreach (var baseMember in base.GetMembers())
        {
            yield return baseMember;
        }

        if (!this.FileType.IsAbstract)
        {
            if (this.ConvertToStrict)
            {
                if (this.Configuration.GeneratePolicy.Used(this.DomainType, DTOGenerator.FileType.StrictDTO))
                {
                    yield return this.GenerateConvertMethod(DTOGenerator.FileType.StrictDTO);
                }
            }

            if (this.HasMapToDomainObjectMethod)
            {
                yield return new MainMapToDomainObjectMethodFactory<TConfiguration, MainDTOFileFactory<TConfiguration>>(this).GetMethod();
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
