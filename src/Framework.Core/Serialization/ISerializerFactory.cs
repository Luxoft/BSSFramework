using System;
using System.Reflection;

using JetBrains.Annotations;

namespace Framework.Core.Serialization;

public interface ISerializerFactory<TSerializedValue>
{
    ISerializer<TSerializedValue, TValue> Create<TValue>();
}

public static class SerializerFactoryExtensions
{
    public static TSerializedValue Serialize<TSerializedValue, TValue>(this ISerializerFactory<TSerializedValue> serializerFactory, TValue value)
    {
        if (serializerFactory == null) throw new ArgumentNullException(nameof(serializerFactory));

        return serializerFactory.Create<TValue>().Serialize(value);
    }

    public static void Validate<TSerializedValue>([NotNull] this ISerializerFactory<TSerializedValue> serializerFactory, [NotNull] Type valueType, TSerializedValue serializedValue)
    {
        if (serializerFactory == null) throw new ArgumentNullException(nameof(serializerFactory));
        if (serializedValue == null) throw new ArgumentNullException(nameof(serializedValue));

        try
        {
            new Action<ISerializerFactory<object>, object>(InternalValidate<object, object>)
                    .CreateGenericMethod(typeof(TSerializedValue), valueType)
                    .Invoke(null, new object[] { serializerFactory, serializedValue });
        }
        catch (TargetInvocationException ex)
        {
            throw ex.InnerException;
        }
    }

    private static void InternalValidate<TSerializedValue, TValue>([NotNull] this ISerializerFactory<TSerializedValue> serializerFactory, TSerializedValue serializedValue)
    {
        if (serializerFactory == null) throw new ArgumentNullException(nameof(serializerFactory));
        if (serializedValue == null) throw new ArgumentNullException(nameof(serializedValue));

        serializerFactory.Create<TValue>().Deserialize(serializedValue);
    }
}
