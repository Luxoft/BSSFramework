using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

using Framework.CodeDom;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.DomainDriven.BLLCoreGenerator;
using Framework.Validation;

namespace Framework.Configuration.TestGenerate
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

                if (this.DomainType == typeof(SubscriptionLambda))
                {
                    var domainTypes = typeof(PersistentDomainObjectBase).Assembly
                                                                        .GetTypes()
                                                                        .Where(t => !t.IsAbstract && t.IsAssignableToAll(typeof(PersistentDomainObjectBase)))
                                                                        .ToList();

                    foreach (var domainType in domainTypes)
                    {
                        foreach (var property in domainType.GetProperties().Where(property => property.PropertyType == typeof(SubscriptionLambda)))
                        {
                            var expr = this.ValidatorMapExpr.ToMethodReferenceExpression("GetReferencedSubscriptionLambdaValidator", new[] { domainType })
                                                            .ToMethodInvokeExpression(property.ToCodeLambdaExpression());

                            yield return new KeyValuePair<CodeExpression, IValidationData>(expr, null);
                        }
                    }
                }
            }
        }
    }
}