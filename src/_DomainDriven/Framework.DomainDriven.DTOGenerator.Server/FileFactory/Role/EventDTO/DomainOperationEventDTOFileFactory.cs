﻿using System.CodeDom;
using System.Reflection;
using System.Runtime.Serialization;

using Framework.CodeDom;
using Framework.Core;
using Framework.Events;

namespace Framework.DomainDriven.DTOGenerator.Server;

public class DefaultDomainOperationEventDTOFileFactory<TConfiguration> : DTOFileFactory<TConfiguration, DomainOperationEventDTOFileType>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    public DefaultDomainOperationEventDTOFileFactory(TConfiguration configuration, Type domainType, EventOperation domainObjectEvent)
            : base(configuration, domainType)
    {
        if (domainObjectEvent == null) throw new ArgumentNullException(nameof(domainObjectEvent));

        this.FileType = new DomainOperationEventDTOFileType(domainObjectEvent);
    }


    public override DomainOperationEventDTOFileType FileType { get; }

    public override CodeTypeReference BaseReference => this.Configuration.GetCodeTypeReference(null, ServerFileType.BaseEventDTO);

    protected override bool HasMapToDomainObjectMethod { get; }

    protected override bool HasToDomainObjectMethod { get; }


    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration(this.Name)
               {
                       IsClass = true,
                       IsPartial = true,
                       TypeAttributes = TypeAttributes.Public
               };
    }


    protected override IEnumerable<CodeAttributeDeclaration> GetCustomAttributes()
    {
        yield return this.Configuration.GetDTOFileAttribute(this.DomainType, this.FileType);
        yield return this.GetDataContractCodeAttributeDeclaration(this.Configuration.EventDataContractNamespace);
    }


    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        var fieldTypeRef = this.Configuration.GetCodeTypeReference(this.DomainType, ServerFileType.RichEventDTO);

        var field = new CodeMemberField
                    {
                        Type = fieldTypeRef,
                        Name = this.DomainType.Name.ToStartLowerCase(),
                        Attributes = MemberAttributes.Private,
                    };

        var fieldMemberRef = new CodeThisReferenceExpression().ToFieldReference(field);

        var property = new CodeMemberProperty
                       {
                           Type = fieldTypeRef,
                           Name = this.DomainType.Name,
                           Attributes = MemberAttributes.Public | MemberAttributes.Final,
                           CustomAttributes = { new CodeAttributeDeclaration(new CodeTypeReference(typeof(DataMemberAttribute))) },
                           GetStatements = { fieldMemberRef.ToMethodReturnStatement() },
                           SetStatements = { new CodePropertySetValueReferenceExpression().ToAssignStatement(fieldMemberRef) }
                       };

        var mappingServiceParameter = this.GetMappingServiceParameter();
        var mappingServiceParameterRefExpr = mappingServiceParameter.ToVariableReferenceExpression();

        var domainTypeParameter = this.GetDomainTypeSourceParameter();
        var domainTypeParameterRefExp = domainTypeParameter.ToVariableReferenceExpression();


        yield return field;
        yield return property;
        yield return this.GenerateDefaultConstructor();

        yield return new CodeConstructor
                     {
                             Attributes = MemberAttributes.Public,
                             Parameters = { mappingServiceParameter, domainTypeParameter },
                             Statements =
                             {
                                     this.Configuration.GetConvertToDTOMethod(this.DomainType, ServerFileType.RichEventDTO)
                                         .ToMethodInvokeExpression(domainTypeParameterRefExp, mappingServiceParameterRefExpr)
                                         .ToAssignStatement(new CodeThisReferenceExpression().ToFieldReference(field))
                             }
                     };
    }
}
