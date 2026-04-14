using System.CodeDom;
using Framework.BLL;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators;

namespace Framework.CodeGeneration.ServiceModelGenerator.Configuration;

internal static class ServiceModelGeneratorConfigurationExtensions
{
    extension(IServiceModelGeneratorConfiguration<IServiceModelGenerationEnvironment> configuration)
    {
        public IEnumerable<Type> GetActualDomainTypes()
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return configuration.DomainTypes.Where(configuration.HasMethods);
        }

        public IEnumerable<IServiceMethodGenerator> GetActualMethodGenerators(Type domainType)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (domainType == null) throw new ArgumentNullException(nameof(domainType));

            return configuration.GetMethodGenerators(domainType).Where(m => configuration.GeneratePolicy.Used(domainType, m.Identity));
        }

        public CodeExpression GetByIdExpr(CodeExpression bllRefExpr, CodeExpression parameterExprRef, CodeExpression fetchsExpr)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (bllRefExpr == null) throw new ArgumentNullException(nameof(bllRefExpr));
            if (parameterExprRef == null) throw new ArgumentNullException(nameof(parameterExprRef));

            return configuration.GetByIdExprByIdentityRef(bllRefExpr, parameterExprRef.ToPropertyReference(configuration.Environment.IdentityProperty), fetchsExpr);
        }

        public CodeExpression GetByIdExprByIdentityRef(CodeExpression bllRefExpr, CodeExpression parameterIdentityProperty, CodeExpression fetchsExpr)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (bllRefExpr == null) throw new ArgumentNullException(nameof(bllRefExpr));
            if (parameterIdentityProperty == null) throw new ArgumentNullException(nameof(parameterIdentityProperty));

            return bllRefExpr.ToMethodInvokeExpression("GetById", parameterIdentityProperty, new CodePrimitiveExpression(true), fetchsExpr);
        }

        public CodeExpression GetByIdExpr(CodeExpression bllRefExpr, CodeExpression parameterExprRef)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (bllRefExpr == null) throw new ArgumentNullException(nameof(bllRefExpr));
            if (parameterExprRef == null) throw new ArgumentNullException(nameof(parameterExprRef));

            return configuration.GetByIdExprByIdentityRef(bllRefExpr, parameterExprRef.ToPropertyReference(configuration.Environment.IdentityProperty));
        }

        public CodeExpression GetByIdExprByIdentityRef(CodeExpression bllRefExpr, CodeExpression parameterIdentityProperty)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (bllRefExpr == null) throw new ArgumentNullException(nameof(bllRefExpr));
            if (parameterIdentityProperty == null) throw new ArgumentNullException(nameof(parameterIdentityProperty));

            return bllRefExpr.ToMethodInvokeExpression("GetById", parameterIdentityProperty, new CodePrimitiveExpression(true));
        }

        public CodeExpression GetByNameExpr(CodeExpression bllRefExpr, CodeExpression parameterExprRef, CodeExpression fetchsExpr)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (bllRefExpr == null) throw new ArgumentNullException(nameof(bllRefExpr));
            if (parameterExprRef == null) throw new ArgumentNullException(nameof(parameterExprRef));

            var method = typeof(DefaultDomainBLLBaseExtensions).ToTypeReferenceExpression().ToMethodReferenceExpression("GetByName");

            return bllRefExpr.ToStaticMethodInvokeExpression(method, parameterExprRef, new CodePrimitiveExpression(true), fetchsExpr);
        }

        public CodeExpression GetByCodeExpr(CodeExpression bllRefExpr, CodeExpression parameterExprRef, CodeExpression fetchsExpr)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (bllRefExpr == null) throw new ArgumentNullException(nameof(bllRefExpr));
            if (parameterExprRef == null) throw new ArgumentNullException(nameof(parameterExprRef));

            var method = typeof(DefaultDomainBLLBaseExtensions).ToTypeReferenceExpression().ToMethodReferenceExpression("GetByCode");

            return bllRefExpr.ToStaticMethodInvokeExpression(method, parameterExprRef, new CodePrimitiveExpression(true), fetchsExpr);
        }
    }
}
