using System.Collections;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace FluentAssertions;

public static class AssertionExtensions
{
    public static ObjectAssertions<T> Should<T>(this T actual) => new(actual);

    public static ActionAssertions Should(this Action action) => new(action);

    public static FuncAssertions<TResult> Should<TResult>(this Func<TResult> action) => new(action);

    public static AsyncActionAssertions Should(this Func<Task> action) => new(action);
}

public sealed class ObjectAssertions<T>(T actual)
{
    public T Subject { get; } = actual;

    public ObjectAssertions<T> And => this;

    public ObjectAssertions<T> Be(T expected)
    {
        Xunit.Assert.Equal(expected, actual);
        return this;
    }

    public ObjectAssertions<T> NotBe(T expected)
    {
        Xunit.Assert.NotEqual(expected, actual);
        return this;
    }

    public ObjectAssertions<T> BeTrue()
    {
        Xunit.Assert.True((bool)(object)actual!);
        return this;
    }

    public ObjectAssertions<T> BeFalse()
    {
        Xunit.Assert.False((bool)(object)actual!);
        return this;
    }

    public ObjectAssertions<T> NotBeNull()
    {
        Xunit.Assert.NotNull(actual);
        return this;
    }

    public ObjectAssertions<T> BeNull()
    {
        Xunit.Assert.Null(actual);
        return this;
    }

    public ObjectAssertions<T> BeEmpty()
    {
        switch (actual)
        {
            case string s:
                Xunit.Assert.Empty(s);
                break;
            case IEnumerable e:
                Xunit.Assert.Empty(e.Cast<object?>());
                break;
            default:
                throw new Xunit.Sdk.XunitException("Unsupported BeEmpty assertion subject.");
        }

        return this;
    }

    public ObjectAssertions<T> HaveCount(int expectedCount)
    {
        Xunit.Assert.Equal(expectedCount, GetEnumerable().Cast<object?>().Count());
        return this;
    }

    public ObjectAssertions<T> Contain(Func<dynamic, bool> predicate)
    {
        Xunit.Assert.True(GetEnumerable().Cast<dynamic>().Any(item => predicate(item)));
        return this;
    }

    public ObjectAssertions<T> Contain(object expected)
    {
        Xunit.Assert.True(GetEnumerable().Cast<object?>().Any(item => Equals(item, expected)));
        return this;
    }

    public ObjectAssertions<T> NotContain(Func<dynamic, bool> predicate)
    {
        Xunit.Assert.False(GetEnumerable().Cast<dynamic>().Any(item => predicate(item)));
        return this;
    }

    public ObjectAssertions<T> NotContain(object expected)
    {
        Xunit.Assert.False(GetEnumerable().Cast<object?>().Any(item => Equals(item, expected)));
        return this;
    }

    public dynamic ContainSingle()
    {
        var item = Xunit.Assert.Single(GetEnumerable().Cast<object?>());
        return item!;
    }

    public dynamic ContainSingle(Func<dynamic, bool> predicate)
    {
        var matches = GetEnumerable().Cast<dynamic>().Where(item => predicate(item)).Cast<object?>().ToList();
        Xunit.Assert.Single(matches);
        return matches[0]!;
    }

    public ObjectAssertions<T> OnlyHaveUniqueItems()
    {
        var items = GetEnumerable().Cast<object?>().ToList();
        Xunit.Assert.Equal(items.Count, items.Distinct().Count());
        return this;
    }

    public ObjectAssertions<T> BeReadable()
    {
        Xunit.Assert.True((actual as Stream)?.CanRead ?? false);
        return this;
    }

    public ObjectAssertions<T> BeEquivalentTo(IEnumerable expected)
    {
        Xunit.Assert.Equal(Serialize(expected.Cast<object?>().ToList()), Serialize(GetEnumerable().Cast<object?>().ToList()));
        return this;
    }

    public ObjectAssertions<T> BeEquivalentTo<TExpected>(TExpected expected)
    {
        Xunit.Assert.Equal(Serialize(expected), Serialize(actual));
        return this;
    }

    public ObjectAssertions<T> BeGreaterThan<I>(I expected)
        where I : IComparable
    {
        Xunit.Assert.True(((IComparable)(object)actual!).CompareTo(expected) > 0);
        return this;
    }

    public ObjectAssertions<T> BeGreaterThanOrEqualTo<I>(I expected)
        where I : IComparable
    {
        Xunit.Assert.True(((IComparable)(object)actual!).CompareTo(expected) >= 0);
        return this;
    }

    public ObjectAssertions<T> BeCloseTo(DateTime expected, TimeSpan precision)
    {
        var value = (DateTime)(object)actual!;
        Xunit.Assert.InRange((value - expected).Duration(), TimeSpan.Zero, precision);
        return this;
    }

    public ObjectAssertions<T> Contain(string expectedSubstring)
    {
        Xunit.Assert.Contains(expectedSubstring, (string)(object)actual!);
        return this;
    }

    public ObjectAssertions<T> NotContainInConsecutiveOrder(IEnumerable expectedItems)
    {
        var source = GetEnumerable().Cast<object?>().ToList();
        var expected = expectedItems.Cast<object?>().ToList();

        if (expected.Count == 0)
        {
            return this;
        }

        for (var i = 0; i <= source.Count - expected.Count; i++)
        {
            if (source.Skip(i).Take(expected.Count).SequenceEqual(expected))
            {
                throw new Xunit.Sdk.XunitException("Sequence contains the specified items in consecutive order.");
            }
        }

        return this;
    }

    private IEnumerable GetEnumerable() => actual as IEnumerable ?? throw new Xunit.Sdk.XunitException("Assertion subject is not enumerable.");

    private static string Serialize<TValue>(TValue value) => JsonSerializer.Serialize(value);
}

public sealed class ActionAssertions(Action action)
{
    public ExceptionAssertions<TException> Throw<TException>(string because = "")
        where TException : Exception
        => new(Xunit.Assert.Throws<TException>(action));

    public void NotThrow(string because = "") => Xunit.Assert.Null(Xunit.Record.Exception(action));
}

public sealed class FuncAssertions<TResult>(Func<TResult> action)
{
    public ExceptionAssertions<TException> Throw<TException>(string because = "")
        where TException : Exception
        => new(Xunit.Assert.Throws<TException>(() => action()));

    public void NotThrow(string because = "") => Xunit.Assert.Null(Xunit.Record.Exception(() => action()));
}

public sealed class AsyncActionAssertions(Func<Task> action)
{
    public async Task<ExceptionAssertions<TException>> ThrowAsync<TException>(string because = "")
        where TException : Exception
        => new(await Xunit.Assert.ThrowsAsync<TException>(action));

    public async Task NotThrowAsync() => Xunit.Assert.Null(await Xunit.Record.ExceptionAsync(action));
}

public sealed class ExceptionAssertions<TException>(TException exception)
    where TException : Exception
{
    public TException And => exception;

    public ExceptionAssertions<TException> WithMessage(string message)
    {
        var pattern = "^" + Regex.Escape(message).Replace("\\*", ".*") + "$";
        Xunit.Assert.Matches(pattern, exception.Message);
        return this;
    }
}
