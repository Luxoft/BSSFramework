using System.CodeDom;
using System.Reflection;

using CommonFramework.Maybe;

using Framework.CodeDom;
using Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService.Base;
using Framework.CodeGeneration.DTOGenerator.PropertyAssigner.__Base;
using Framework.CodeGeneration.DTOGenerator.PropertyAssigner._Security.Base;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.Core;

namespace Framework.CodeGeneration.DTOGenerator.Server.PropertyAssigner._Security.ExpandMaybe;

public class ExpandMaybeSecurityToSecurityPropertyAssigner<TConfiguration>  : MaybePropertyAssigner<TConfiguration>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    private readonly IPropertyCodeTypeReferenceService sourceTypeReferenceService;

    public ExpandMaybeSecurityToSecurityPropertyAssigner(IPropertyAssigner<TConfiguration> innerAssigner, IPropertyCodeTypeReferenceService sourceTypeReferenceService)
            : base(innerAssigner)
    {
        if (sourceTypeReferenceService == null) throw new ArgumentNullException(nameof(sourceTypeReferenceService));

        this.sourceTypeReferenceService = sourceTypeReferenceService;
    }


    protected override CodeStatement GetSecurityAssignStatement(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));
        if (sourcePropertyRef == null) throw new ArgumentNullException(nameof(sourcePropertyRef));
        if (targetPropertyRef == null) throw new ArgumentNullException(nameof(targetPropertyRef));

        var targetPropertyTypeRef = this.CodeTypeReferenceService.GetCodeTypeReference(property);

        var resultVarDecl = new CodeVariableDeclarationStatement(targetPropertyTypeRef, "result" + property.Name,

                                                                 property.PropertyType.IsCollection() ? new CodeObjectCreateExpression(targetPropertyTypeRef) : null);

        var resultVarDeclRef = new CodeVariableReferenceExpression(resultVarDecl.Name);

        return new CodeConditionStatement(sourcePropertyRef.ToPropertyReference(nameof(Maybe<>.HasValue)))
               {
                   TrueStatements =
                   {
                       resultVarDecl,
                       this.InnerAssigner.GetAssignStatement(property, sourcePropertyRef.ToPropertyReference(nameof(Maybe<>.Value)), resultVarDeclRef),
                       resultVarDeclRef.ToAssignStatement(targetPropertyRef)
                   },
                   FalseStatements = { new CodeDefaultValueExpression(targetPropertyTypeRef).ToAssignStatement(targetPropertyRef) }
               };
    }
}
