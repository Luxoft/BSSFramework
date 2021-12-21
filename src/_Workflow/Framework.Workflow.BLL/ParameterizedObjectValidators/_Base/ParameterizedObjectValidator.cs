using System;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Validation;
using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Definition;
using Framework.Workflow.Domain.Runtime;

namespace Framework.Workflow.BLL
{
    public abstract class ParameterizedObjectValidator<TDomainObject, TDomainObjectDefinition, TParameterInstance, TParameterDefinition> : BLLContextContainer<IWorkflowBLLContext>
        where TDomainObject : PersistentDomainObjectBase, IDefinitionDomainObject<TDomainObjectDefinition>, IParametersContainer<TParameterInstance>
        where TDomainObjectDefinition : PersistentDomainObjectBase, IParametersContainer<TParameterDefinition>
        where TParameterInstance : ParameterInstance<TParameterDefinition>, IWorkflowInstanceElement
        where TParameterDefinition : Parameter
    {
        protected readonly ITargetSystemService TargetSystemService;

        private readonly string _parameterTypeName = typeof(TDomainObjectDefinition).Name.ToStartLowerCase();


        protected ParameterizedObjectValidator(IWorkflowBLLContext context, ITargetSystemService targetSystemService)
            : base (context)
        {
            if (targetSystemService == null) throw new ArgumentNullException(nameof(targetSystemService));

            this.TargetSystemService = targetSystemService;
        }


        protected abstract object CreateParameterizedObject(TDomainObject domainObject);


        public ValidationResult GetValidateResult(TDomainObject domainObject)
        {
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));


            var mergeResult = domainObject.Definition.Parameters.GetMergeResult(domainObject.Parameters, definition => definition, instance => instance.Definition);

            var validateParametersResult = mergeResult.ToValidationResult(

                list => ValidationResult.FromCondition(!list.Any(), () =>
                                                                    $"Unexpected {this._parameterTypeName} parameters: {list.Join(", ", v => v.Definition.Name)}"),
                list => ValidationResult.FromCondition(!list.Any(), () => $"Missing {this._parameterTypeName} parameters: {list.Join(", ", v => v.Name)}"));


            var validateParametersValueResult = TryResult.Catch(() =>
                this.Context.AnonymousObjectValidator.GetDynamicValidateResult(this.CreateParameterizedObject(domainObject)))

                .Match(res => res, error => ValidationResult.CreateError(new Framework.Validation.ValidationException(error.Message, error)));


            return (validateParametersResult + validateParametersValueResult);
        }
    }
}