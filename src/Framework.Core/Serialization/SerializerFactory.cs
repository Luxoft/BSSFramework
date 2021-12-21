namespace Framework.Core.Serialization
{
    public abstract class SerializerFactory : ISerializerFactory<string>
    {
        public abstract ISerializer<string, T> Create<T>();


        public static readonly SerializerFactory Default = new DefaultSerializerFactory();


        public class DefaultSerializerFactory : SerializerFactory
        {
            public override ISerializer<string, T> Create<T>()
            {
                return Serializer<string, T>.Default;
            }
        }
    }
}