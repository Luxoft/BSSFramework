using System;
using System.CodeDom;

namespace Framework.CodeDom.TypeScript
{
    /// <summary>
    /// Code expression helper extensions
    /// </summary>
    public static class CodeExpressionExtensions
    {
        public static CodeExpression ToIsNullOrUndefinedExpression(this CodeExpression value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            return new CodeCastExpression("!", value);
        }

        public static CodeParameterDeclarationExpression WithOptional(this CodeParameterDeclarationExpression parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            parameter.UserData["IsOptional"] = true;

            return parameter;
        }

        public static bool IsOptional(this CodeParameterDeclarationExpression parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            return parameter.UserData["IsOptional"] is bool v && v;
        }
    }
}
