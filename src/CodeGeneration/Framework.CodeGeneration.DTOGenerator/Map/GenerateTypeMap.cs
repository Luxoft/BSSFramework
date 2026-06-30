using Framework.CodeGeneration.DTOGenerator.FileTypes;

namespace Framework.CodeGeneration.DTOGenerator.Map;

public class GenerateTypeMap
{
    public GenerateTypeMap(Type domainType, DTOFileType fileType, IEnumerable<GeneratePropertyMap> properties)
    {
        if (properties is null) throw new ArgumentNullException(nameof(properties));

        this.DomainType = domainType ?? throw new ArgumentNullException(nameof(domainType));
        this.FileType = fileType;
        this.Properties = properties.ToArray();
    }

    public Type DomainType { get; }

    public DTOFileType FileType { get; }

    public IReadOnlyCollection<GeneratePropertyMap> Properties { get; }

    /// <summary>
    /// Проверка на использование подтипа в каком-либо свойстве
    /// </summary>
    /// <param name="elementType">Тип элемента свойства</param>
    /// <param name="elementFileType">Тип DTO свойства</param>
    /// <param name="isDetail">Свойство является деталью</param>
    /// <returns></returns>
    public bool UsedDetailRole(Type elementType, RoleFileType? elementFileType, bool? isDetail) =>
        this.GetNotSelfProperties().Any(prop =>
                                            prop.ElementType == elementType
                                            && (elementFileType is null || prop.ElementFileType is null || prop.ElementFileType == elementFileType)
                                            && (isDetail is null || prop.IsDetail == isDetail.Value));

    private IEnumerable<GeneratePropertyMap> GetNotSelfProperties() => this.Properties.Where(prop => prop.ElementType != this.DomainType || prop.ElementFileType != this.FileType);

    public override string ToString() => $"DomainType: {this.DomainType.Name} | FileType: {this.FileType}";
}

