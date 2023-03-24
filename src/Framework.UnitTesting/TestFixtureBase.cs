using AutoFixture;
using AutoFixture.AutoNSubstitute;

using NSubstitute;

namespace Framework.UnitTesting;

/// <summary>
/// Base class for test fixtures.
/// </summary>
public class TestFixtureBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestFixtureBase"/> class.
    /// </summary>
    protected TestFixtureBase()
    {
        this.Fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        this.Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    /// <summary>
    /// Gets the fixture.
    /// </summary>
    /// <value>
    /// The fixture.
    /// </value>
    protected IFixture Fixture { get; }

    /// <summary>
    /// Creates the strict mock.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="argumentsForConstructor">The arguments for constructor.</param>
    /// <returns>Created mock instance.</returns>
    protected T CreateStrictMock<T>(params object[] argumentsForConstructor)
            where T : class
    {
        return Substitute.For<T>(argumentsForConstructor);
    }

    /// <summary>
    /// Creates the stub.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="argumentsForConstructor">The arguments for constructor.</param>
    /// <returns>Created mock instance.</returns>
    protected T CreateStub<T>(params object[] argumentsForConstructor)
            where T : class
    {
        return Substitute.For<T>(argumentsForConstructor);
    }
}
