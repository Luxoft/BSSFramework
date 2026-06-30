using System.Reflection;

using Framework.CodeGeneration.DTOGenerator.FileTypes;

namespace Framework.CodeGeneration.DTOGenerator.Map;

public class GeneratePropertyMap(PropertyInfo property, Type elementType, RoleFileType? elementFileType, bool isCollection, bool isNullable, bool isDetail)
{
    public PropertyInfo Property { get; } = property ?? throw new ArgumentNullException(nameof(property));

    public Type ElementType { get; } = elementType ?? throw new ArgumentNullException(nameof(elementType));

    public bool IsCollection { get; } = isCollection;

    public bool IsNullable { get; } = isNullable;

    public bool IsDetail { get; } = isDetail;

    public RoleFileType? ElementFileType { get; } = elementFileType;

    public override string ToString() => $"Name: {this.Property.Name} | ElementFileType: {this.ElementFileType} | ElementType: {this.ElementType}";
}

