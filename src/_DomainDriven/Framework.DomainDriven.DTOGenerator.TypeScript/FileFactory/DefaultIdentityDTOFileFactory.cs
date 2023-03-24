using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

using Framework.CodeDom;
using Framework.CodeDom.TypeScript;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Base;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Helpers;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory;

/// <summary>
/// ITypeScriptIdentityDTOFileFactory
/// </summary>
public interface ITypeScriptIdentityDTOFileFactory
{
}

/// <summary>
/// TypeScript IdentityDTO file factory
/// </summary>
/// <typeparam name="TConfiguration"></typeparam>
public class DefaultIdentityDTOFileFactory<TConfiguration> : ClientFileFactory<TConfiguration, DTOFileType>, ITypeScriptIdentityDTOFileFactory
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
{
    public DefaultIdentityDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
    }

    public override DTOFileType FileType => DTOGenerator.FileType.IdentityDTO;

    public override CodeTypeReference BaseReference => null;

    public string IdPropertyName => this.Configuration.Environment.IdentityProperty.Name;

    public CodeConstructor GenerateIdentityConstructor()
    {
        var sourceParameter = new CodeParameterDeclarationExpression(Constants.IdentityTypeName, "id");

        var sourceParameterRef = sourceParameter.ToVariableReferenceExpression();

        return new CodeConstructor
               {
                       Parameters = { sourceParameter },
                       Attributes = MemberAttributes.Public | MemberAttributes.Override,
                       Statements =
                       {
                               new CodeThrowArgumentIsNullOrUndefinedExceptionConditionStatement(sourceParameter),
                               sourceParameterRef.ToAssignStatement(new CodeThisReferenceExpression().ToPropertyReference(this.IdPropertyName))
                       }
               };
    }

    protected CodeMemberField GetIdCodeMemberField()
    {
        return new CodeMemberField(Constants.IdentityTypeName, this.IdPropertyName)
               {
                       Attributes = MemberAttributes.Public,
                       CustomAttributes = { new CodeAttributeDeclaration(new CodeTypeReference(typeof(DataMemberAttribute))) }
               };
    }

    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration(this.Name)
               {
                       IsClass = this.Configuration.IdentityIsReference,
                       IsStruct = !this.Configuration.IdentityIsReference,

                       Attributes = MemberAttributes.Public,
               };
    }

    protected virtual CodeMemberField CreateTypeFieldMember()
    {
        return new CodeMemberField
               {
                       Name = "__type",
                       Type = this.CodeTypeReferenceService.GetCodeTypeReferenceByType(typeof(string)),
                       Attributes = MemberAttributes.Public,
                       InitExpression = new CodePrimitiveExpression(this.Name)
               };
    }

    protected virtual CodeMemberField CreateOwnTypeFieldMember()
    {
        return new CodeMemberField
               {
                       Name = Constants.GenerateTypeIdenity(this.Name),
                       Type = this.CodeTypeReferenceService.GetCodeTypeReferenceByType(typeof(string)),
                       Attributes = MemberAttributes.Private
               };
    }

    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        var emptyInstanceFieldName = "Empty";

        yield return this.CreateTypeFieldMember();
        yield return this.CreateOwnTypeFieldMember();
        yield return this.GetIdCodeMemberField();

        yield return this.GenerateIdentityConstructor();

        yield return new CodeMemberField(this.CurrentReference, emptyInstanceFieldName)
                     {
                             Attributes = MemberAttributes.Static | MemberAttributes.Public,
                             InitExpression = new CodeObjectCreateExpression(
                                                                             this.CurrentReference.BaseType.Split('.').Last(),
                                                                             new CodeTypeReferenceExpression(Constants.IdentityTypeName)
                                                                                     .ToFieldReference(this.Configuration.DTOEmptyPropertyName))
                     };

        yield return this.GenerateIdentityFromStaticInitializeMethodJs();

        yield return this.GenerateToNativeJsonMethod();
    }

    protected override IEnumerable<CodeAttributeDeclaration> GetCustomAttributes()
    {
        yield return this.GetDataContractCodeAttributeDeclaration();
    }
}
