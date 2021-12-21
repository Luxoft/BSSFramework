using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;
using Framework.Exceptions;
using Framework.Validation;
using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.BLL
{
    public partial class WorkflowValidationMapBase
    {
        private IClassValidator<WorkflowLambda> GetReferencedWorkflowLambdaValidator<TDomainObject>(Expression<Func<TDomainObject, WorkflowLambda>> propExpr)
            where TDomainObject : PersistentDomainObjectBase
        {
            return new WorkflowLambdaValidator<TDomainObject>(propExpr);
        }

        private IPropertyValidator<TDomainObject, WorkflowLambda> GetAppliedLambdaValidator<TDomainObject>()
            where TDomainObject : PersistentDomainObjectBase
        {
            return new AppliedLambdaValidator<TDomainObject>();
        }

        private IClassValidator<TDomainObject> GetStateAutoSetValidator<TDomainObject>()
            where TDomainObject : StateBase
        {
            return new StateAutoSetValidator<TDomainObject>();
        }

        private class StateAutoSetValidator<TDomainObject> : IClassValidator<TDomainObject>
            where TDomainObject : StateBase
        {
            public ValidationResult GetValidationResult(IClassValidationContext<TDomainObject> validationContext)
            {
                var stateBase = validationContext.Source;

                var context = validationContext.ExtendedValidationData.GetValue<IWorkflowBLLContext>(true);

                var domainType = context.GetTargetSystemService(stateBase)
                                        .TypeResolver
                                        .Resolve(stateBase.Workflow.DomainType, true);


                return ValidationResult.TryCatch(() =>
                {
                    if (string.IsNullOrWhiteSpace(stateBase.AutoSetStatePropertyName))
                    {
                        if (string.IsNullOrWhiteSpace(stateBase.AutoSetStatePropertyValue))
                        {
                            return;
                        }

                        throw new BusinessLogicException($"State \"{stateBase.Name}\" Error. Can't apply AutoSetStatePropertyValue \"{stateBase.AutoSetStatePropertyValue}\", because AutoSetStatePropertyName is empty");
                    }

                    var prop = domainType.GetProperty(stateBase.AutoSetStatePropertyName, true);

                    if (!prop.HasSetMethod())
                    {
                        throw new BusinessLogicException($"State \"{stateBase.Name}\" Error. Property \"{prop.Name}\" in domainType \"{domainType.Name}\" must be have public setter");
                    }

                    if (prop.PropertyType == typeof(string))
                    {
                        return;
                    }

                    if (!prop.PropertyType.IsEnum)
                    {
                        throw new BusinessLogicException($"State \"{stateBase.Name}\" Error. Property \"{prop.Name}\" in domainType \"{domainType.Name}\" must be {typeof(Enum).FullName} or {typeof(string).FullName}");
                    }
                    else
                    {
                        try
                        {
                            Enum.Parse(prop.PropertyType, stateBase.AutoSetStatePropertyValue, false);
                        }
                        catch
                        {
                            throw new BusinessLogicException($"State \"{stateBase.Name}\" Error. Can't parse \"{stateBase.AutoSetStatePropertyValue}\" to Enum \"{prop.PropertyType.Name}\"");
                        }
                    }
                });
            }
        }

        private class AppliedLambdaValidator<TDomainObject> : IPropertyValidator<TDomainObject, WorkflowLambda>
            where TDomainObject : PersistentDomainObjectBase
        {
            public ValidationResult GetValidationResult(IPropertyValidationContext<TDomainObject, WorkflowLambda> validationContext)
            {
                var domainObject = validationContext.Source;

                var context = validationContext.ExtendedValidationData.GetValue<IWorkflowBLLContext>(true);

                return ValidationResult.TryCatch(() =>
                {
                    if (validationContext.Value != null)
                    {
                        context.ValidateLambda(domainObject, validationContext.Map.Property);
                    }
                });
            }
        }

        private class WorkflowLambdaValidator<TDomainObject> : IClassValidator<WorkflowLambda>
            where TDomainObject : PersistentDomainObjectBase
        {
            private readonly PropertyInfo property;

            private readonly Func<WorkflowLambda, Expression<Func<TDomainObject, bool>>> getFilter;


            public WorkflowLambdaValidator(Expression<Func<TDomainObject, WorkflowLambda>> propExpr)
            {
                if (propExpr == null) throw new ArgumentNullException(nameof(propExpr));

                this.property = propExpr.GetProperty();

                this.getFilter = lambda => from domainObjectLambda in propExpr

                                           select domainObjectLambda == lambda;
            }


            public ValidationResult GetValidationResult(IClassValidationContext<WorkflowLambda> validationContext)
            {
                if (validationContext == null) throw new ArgumentNullException(nameof(validationContext));

                var context = validationContext.ExtendedValidationData.GetValue<IWorkflowBLLContext>(true);

                var objects = context.Logics.Default.Create<TDomainObject>().GetObjectsBy(this.getFilter(validationContext.Source));

                return objects.Select(obj => ValidationResult.TryCatch(() => context.ValidateLambda(obj, this.property))).Sum();
            }
        }
    }
}
