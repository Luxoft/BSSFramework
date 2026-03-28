using System.CodeDom;

namespace Framework.CodeDom.Extend;

public class CodeBooleanAndOperatorExpression(params CodeExpression[] operations) : CodeBinaryOperatorCollectionExpression(CodeBinaryOperatorType.BooleanAnd, operations);

public class CodeBooleanOrOperatorExpression(params CodeExpression[] operations) : CodeBinaryOperatorCollectionExpression(CodeBinaryOperatorType.BooleanOr, operations);
