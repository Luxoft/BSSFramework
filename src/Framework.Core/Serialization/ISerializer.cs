using System;
using System.Linq.Expressions;

namespace Framework.Core.Serialization;

/// <summary>
/// Сериализатор
/// </summary>
/// <typeparam name="TSerializedValue">Тип сериализованного значения</typeparam>
/// <typeparam name="TValue">Сериализуемый тип</typeparam>
public interface ISerializer<TSerializedValue, TValue> :
        IParser<TSerializedValue, TValue>, IFormatter<TValue, TSerializedValue>
{
}

/// <summary>
/// Сериализатор
/// </summary>
/// <typeparam name="TSerializedValue">Тип сериализованного значения</typeparam>
/// <typeparam name="TValue">Сериализуемый тип</typeparam>
public class Serializer<TSerializedValue, TValue> : ISerializer<TSerializedValue, TValue>
{
    private readonly Func<TSerializedValue, TValue> deserializeFunc;

    private readonly Func<TValue, TSerializedValue> serializeFunc;


    public Serializer(IParser<TSerializedValue, TValue> parser, IFormatter<TValue, TSerializedValue> serializer)
            : this(parser.Parse, serializer.Format)
    {
    }

    public Serializer(Func<TSerializedValue, TValue> deserializeFunc, Func<TValue, TSerializedValue> serializeFunc)
    {
        if (deserializeFunc == null) throw new ArgumentNullException(nameof(deserializeFunc));
        if (serializeFunc == null) throw new ArgumentNullException(nameof(serializeFunc));

        this.deserializeFunc = deserializeFunc;
        this.serializeFunc = serializeFunc;
    }


    public TValue Parse(TSerializedValue input)
    {
        return this.deserializeFunc(input);
    }

    public TSerializedValue Format(TValue value)
    {
        return this.serializeFunc(value);
    }


    /// <summary>
    /// Сереализатор между типами по умолчанию
    /// </summary>
    public static Serializer<TSerializedValue, TValue> Default => DefaultHelper.Value;

    private static class DefaultHelper
    {
        public static readonly Serializer<TSerializedValue, TValue> Value = GetValue();

        private static Serializer<TSerializedValue, TValue> GetValue()
        {
            if (typeof(TSerializedValue) == typeof(string))
            {
                var parser = new Parser<string, TValue>(ParserHelper.GetParseFunc<TValue>());

                var formatter = new Formatter<TValue, string>(v => v == null ? null : v.ToString());

                return (Serializer<TSerializedValue, TValue>)(object)new Serializer<string, TValue>(parser, formatter);
            }
            else if (typeof(TValue).IsEnum && Enum.GetUnderlyingType(typeof(TValue)) == typeof(TSerializedValue))
            {
                var parseFunc = Expression.Parameter(typeof(TSerializedValue)).Pipe(param => Expression.Lambda<Func<TSerializedValue, TValue>>(Expression.Convert(param, typeof(TValue)), param)).Compile();

                var formatFunc = Expression.Parameter(typeof(TValue)).Pipe(param => Expression.Lambda<Func<TValue, TSerializedValue>>(Expression.Convert(param, typeof(TSerializedValue)), param)).Compile();

                return new Serializer<TSerializedValue, TValue>(new Parser<TSerializedValue, TValue>(parseFunc), new Formatter<TValue, TSerializedValue>(formatFunc));
            }
            else
            {
                throw new Exception($"Can't convert {typeof(TValue)} to {typeof(TSerializedValue)}");
            }
        }
    }
}
