using System;
using System.Reflection;

namespace Framework.DomainDriven.DTOGenerator
{
    public class GeneratePropertyMap
    {
        public GeneratePropertyMap(PropertyInfo property, Type elementType, RoleFileType elementFileType, bool isCollection, bool isNullable, bool isDetail)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));
            if (elementType == null) throw new ArgumentNullException(nameof(elementType));

            this.Property = property;
            this.ElementType = elementType;
            this.ElementFileType = elementFileType;
            this.IsCollection = isCollection;
            this.IsNullable = isNullable;
            this.IsDetail = isDetail;
        }

        public PropertyInfo Property { get; }

        public Type ElementType { get; }

        public bool IsCollection { get; }

        public bool IsNullable { get; }

        public bool IsDetail { get; }

        public RoleFileType ElementFileType { get; }

        public override string ToString()
        {
            return $"Name: {this.Property.Name} | ElementFileType: {this.ElementFileType} | ElementType: {this.ElementType}";
        }
    }
}