using System.CodeDom;
using System.Reflection;
using System.Runtime.Serialization;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.Generation.Domain;
using Framework.Persistent;

namespace Framework.DomainDriven.DTOGenerator.Server;

public interface IDTOFileFactory<out TConfiguration, out TFileType> : IFileFactory<TConfiguration, TFileType>, IDTOSource<TConfiguration>, IServerMappingServiceExternalMethodGenerator
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
        where TFileType : DTOFileType
{
}

public abstract class DTOFileFactory<TConfiguration, TFileType> : FileFactory<TConfiguration, TFileType>, IDTOFileFactory<TConfiguration, TFileType>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
        where TFileType : DTOFileType
{
    protected DTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
    }


    protected virtual IPropertyAssigner MapDomainObjectToMappingObjectPropertyAssigner { get; }

    protected virtual IPropertyAssigner MapMappingObjectToDomainObjectPropertyAssigner { get; }


    protected virtual bool HasMapToDomainObjectMethod { get; }

    protected virtual bool HasToDomainObjectMethod => this.HasMapToDomainObjectMethod;

    protected virtual bool AllowCreate { get; } = true;


    public IEnumerable<PropertyInfo> GetProperties(bool isWritable)
    {
        return this.Configuration.GetDomainTypeProperties(this.DomainType, this.FileType, isWritable);
    }


    public virtual IEnumerable<CodeMemberMethod> GetServerMappingServiceInterfaceMethods()
    {
        var domainTypeRef = this.DomainType.ToTypeReference();
        var mappingTypeRef = this.CurrentReference;

        if (this.MapDomainObjectToMappingObjectPropertyAssigner != null)
        {
            yield return new CodeMemberMethod
                         {
                                 Name = "Map" + this.DomainType.Name,
                                 ReturnType = typeof(void).ToTypeReference(),
                                 Parameters =
                                 {
                                         domainTypeRef.ToParameterDeclarationExpression("domainObject"),
                                         mappingTypeRef.ToParameterDeclarationExpression("mappingObject"),
                                 }
                         };
        }

        if (this.MapMappingObjectToDomainObjectPropertyAssigner != null)
        {
            yield return new CodeMemberMethod
                         {
                                 Name = "Map" + this.DomainType.Name,
                                 ReturnType = typeof(void).ToTypeReference(),
                                 Parameters =
                                 {
                                         mappingTypeRef.ToParameterDeclarationExpression("mappingObject"),
                                         domainTypeRef.ToParameterDeclarationExpression("domainObject"),
                                 }
                         };
        }

        if (this.HasToDomainObjectMethod)
        {
            if (this.IsPersistent())
            {
                yield return this.GetMappingServiceInterfaceToDomainObjectMethod();

                if (this.AllowCreate && this.DomainType.HasDefaultConstructor())
                {
                    yield return this.GetMappingServiceInterfaceToDomainObjectWithAllowCreateMethod();
                }
            }
            else
            {
                if (this.DomainType.HasDefaultConstructor())
                {
                    yield return this.GetMappingServiceInterfaceToDomainObjectMethod();
                }
            }
        }
    }


    public virtual IEnumerable<CodeMemberMethod> GetServerMappingServiceMethods()
    {
        var domainTypeParameter = this.DomainType.ToTypeReference().ToParameterDeclarationExpression("domainObject");
        var domainTypeParameterRef = domainTypeParameter.ToVariableReferenceExpression();

        var mappingTypeParameter = this.CurrentReference.ToParameterDeclarationExpression("mappingObject");
        var mappingTypeParameterRef = mappingTypeParameter.ToVariableReferenceExpression();

        if (this.MapDomainObjectToMappingObjectPropertyAssigner != null)
        {
            var assignStatements = this.GetProperties(false).Select(property =>

                                                                            this.MapDomainObjectToMappingObjectPropertyAssigner.GetAssignStatementBySource(property, domainTypeParameterRef, mappingTypeParameterRef));

            yield return new CodeMemberMethod
                         {
                                 Attributes = MemberAttributes.Public,
                                 Name = "Map" + this.DomainType.Name,
                                 Parameters = { domainTypeParameter, mappingTypeParameter },
                                 Statements = { assignStatements.Composite() }
                         };
        }

        if (this.MapMappingObjectToDomainObjectPropertyAssigner != null)
        {
            var properties = from property in this.GetProperties(true)

                             orderby !property.HasAttribute<VersionAttribute>(),

                                     this.CodeTypeReferenceService.IsOptional(property),

                                     property.GetCustomAttribute<MappingPriorityAttribute>().Maybe(attr => attr.Value)

                             select property;



            yield return new CodeMemberMethod
                         {
                                 Attributes = MemberAttributes.Public,
                                 Name = "Map" + this.DomainType.Name,
                                 Parameters = { mappingTypeParameter, domainTypeParameter },
                                 Statements =
                                 {
                                         properties.Select(property => this.MapMappingObjectToDomainObjectPropertyAssigner.GetAssignStatementBySource(property, mappingTypeParameterRef, domainTypeParameterRef)).Composite(),
                                 }
                         };
        }

        if (this.HasToDomainObjectMethod)
        {
            if (this.IsPersistent())
            {
                yield return this.GetMappingServiceToDomainObjectMethod();

                if (this.AllowCreate && this.DomainType.HasDefaultConstructor())
                {
                    yield return this.GetMappingServiceToDomainObjectWithAllowCreateMethod();
                }
            }
            else
            {
                if (this.DomainType.HasDefaultConstructor())
                {
                    yield return this.GetMappingServiceToDomainObjectMethod();
                }
            }
        }
    }

    protected override IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        foreach (var baseType in base.GetBaseTypes())
        {
            yield return baseType;
        }

        if (this.HasMapToDomainObjectMethod)
        {
            yield return this.GetMappingObjectReference();
        }


        if (this.HasToDomainObjectMethod)
        {
            if (this.IsPersistent() || this.DomainType.HasDefaultConstructor())
            {
                var mappingServiceType = this.Configuration.GetCodeTypeReference(null, ServerFileType.ServerDTOMappingServiceInterface);

                yield return typeof(IConvertMappingObject<,>).ToTypeReference(mappingServiceType, this.DomainType.ToTypeReference());
            }
        }
    }


    protected override IEnumerable<CodeAttributeDeclaration> GetCustomAttributes()
    {
        foreach (var customAttribute in base.GetCustomAttributes())
        {
            yield return customAttribute;
        }

        yield return this.GetDataContractCodeAttributeDeclaration();
        yield return this.Configuration.GetDTOFileAttribute(this.DomainType, this.FileType);
    }

    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        foreach (var baseMember in base.GetMembers())
        {
            yield return baseMember;
        }

        foreach (var member in this.CreatePropertyMembers())
        {
            yield return member;
        }

        if (this.HasToDomainObjectMethod)
        {
            if (this.IsPersistent())
            {
                yield return this.GetToDomainObjectMethod();

                if (this.AllowCreate && this.DomainType.HasDefaultConstructor())
                {
                    yield return this.GetToDomainObjectWithAllowCreateMethod();
                }
            }
            else
            {
                if (this.DomainType.HasDefaultConstructor())
                {
                    yield return this.GetToDomainObjectMethod();
                }
            }
        }
    }


    private IEnumerable<CodeTypeMember> CreatePropertyMembers()
    {
        return from sourceProperty in this.GetProperties(false)

               from member in this.CreatePropertyMember(sourceProperty)

               select member;
    }


    protected virtual CodeMemberField CreateFieldMember(PropertyInfo property, string fieldName)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));
        if (fieldName == null) throw new ArgumentNullException(nameof(fieldName));

        var fieldTypeRef = this.CodeTypeReferenceService.GetCodeTypeReference(property, true);

        return new CodeMemberField
               {
                       Name = fieldName,
                       Type = fieldTypeRef,
                       InitExpression = this.GetFieldInitExpression(fieldTypeRef, property)
               };
    }

    protected virtual CodeExpression GetFieldInitExpression(CodeTypeReference codeTypeReference, PropertyInfo property)
    {
        return null;
    }



    protected virtual CodeMemberProperty CreatePropertyMember(PropertyInfo sourceProperty, CodeMemberField fieldMember)
    {
        var preSetVariableName = "newValue";

        var fieldMemberRef = new CodeThisReferenceExpression().ToFieldReference(fieldMember);

        return new CodeMemberProperty
               {
                       Type = fieldMember.Type,
                       Name = sourceProperty.Name,
                       Attributes = MemberAttributes.Public | MemberAttributes.Final,
                       GetStatements = { fieldMemberRef.ToMethodReturnStatement() },
                       SetStatements = { new CodePropertySetValueReferenceExpression().ToAssignStatement(fieldMemberRef) }
               }.Self(p => p.CustomAttributes.AddRange(this.GetPropertyCustomAttributes(sourceProperty).ToArray()));
    }


    private IEnumerable<CodeTypeMember> CreatePropertyMember(PropertyInfo sourceProperty)
    {
        if (sourceProperty == null) throw new ArgumentNullException(nameof(sourceProperty));

        if (!this.Configuration.ForceGenerateProperties(this.DomainType, this.FileType))
        {
            yield return this.CreateFieldMember(sourceProperty, sourceProperty.Name)
                             .Self(field => field.CustomAttributes.Add(typeof(DataMemberAttribute).ToTypeReference().ToAttributeDeclaration()))
                             .Self(field => field.Attributes = MemberAttributes.Public);
        }
        else
        {
            var fieldMember = this.CreateFieldMember(sourceProperty, "_" + sourceProperty.Name.ToStartLowerCase());


            yield return fieldMember;
            yield return this.CreatePropertyMember(sourceProperty, fieldMember);
        }
    }

    protected virtual IEnumerable<CodeAttributeDeclaration> GetPropertyCustomAttributes(PropertyInfo sourceProperty)
    {
        if (sourceProperty == null) throw new ArgumentNullException(nameof(sourceProperty));

        yield return typeof(DataMemberAttribute).ToTypeReference().ToAttributeDeclaration();
    }


    protected CodeTypeReference GetMappingObjectReference()
    {
        return this.IsPersistent() ?

                       new CodeTypeReference(typeof(IMappingObject<,,>))
                       {
                               TypeArguments =
                               {
                                       this.Configuration.DTOMappingServiceInterfaceTypeReference,
                                       this.DomainType,
                                       this.Configuration.Environment.GetIdentityType()
                               }
                       }
                       : new CodeTypeReference(typeof(IMappingObject<,>))
                         {
                                 TypeArguments =
                                 {
                                         this.Configuration.DTOMappingServiceInterfaceTypeReference,
                                         this.DomainType
                                 }
                         };
    }

    DTOFileType IDTOSource.FileType => this.FileType;
    //DTOFileType IFileTypeSource<DTOFileType>.FileType => this.FileType;
}

//public interface IValueMemberFactory
//{
//    IEnumerable<CodeTypeMember> CreatePropertyMember(PropertyInfo sourceProperty);
//}
