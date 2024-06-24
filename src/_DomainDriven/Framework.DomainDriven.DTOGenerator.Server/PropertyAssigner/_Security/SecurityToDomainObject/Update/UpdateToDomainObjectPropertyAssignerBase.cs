using System.CodeDom;
using System.Reflection;

using Framework.CodeDom;
using Framework.Exceptions;
using Framework.Security;

namespace Framework.DomainDriven.DTOGenerator.Server;

public abstract class UpdateToDomainObjectPropertyAssignerBase<TConfiguration> : MaybeSecurityToDomainObjectPropertyAssigner<TConfiguration>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    private readonly IGeneratorConfigurationBase<IGenerationEnvironmentBase> configuration;

    protected UpdateToDomainObjectPropertyAssignerBase(IPropertyAssigner<TConfiguration> innerAssigner, IGeneratorConfigurationBase<IGenerationEnvironmentBase> configuration)
            : base(innerAssigner)
    {
        this.configuration = configuration;
    }



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

        var editAttr = this.configuration.Environment.ExtendedMetadata.GetProperty(property).GetEditDomainObjectAttribute();

        return new CodeNotNullConditionStatement(justValueRefExpr)
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
