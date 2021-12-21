using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.BLLCoreGenerator;
using Framework.Validation;
using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.TestGenerate
{
    public partial class BLLCoreGeneratorConfiguration
    {
        public override IValidatorGenerator GetValidatorGenerator(Type domainType, CodeExpression validatorMapExpr)
        {
            return new ConfigurationValidatorGenerator(this, domainType, validatorMapExpr);
        }

        private class ConfigurationValidatorGenerator : DefaultValidatorGenerator<BLLCoreGeneratorConfiguration>
        {
            public ConfigurationValidatorGenerator(BLLCoreGeneratorConfiguration configuration, Type domainType, CodeExpression validatorMapExpr)
                : base(configuration, domainType, validatorMapExpr)
            {
            }

            protected override IEnumerable<KeyValuePair<CodeExpression, IValidationData>> GetClassValidators()
            {
                foreach (var classValidator in base.GetClassValidators())
                {
                    yield return classValidator;
                }

                if (this.DomainType == typeof(WorkflowLambda))
                {
                    var domainTypes = typeof(PersistentDomainObjectBase).Assembly
                                                                        .GetTypes()
                                                                        .Where(t => !t.IsAbstract && t.IsAssignableToAll(typeof(PersistentDomainObjectBase)))
                                                                        .ToList();

                    foreach (var domainType in domainTypes)
                    {
                        foreach (var property in domainType.GetProperties().Where(property => property.PropertyType == typeof(WorkflowLambda)))
                        {
                            var expr = this.ValidatorMapExpr.ToMethodReferenceExpression("GetReferencedWorkflowLambdaValidator", new[] { domainType })
                                           .ToMethodInvokeExpression(property.ToCodeLambdaExpression());

                            yield return new KeyValuePair<CodeExpression, IValidationData>(expr, null);
                        }
                    }
                }

                if (typeof(StateBase).IsAssignableFrom(this.DomainType))
                {
                    var expr = this.ValidatorMapExpr.ToMethodReferenceExpression("GetStateAutoSetValidator", new[] { this.DomainType }).ToMethodInvokeExpression();

                    yield return new KeyValuePair<CodeExpression, IValidationData>(expr, null);
                }
            }

            protected override IEnumerable<KeyValuePair<CodeExpression, IValidationData>> GetPropertyValidators(PropertyInfo property)
            {
                foreach (var propertyValidator in base.GetPropertyValidators(property))
                {
                    yield return propertyValidator;
                }

                if (this.DomainType.IsAssignableToAll(typeof(PersistentDomainObjectBase)) && property.PropertyType == typeof(WorkflowLambda))
                {
                    var expr = this.ValidatorMapExpr.ToMethodReferenceExpression("GetAppliedLambdaValidator", new[] { this.DomainType }).ToMethodInvokeExpression();

                    yield return new KeyValuePair<CodeExpression, IValidationData>(expr, null);
                }
            }
        }
    }
}