namespace Framework.Core.Serialization;

public static class SerializerExtensions
{
    public static TSerializedValue Serialize<TSerializedValue, TValue>(this ISerializer<TSerializedValue, TValue> serializer, TValue value)
    {
        if (serializer == null) throw new ArgumentNullException(nameof(serializer));

        return serializer.Format(value);
    }

    public static TValue Deserialize<TSerializedValue, TValue>(this ISerializer<TSerializedValue, TValue> serializer, TSerializedValue value)
    {
        if (serializer == null) throw new ArgumentNullException(nameof(serializer));

        return serializer.Parse(value);
    }

    public static ISerializer<TValue, TResult> WithParseCache<TValue, TResult>(this ISerializer<TValue, TResult> serializer)
    {
        if (serializer == null) throw new ArgumentNullException(nameof(serializer));

        return new Serializer<TValue, TResult>(
                                               FuncHelper.Create((TValue value) => serializer.Deserialize(value)).WithCache(), serializer.Serialize);
    }

    //public static ISerializer<TValue, TResult> WithLock<TValue, TResult>(ISerializer<TValue, TResult> serializer)
    //{
    //    if (serializer == null) throw new ArgumentNullException("serializer");

    //    return new Serializer<TValue, TResult>(FuncHelper.Create((TValue value) => serializer.Format(value)).WithLock());
    //}

    public static ISerializer<TS2, TValue> Select<TS1, TS2, TValue>(this ISerializer<TS1, TValue> serializer, ISerializer<TS2, TS1> nextSerializer)
    {
        if (serializer == null) throw new ArgumentNullException(nameof(serializer));
        if (nextSerializer == null) throw new ArgumentNullException(nameof(nextSerializer));

        return new Serializer<TS2, TValue>(v => serializer.Parse(nextSerializer.Parse(v)), v => nextSerializer.Format(serializer.Format(v)));
    }
}
