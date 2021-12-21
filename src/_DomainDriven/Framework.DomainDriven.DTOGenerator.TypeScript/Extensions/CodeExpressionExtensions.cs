using System.CodeDom;

using Framework.CodeDom.TypeScript;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.Extensions
{
    public static class CodeExpressionExtensions
    {
        public static CodeExpression UnwrapObservableProperty(this CodeExpression propertyRef)
        {
            return new CodeMethodInvokeExpression(new CodeMethodReferenceExpression() { MethodName = "unwrap" }, new[] { propertyRef });
        }

        public static CodeExpression UnwrapObservablePeriodPropertyToPeriod(this CodeExpression propertyRef)
        {
            return new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("Framework.Core.Period"), "fromObservable", new[] { propertyRef.UnwrapObservableProperty() });
        }

        public static CodeExpression ConvertToDate(this CodeExpression propertyRef)
        {
            return new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("Convert"), "toDate", new[] { propertyRef });
        }

        public static CodeExpression ConvertToPeriod(this CodeExpression propertyRef)
        {
            return new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("Framework"), "Core.Period.toPeriod", new[] { propertyRef });
        }

        public static CodeExpression ConvertDateToOData(this CodeExpression sourcePropertyRef)
        {
            return new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("Convert"), "toOData", new[] { sourcePropertyRef });
        }

        public static CodeExpression ConvertPeriodToOData(this CodeExpression sourcePropertyRef)
        {
            return new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("Framework"), "Core.Period.toOData", new[] { sourcePropertyRef });
        }

        public static CodeExpression ConvertToObservablePeriod(this CodeExpression propertyRef)
        {
            return new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("Framework"), "Core.ObservablePeriod.toPeriod", new[] { propertyRef });
        }

        public static CodeTypeReference NormalizeTypeReference(this CodeTypeReference typeReference, System.Type type)
        {
            var keepNamespace = type.IsPeriod();
            var typeName = keepNamespace ? typeReference.GetTypeName() : typeReference.GetTypeNameWithoutNameSpace().ConvertToTypeScriptType(true);

            return new CodeTypeReference(typeName);
        }

        public static CodeTypeReference ConvertToArray(this CodeTypeReference propertyRef, bool generic = true)
        {
            if (!generic)
            {
                return new CodeTypeReference(propertyRef.BaseType + " []");
            }

            return new CodeTypeReference("Array")
                   {
                       TypeArguments = { propertyRef }
                   };
        }
    }
}