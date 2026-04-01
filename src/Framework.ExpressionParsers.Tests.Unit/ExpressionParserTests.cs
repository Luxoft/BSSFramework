using System.Linq.Expressions;

using FluentAssertions;

using NUnit.Framework;

namespace Framework.ExpressionParsers.Tests.Unit;

[TestFixture]
public sealed class ExpressionParserTests
{
    private readonly NumberEvaluationExpressionParser parser = new (
        CSharpNativeExpressionParser.Compile,
        exp => exp.Compile());

    [Test]
    public void Validate_InvalidExpressionAndErrorsUnWrapped_CorrectException()
    {
        // Arrange
        var call = () => this.parser.Validate("n => blah");

        // Act, Assert
        call.Should().Throw<ParseException>()
            .WithMessage("CS0103: The name 'blah' does not exist in the current context\r\n");
    }

    private class NumberEvaluationExpressionParser(
        INativeExpressionParser parser,
        Func<Expression<Func<int, bool>>, Func<int, bool>> compileFunc)
        : ExpressionParser<Func<int, bool>, Expression<Func<int, bool>>>(parser, compileFunc)
    {
        public override void Validate(string source)
        {
            this.GetExpression(source);
        }

        protected override Expression<Func<int, bool>> GetInternalExpression(string value)
        {
            return this.Parser.Parse<Func<int, bool>>(value);
        }
    }
}
