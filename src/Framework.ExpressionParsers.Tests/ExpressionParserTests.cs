using FluentAssertions;

using Framework.ExpressionParsers.Native.Exceptions;

using NUnit.Framework;

namespace Framework.ExpressionParsers.Tests;

[TestFixture]
public sealed class ExpressionParserTests
{
    private readonly ExpressionParser<Func<int, bool>> parser = new();

    [Test]
    public void Validate_InvalidExpressionAndErrorsUnWrapped_CorrectException()
    {
        // Arrange
        var call = () => this.parser.Validate("n => blah");

        // Act, Assert
        call.Should().Throw<ExpressionParingException>()
            .WithMessage("Can't parse value: \"n => blah\". Expected format: \"Func<Int32, Boolean>\"")
            .WithInnerException<ParseException>()
            .WithMessage("CS0103: The name 'blah' does not exist in the current context\r\n");
    }
}
