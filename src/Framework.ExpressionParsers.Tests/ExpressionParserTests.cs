using Framework.ExpressionParsers.Native.Exceptions;

namespace Framework.ExpressionParsers.Tests;

public sealed class ExpressionParserTests
{
    private readonly ExpressionParser<Func<int, bool>> parser = new();

    [Fact]
    public void Validate_InvalidExpressionAndErrorsUnWrapped_CorrectException()
    {
        // Arrange
        var call = () => this.parser.Validate("n => blah");

        // Act
        var exception = Assert.Throws<ExpressionParingException>(call);

        // Assert
        Assert.Equal("Can't parse value: \"n => blah\". Expected format: \"Func<Int32, Boolean>\"", exception.Message);
        var innerException = Assert.IsType<ParseException>(exception.InnerException);
        Assert.Contains("The name 'blah' does not exist in the current context", innerException.Message);
    }
}
