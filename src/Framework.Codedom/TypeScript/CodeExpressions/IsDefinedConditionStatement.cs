using System.CodeDom;

namespace Framework.CodeDom.TypeScript
{
    /// <summary>
    /// Condition statement for expression which checks it ref state
    /// </summary>
    public class IsDefinedConditionStatement : CodeConditionStatement
    {
        public IsDefinedConditionStatement(CodeExpression value)
            : base(value)
        {
        }
    }
}
