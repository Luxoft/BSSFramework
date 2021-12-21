using System;

namespace Framework.DomainDriven.Serialization
{
    /// <summary>
    /// Позволяет настраивать сериализацию свойств доменного типа в конкретный тип DTO
    /// По умолчанию для всех доменных типов генерируются все подходящие типы DTO, а также соответствующий мапинг из домена в ДТО или из ДТО в домен(в зависимости от вида ДТО - input или output).
    /// <see href="confluence/display/IADFRAME/CustomSerializationAttribute"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct, AllowMultiple = true)]
    public class CustomSerializationAttribute : Attribute
    {
        public readonly CustomSerializationMode Mode;

        public readonly DTORole Role;

        public CustomSerializationAttribute(CustomSerializationMode mode)
            : this(mode, DTORole.All)
        {
        }

        public CustomSerializationAttribute(CustomSerializationMode mode, DTORole role)
        {
            this.Mode = mode;
            this.Role = role;
        }
    }
}
