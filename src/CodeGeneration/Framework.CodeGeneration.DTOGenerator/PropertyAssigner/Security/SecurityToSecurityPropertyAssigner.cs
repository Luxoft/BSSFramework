using System.CodeDom;
using System.Reflection;

using Anch.Core;

using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService.Base;
using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.Core;

namespace Framework.CodeGeneration.DTOGenerator.PropertyAssigner.Security;

public class SecurityToSecurityPropertyAssigner<TConfiguration>(
    IPropertyAssigner<TConfiguration> innerAssigner,
    IPropertyCodeTypeReferenceService sourceTypeReferenceService)
    : MaybePropertyAssigner<TConfiguration>(innerAssigner)
    where TConfiguration : class, IDTOGeneratorConfiguration<IDTOGenerationEnvironment>
{
    private readonly IPropertyCodeTypeReferenceService sourceTypeReferenceService = sourceTypeReferenceService ?? throw new ArgumentNullException(nameof(sourceTypeReferenceService));

    protected override CodeStatement GetSecurityAssignStatement(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef)
    {
        if (property is null) throw new ArgumentNullException(nameof(property));
        if (sourcePropertyRef is null) throw new ArgumentNullException(nameof(sourcePropertyRef));
        if (targetPropertyRef is null) throw new ArgumentNullException(nameof(targetPropertyRef));

        var targetPropertyTypeRef = this.CodeTypeReferenceService!.GetCodeTypeReference(property);

        var resultVarDecl = new CodeVariableDeclarationStatement(targetPropertyTypeRef, "result" + property.Name,

                                                                 property.PropertyType.IsCollection() ? new CodeObjectCreateExpression(targetPropertyTypeRef) : null);

        var resultVarDeclRef = new CodeVariableReferenceExpression(resultVarDecl.Name);

        return new CodeConditionStatement(sourcePropertyRef.ToPropertyReference(nameof(Maybe<>.HasValue)))
        {
            TrueStatements =
                   {
                       resultVarDecl,
                       this.InnerAssigner.GetAssignStatement(property, sourcePropertyRef.ToPropertyReference(nameof(Maybe<>.Value)), resultVarDeclRef),
                       typeof(Maybe).ToTypeReferenceExpression()
                                    .ToMethodReferenceExpression(nameof(Maybe.Return))
                                    .ToMethodInvokeExpression(resultVarDeclRef)
                                    .ToAssignStatement(targetPropertyRef)
                   },
            FalseStatements = { targetPropertyTypeRef.ToNothingValueExpression().ToAssignStatement(targetPropertyRef) }
        };


    }
}

