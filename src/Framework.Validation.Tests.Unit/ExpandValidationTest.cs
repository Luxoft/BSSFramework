using System;
using System.Linq;

using FluentAssertions;

using NUnit.Framework;

namespace Framework.Validation.Tests.Unit;

[TestFixture]
public class ExpandValidationTest
{
    [Test]
    public void ExpandValidation_CheckCompositeProperty_ErrorContainsFullPropertyPath()
    {
        // Arrange
        var validator = Validator.Default;
        var source = new TestRootClass { CompositeProp = new TestCompositeClass() };

        // Act
        var result = validator.GetValidationResult(source);

        // Assert
        result.Errors.Should().HaveCount(1);
        result.Errors[0].Message.Should().Be("The field CompositeProp.SomeProperty of type TestRootClass must be initialized");
    }
}


public class TestRootClass
{
    public TestCompositeClass CompositeProp { get; set; }
}

[ExpandValidation]
public class TestCompositeClass
{
    [RequiredValidator]
    public string SomeProperty { get; set; }
}
