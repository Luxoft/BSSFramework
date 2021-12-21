using System;

namespace Framework.Core.Serialization
{
    public static class Serializer
    {
        [Obsolete("v10 Not used")]
        public static readonly ISerializer<string, byte[]> Base64 = new Serializer<string, byte[]>(Convert.FromBase64String, Convert.ToBase64String);

        [Obsolete("v10 Not used")]
        public static readonly ISerializer<string, byte[]> Base85 = new Serializer<string, byte[]>(Base85Convert.Decode, Base85Convert.Encode);

        /// <summary>
        /// Получение дефолтового сериализатора
        /// </summary>
        /// <typeparam name="TSerializedValue">Тип сериализованного значения</typeparam>
        /// <typeparam name="TValue">Сериализуемый тип</typeparam>
        /// <returns></returns>
        public static ISerializer<TSerializedValue, TValue> GetDefault<TSerializedValue, TValue>() => Serializer<TSerializedValue, TValue>.Default;
    }
}
