using CommonFramework;

namespace Framework.BLL.Domain.Exceptions;

public static class TypeExtensions
{
    public static bool IsAggregateException(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.GetAggregateExceptionInnerExceptionType() != null;
    }

    public static Type? GetAggregateExceptionInnerExceptionType(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.GetInterfaces().FirstOrDefault(i => i.IsGenericTypeImplementation(typeof(IAggregateException<>)))
                   .Maybe(i => i.GetGenericArguments().Single());
    }
}
