using System.CodeDom;
using System.Reflection;

using CommonFramework.Maybe;
using Framework.BLL.Domain.Exceptions;
using Framework.BLL.Domain.Extensions;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.PropertyAssigner.__Base;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;

namespace Framework.CodeGeneration.DTOGenerator.Server.PropertyAssigner._Security.SecurityToDomainObject.Update;

public abstract class UpdateToDomainObjectPropertyAssignerBase<TConfiguration>(
    IPropertyAssigner<TConfiguration> innerAssigner,
    IDTOGeneratorConfiguration<IDTOGenerationEnvironment> configuration)
    : MaybeSecurityToDomainObjectPropertyAssigner<TConfiguration>(innerAssigner)
    where TConfiguration : class, IServerDTOGeneratorConfiguration<IServerDTOGenerationEnvironment>
{
    protected override bool IsMaybeProperty(PropertyInfo property)
    {
        return !this.CodeTypeReferenceService.IsCollection(property);
    }

    protected abstract CodeExpression GetCondition(PropertyInfo property);

    protected sealed override CodeStatement GetSecurityAssignStatementInternal(PropertyInfo property, CodeExpression justValueRefExpr, CodeStatement innerAssignStatement)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));
        if (justValueRefExpr == null) throw new ArgumentNullException(nameof(justValueRefExpr));
        if (innerAssignStatement == null) throw new ArgumentNullException(nameof(innerAssignStatement));

        var editAttr = configuration.Environment.ExtendedMetadata.GetProperty(property).GetEditDomainObjectAttribute();

        return new CodeConditionStatement(justValueRefExpr.ToPropertyReference(nameof(Maybe<>.HasValue)))
               {
                   TrueStatements =
                   {
                       editAttr == null ? innerAssignStatement : new CodeConditionStatement
                                                                 {
                                                                     Condition = this.GetCondition(property),

                                                                     TrueStatements =
                                                                     {
                                                                         innerAssignStatement
                                                                     },

                                                                     FalseStatements =
                                                                     {
                                                                         new CodeThrowExceptionStatement(new CodeObjectCreateExpression(typeof(BusinessLogicException),
                                                                                                             $"Access for write to field \"{property.Name}\" denied".ToPrimitiveExpression()))
                                                                     }
                                                                 }
                   }
               };
    }
}
