using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;

using JetBrains.Annotations;

namespace Framework.Projection.Lambda;

/// <summary>
/// Источник атрибутов
/// </summary>
/// <typeparam name="TProjectionValue">Проекционный тип, содержащий атрибуты</typeparam>
public abstract class AttributeSourceBase<TProjectionValue> : IAttributeSource
        where TProjectionValue : class, IProjectionAttributeProvider
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="environment">Окружение</param>
    /// <param name="projectionValue">Проекционный объект</param>
    protected AttributeSourceBase([NotNull] ProjectionLambdaEnvironment environment, [NotNull] TProjectionValue projectionValue)
    {
        this.Environment = environment ?? throw new ArgumentNullException(nameof(environment));
        this.ProjectionValue = projectionValue ?? throw new ArgumentNullException(nameof(projectionValue));
    }

    /// <summary>
    /// Окружение
    /// </summary>
    public ProjectionLambdaEnvironment Environment { get; }


    /// <summary>
    /// Проекционный объект
    /// </summary>
    public TProjectionValue ProjectionValue { get; }

    /// <inheritdoc />
    public virtual IEnumerable<Attribute> GetAttributes()
    {
        var singleAttrTypes = this.ProjectionValue.Attributes.Select(attr => attr.GetType()).Where(attrType => !attrType.HasAttribute<AttributeUsageAttribute>(usageAttr => usageAttr.AllowMultiple)).ToHashSet();

        return this.GetDefaultAttributes().Where(attr => !singleAttrTypes.Contains(attr.GetType())).Concat(this.ProjectionValue.Attributes);
    }

    /// <summary>
    /// Получение дефолтовых атрибутов объекта
    /// </summary>
    /// <returns></returns>
    protected abstract IEnumerable<Attribute> GetDefaultAttributes();
}
