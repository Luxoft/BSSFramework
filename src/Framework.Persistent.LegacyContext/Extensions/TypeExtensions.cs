using CommonFramework;

using Framework.Core;

namespace Framework.Persistent;

public static class TypeExtensions
{
    public static Type GetIdentType(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        var request = from i in type.GetInterfaces()

                      where i.IsGenericTypeImplementation(typeof(IIdentityObject<>))

                      select i.GetGenericArguments().Single(() => new Exception($"Type:{type.Name} has more then one generic argument"));

        return request.SingleOrDefault(() => new ArgumentException($"Type:{type.Name} has more then one IIdentityObject interface"));
    }
}
