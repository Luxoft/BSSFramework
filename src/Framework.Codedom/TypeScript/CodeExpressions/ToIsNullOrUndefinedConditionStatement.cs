using System.CodeDom;

namespace Framework.CodeDom.TypeScript
{
    /// <summary>
    /// Condition statement for expression which checks it on eguality null or indefined
    /// </summary>
    public class ToIsNullOrUndefinedConditionStatement : CodeConditionStatement
    {
        public ToIsNullOrUndefinedConditionStatement(CodeExpression value)
            : base(value.ToIsNullOrUndefinedExpression())
        {
        }
    }
}
