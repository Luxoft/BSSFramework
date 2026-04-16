using Framework.Validation.Attributes;
using Framework.Validation.Map;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Validation.Tests;

public class ExpandValidationTest
{
    [Fact]
    public void ExpandValidation_CheckCompositeProperty_ErrorContainsFullPropertyPath()
    {
        // Arrange
        var sp = new ServiceCollection()
                 .AddSingleton<IValidationMap, ValidationMap>()
                 .AddSingleton<ValidatorCompileCache>()
                 .AddSingleton<IValidator, Validator>()
                 .BuildServiceProvider(new ServiceProviderOptions{ ValidateScopes = true, ValidateOnBuild = true });


        var validator = sp.GetRequiredService<IValidator>();
        var source = new TestRootClass { CompositeProp = new TestCompositeClass() };

        // Act
        var result = validator.GetValidationResult(source);

        // Assert
        Assert.Single(result.Errors);
        Assert.Equal("The field CompositeProp.SomeProperty of type TestRootClass must be initialized", result.Errors[0].Message);
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
