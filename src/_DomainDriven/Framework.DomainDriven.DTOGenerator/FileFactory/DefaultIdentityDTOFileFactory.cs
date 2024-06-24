using System.CodeDom;
using System.Reflection;
using System.Runtime.Serialization;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.DTOGenerator;

public class DefaultIdentityDTOFileFactory<TConfiguration> : FileFactory<TConfiguration, DTOFileType>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public DefaultIdentityDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
    }


    public override DTOFileType FileType { get; } = DTOGenerator.FileType.IdentityDTO;

    public string IdPropertyName => this.Configuration.Environment.IdentityProperty.Name;


    protected IEnumerable<CodeTypeMember> GetIdCodeMembers()
    {
        if (this.Configuration.ForceGenerateProperties(this.DomainType, this.FileType))
        {
            var fieldMember = new CodeMemberField(this.Configuration.Environment.IdentityProperty.PropertyType, "_" + this.IdPropertyName.ToStartLowerCase())
                              {
                                      Attributes = MemberAttributes.Private
                              };

            var fieldRefExpr = new CodeThisReferenceExpression().ToFieldReference(fieldMember);

            yield return fieldMember;

            yield return new CodeMemberProperty
                         {
                                 Type = fieldMember.Type,
                                 Name = this.IdPropertyName,
                                 Attributes = MemberAttributes.Public | MemberAttributes.Final,
                                 CustomAttributes = { new CodeAttributeDeclaration(new CodeTypeReference(typeof(DataMemberAttribute))) },
                                 GetStatements = { { fieldRefExpr.ToMethodReturnStatement() } },
                                 SetStatements = { { new CodePropertySetValueReferenceExpression().ToAssignStatement(fieldRefExpr) } }
                         };
        }
        else
        {
            yield return new CodeMemberField(this.Configuration.Environment.IdentityProperty.PropertyType, this.IdPropertyName)
                         {
                                 Attributes = MemberAttributes.Public,
                                 CustomAttributes = { new CodeAttributeDeclaration(new CodeTypeReference(typeof(DataMemberAttribute))) }
                         };
        }
    }

    protected CodeExpression GetAssignIdExpression()
    {
        var forceProp = this.Configuration.ForceGenerateProperties(this.DomainType, this.FileType);

        return new CodeThisReferenceExpression().ToPropertyReference(forceProp ? "_" + this.IdPropertyName.ToStartLowerCase() : this.IdPropertyName);
    }

    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration(this.Name)
               {
                       IsClass = this.Configuration.IdentityIsReference,
                       IsStruct = !this.Configuration.IdentityIsReference,

                       TypeAttributes = TypeAttributes.Public,
               };
    }

    protected override IEnumerable<CodeConstructor> GetConstructors()
    {
        foreach (var baseCtor in base.GetConstructors())
        {
            yield return baseCtor;
        }

        {
            var idParameter = new CodeParameterDeclarationExpression(this.Configuration.Environment.IdentityProperty.PropertyType, this.IdPropertyName.ToStartLowerCase());

            yield return new CodeConstructor
                         {
                                 Attributes = MemberAttributes.Public,
                                 Parameters = { idParameter },
                                 Statements = { idParameter.ToVariableReferenceExpression().ToAssignStatement(this.GetAssignIdExpression()) }
                         };
        }

        if (this.Configuration.GeneratePolicy.Used(this.DomainType, DTOGenerator.FileType.SimpleDTO))
        {
            var sourceParameter = this.Configuration.GetCodeTypeReference(this.DomainType, DTOGenerator.FileType.SimpleDTO).ToParameterDeclarationExpression("source");

            var sourceParameterRef = sourceParameter.ToVariableReferenceExpression();

            yield return new CodeConstructor
                         {
                                 Parameters = { sourceParameter },
                                 Attributes = MemberAttributes.Public | MemberAttributes.Override,
                                 Statements =
                                 {
                                         new CodeThrowArgumentNullExceptionConditionStatement(sourceParameter),

                                         this.Configuration.GetIdentityPropertyCodeExpression(sourceParameterRef).ToAssignStatement(this.GetAssignIdExpression())
                                 },
                         };
        }
    }

    protected override System.Collections.Generic.IEnumerable<CodeTypeMember> GetMembers()
    {
        foreach (var baseMember in base.GetMembers())
        {
            yield return baseMember;
        }

        var emptyInstanceFieldName = "EmptyField";

        var operatorValueParameter1 = this.CurrentReference.ToParameterDeclarationExpression("identity1");
        var operatorValueParameter2 = this.CurrentReference.ToParameterDeclarationExpression("identity2");

        var operatorOtherName = "other";

        foreach (var idMember in this.GetIdCodeMembers())
        {
            yield return idMember;
        }

        yield return new CodeMemberField(this.CurrentReference, emptyInstanceFieldName)
                     {
                             Attributes = MemberAttributes.Private | MemberAttributes.Static | MemberAttributes.Final,
                             InitExpression = new CodeObjectCreateExpression(
                                                                             this.CurrentReference,
                                                                             this.Configuration.Environment.GetIdentityType().ToTypeReferenceExpression().ToFieldReference(
                                                                              this.Configuration.DTOEmptyPropertyName))
                     };

        yield return new CodeMemberProperty
                     {
                             Name = this.Configuration.DTOEmptyPropertyName,
                             Attributes = MemberAttributes.Public | MemberAttributes.Static,
                             Type = this.CurrentReference,
                             GetStatements =
                             {
                                     this.CurrentReference.ToTypeReferenceExpression()
                                         .ToFieldReference(emptyInstanceFieldName)
                                         .ToMethodReturnStatement()
                             }
                     };

        yield return new CodeMemberMethod
                     {
                             Name = "operator ==",
                             Attributes = MemberAttributes.Public | MemberAttributes.Static,
                             ReturnType = new CodeTypeReference(typeof(bool)),
                             Parameters =
                             {
                                     operatorValueParameter1,
                                     operatorValueParameter2
                             },
                             Statements =
                             {
                                     this.Configuration.IdentityIsReference
                                             ? new CodeBooleanOrOperatorExpression(
                                                                                   new CodeObjectReferenceEqualsExpression(
                                                                                    operatorValueParameter1.ToVariableReferenceExpression(),
                                                                                    operatorValueParameter2.ToVariableReferenceExpression()),

                                                                                   new CodeBooleanAndOperatorExpression(

                                                                                    new CodeObjectReferenceEqualsExpression(
                                                                                     operatorValueParameter1.ToVariableReferenceExpression(),
                                                                                     this.CurrentReference.ToDefaultValueExpression()).ToNegateExpression(),

                                                                                    operatorValueParameter1.ToVariableReferenceExpression()
                                                                                            .ToMethodInvokeExpression("Equals",
                                                                                                operatorValueParameter2.
                                                                                                        ToVariableReferenceExpression())))

                                                     .ToMethodReturnStatement()
                                             : operatorValueParameter1.ToVariableReferenceExpression()
                                                                      .ToMethodInvokeExpression("Equals",
                                                                          operatorValueParameter2.ToVariableReferenceExpression())
                                                                      .ToMethodReturnStatement()
                             }
                     };

        yield return new CodeMemberMethod
                     {
                             Name = "operator !=",
                             Attributes = MemberAttributes.Public | MemberAttributes.Static,
                             ReturnType = new CodeTypeReference(typeof(bool)),
                             Parameters =
                             {
                                     operatorValueParameter1,
                                     operatorValueParameter2
                             },
                             Statements =
                             {
                                     new CodeValueUnequalityOperatorExpression(
                                                                               operatorValueParameter1.ToVariableReferenceExpression(),
                                                                               operatorValueParameter2.ToVariableReferenceExpression())
                                             .ToMethodReturnStatement()
                             }
                     };

        yield return new CodeMemberMethod
                     {
                             Name = "Equals",
                             Attributes = MemberAttributes.Override | MemberAttributes.Public,
                             ReturnType = new CodeTypeReference(typeof(bool)),
                             Parameters = { new CodeParameterDeclarationExpression(typeof(object), operatorOtherName) },
                             Statements =
                             {
                                     new CodeBooleanAndOperatorExpression (

                                                                           new CodeIsNotNullExpression(new CodeVariableReferenceExpression(operatorOtherName)),

                                                                           new CodeValueEqualityOperatorExpression(

                                                                            this.CurrentReference.ToTypeOfExpression(),

                                                                            new CodeVariableReferenceExpression(operatorOtherName).ToMethodInvokeExpression("GetType")),

                                                                           new CodeThisReferenceExpression().ToMethodInvokeExpression(

                                                                            "Equals",

                                                                            new CodeCastExpression(this.CurrentReference,

                                                                                new CodeVariableReferenceExpression(operatorOtherName))))

                                             .ToMethodReturnStatement()
                             }
                     };

        yield return new CodeMemberMethod
                     {
                             Name = "Equals",
                             Attributes = MemberAttributes.Final | MemberAttributes.Public,
                             ReturnType = new CodeTypeReference(typeof(bool)),
                             Parameters = { this.CurrentReference.ToParameterDeclarationExpression(operatorOtherName) },
                             Statements =
                             {
                                     this.Configuration.IdentityIsReference
                                             ? new CodeBooleanAndOperatorExpression(

                                                                                    new CodeObjectReferenceEqualsExpression(new CodeVariableReferenceExpression(operatorOtherName), new CodePrimitiveExpression(null)).ToNegateExpression(),
                                                                                    new CodeValueEqualityOperatorExpression(
                                                                                     this.GetAssignIdExpression(),
                                                                                     new CodeVariableReferenceExpression(operatorOtherName).ToFieldReference(this.IdPropertyName)))
                                                     .ToMethodReturnStatement()
                                             : new CodeValueEqualityOperatorExpression(
                                                                                       this.GetAssignIdExpression(),
                                                                                       new CodeVariableReferenceExpression(operatorOtherName).ToFieldReference(this.IdPropertyName))
                                                     .ToMethodReturnStatement()

                             }
                     };

        yield return new CodeMemberMethod
                     {
                             Name = "GetHashCode",
                             Attributes = MemberAttributes.Override | MemberAttributes.Public,
                             ReturnType = new CodeTypeReference(typeof(int)),
                             Statements =
                             {
                                     new CodeThisReferenceExpression().ToFieldReference(this.IdPropertyName)
                                                                      .ToMethodInvokeExpression("GetHashCode")
                                                                      .ToMethodReturnStatement()
                             }
                     };

        yield return new CodeMemberMethod
                     {
                             Name = "ToString",
                             Attributes = MemberAttributes.Override | MemberAttributes.Public,
                             ReturnType = new CodeTypeReference(typeof(string)),
                             Statements =
                             {
                                     new CodeThisReferenceExpression().ToFieldReference(this.IdPropertyName)
                                                                      .ToMethodInvokeExpression("ToString")
                                                                      .ToMethodReturnStatement()
                             }
                     };

        yield return this.GetIdentityObjectImplementation(true);
    }

    protected override System.Collections.Generic.IEnumerable<CodeAttributeDeclaration> GetCustomAttributes()
    {
        yield return this.GetDataContractCodeAttributeDeclaration();

        //foreach (var attr in new[] { true, false }.Select(flag => this.DomainType.GetDomainObjectAccessAttribute(flag)).Where(attr => attr != null))
        //{
        //    yield return attr.ToCodeAttributeDeclaration();
        //}
    }

    protected override System.Collections.Generic.IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        yield return this.CurrentReference.ToEquatableReference();
        yield return this.Configuration.GetIdentityObjectCodeTypeReference();
    }
}
