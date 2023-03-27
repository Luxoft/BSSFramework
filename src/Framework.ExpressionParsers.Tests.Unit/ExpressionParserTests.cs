using System.Linq.Expressions;

using FluentAssertions;

using Framework.Exceptions;

using NUnit.Framework;

namespace Framework.ExpressionParsers.Tests.Unit;

[TestFixture]
public sealed class ExpressionParserTests
{
    private NumberEvaluationExpressionParser parser;

    [SetUp]
    public void SetUp()
    {
        this.parser = new NumberEvaluationExpressionParser(
                                                           CSharpNativeExpressionParser.Compile,
                                                           exp => exp.Compile());
    }

    [Test]
    public void Validate_InvalidExpressionAndErrorsUnWrapped_CorrectException()
    {
        // Arrange
        Action call = () => this.parser.Validate("n => blah");

        // Act, Assert
        call.Should().Throw<ParseException>()
            .WithMessage("CS0103: The name 'blah' does not exist in the current context\r\n");
    }

    [Test]
    public void Validate_InvalidExpressionAndErrorsWrapped_CorrectException()
    {
        // Arrange
        this.parser.WrapErrors = true;
        Action call = () => this.parser.Validate("n => blah");

        // Act, Assert
        call.Should().Throw<BusinessLogicException>()
            .WithMessage("Can't parse value: \"n => blah\". Expected format: \"Func<Int32, Boolean>\"")
            .WithInnerException<ParseException>()
            .WithMessage("CS0103: The name 'blah' does not exist in the current context\r\n");
    }

    private class NumberEvaluationExpressionParser
            : ExpressionParser<Func<int, bool>, Expression<Func<int, bool>>>
    {
        public NumberEvaluationExpressionParser(
                INativeExpressionParser parser,
                Func<Expression<Func<int, bool>>, Func<int, bool>> compileFunc)
                : base(parser, compileFunc)
        {
        }

        public bool WrapErrors { get; set; }

        public override void Validate(string source)
        {
            this.GetExpression(source, this.WrapErrors);
        }

        protected override Expression<Func<int, bool>> GetInternalExpression(string value)
        {
            return this.Parser.Parse<Func<int, bool>>(value);
        }
    }
}
