using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Persistent.Mapping;
using Framework.Security;

using JetBrains.Annotations;

namespace Framework.Projection.Lambda;

/// <summary>
/// Источник атрибутов для свойства проекции
/// </summary>
public class ProjectionPropertyAttributeSource : AttributeSourceBase<IProjectionProperty>
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="environment">Окружение</param>
    /// <param name="projectionProperty">Свойство проекции</param>
    public ProjectionPropertyAttributeSource([NotNull] ProjectionLambdaEnvironment environment, [NotNull] IProjectionProperty projectionProperty)
            : base(environment, projectionProperty)
    {
        this.ExpandPathHead = this.ProjectionValue.Path.TakeWhile(prop => this.Environment.IsPersistent(prop.PropertyType)).ToPropertyPath();

        this.CompositeTail = this.ProjectionValue.Path.SkipWhile(prop => this.Environment.IsPersistent(prop.PropertyType)).ToPropertyPath();

        if (this.ProjectionValue.Path.Count > 1)
        {
            if (this.ExpandPathHead.Any())
            {
                this.IsExpand = true;
            }
            else
            {
                this.IsPureComposite = true;
            }
        }
    }

    /// <summary>
    /// Тип на основе которого строится проекция
    /// </summary>
    public Type SourceType => this.ProjectionValue.SourceType;

    /// <summary>
    /// Свойство является виртуальным и раскрывается через сторонний доменный объект
    /// </summary>
    private bool IsExpand { get; }

    /// <summary>
    /// Свойство является целиком композитным
    /// </summary>
    private bool IsPureComposite { get; }

    /// <summary>
    /// Базовый путь, состоящий из переходов между доменными объектами
    /// </summary>
    private PropertyPath ExpandPathHead { get; }

    /// <summary>
    /// Остаточный путь, состоящий из переходов между неперсистентными объектами (примеры: Id, Name, Period.EndDate, Fio.LastName)
    /// </summary>
    private PropertyPath CompositeTail { get; }

    /// <inheritdoc />
    protected override IEnumerable<Attribute> GetDefaultAttributes()
    {
        return new Attribute[]
               {
                       this.TryCreateExpandPathAttributes(),
                       this.TryCreateCustomSerializationAttribute(),
                       this.CreateProjectionPropertyAttribute(),
                       this.TryCreateMappingAttribute(),
                       this.CreateMappingPropertyAttribute(),
                       this.TryCreateViewAccessAttribute()
               }.Where(attr => attr != null);
    }

    protected virtual ExpandPathAttribute TryCreateExpandPathAttributes()
    {
        if (this.IsExpand)
        {
            if (this.CompositeTail.IsEmpty)
            {
                var expandHeadPathPrefix = this.ExpandPathHead.SkipLast(1).Select(prop => $"{prop.Name}_Auto");
                var expandHeadPathPostfix = this.ExpandPathHead.Last().Name + "_Last_" + this.ProjectionValue.Name;

                var expandHeadPath = expandHeadPathPrefix.Concat(new[] { expandHeadPathPostfix });

                var expandPath = expandHeadPath.Join(".");

                return new ExpandPathAttribute(expandPath);
            }
            else
            {
                var expandHeadPath = this.ExpandPathHead.Select(prop => $"{prop.Name}_Auto");

                var isIdentityTail = this.CompositeTail.SingleMaybe().Select(this.Environment.IsIdentityProperty).GetValueOrDefault();

                var postfix = isIdentityTail ? string.Empty : "_Last_" + this.ProjectionValue.Name;

                var expandPath = expandHeadPath.Concat(new[] { this.CompositeTail.Concat(prop => prop.Name) }).Join(".") + postfix;

                return new ExpandPathAttribute(expandPath);
            }
        }
        else
        {
            return null;
        }
    }

    protected virtual CustomSerializationAttribute TryCreateCustomSerializationAttribute()
    {
        if (this.ProjectionValue.IgnoreSerialization || this.ProjectionValue.Type.ElementProjection.Maybe(proj => proj.Role != ProjectionRole.Default))
        {
            return new CustomSerializationAttribute(CustomSerializationMode.Ignore);
        }
        else
        {
            return null;
        }
    }

    protected virtual ProjectionPropertyAttribute CreateProjectionPropertyAttribute()
    {
        return new ProjectionPropertyAttribute(this.ProjectionValue.Role);
    }

    protected virtual DomainObjectAccessAttribute TryCreateViewAccessAttribute()
    {
        return this.ProjectionValue
                   .Path
                   .SelectMany(prop => prop.GetDomainObjectAccessAttributes())
                   .Where(attr => !(attr is EditDomainObjectAttribute))
                   .SingleMaybe()
                   .GetValueOrDefault();
    }


    private MappingPropertyAttribute CreateMappingPropertyAttribute()
    {
        return new MappingPropertyAttribute() { CanInsert = false, CanUpdate = false };
    }

    protected virtual MappingAttribute TryCreateMappingAttribute()
    {
        var externalTableName = this.TryGetExternalTableName();

        if (this.IsPureComposite)
        {
            return new MappingAttribute { ColumnName = this.ProjectionValue.Path.Concat(prop => prop.Name).ToStartLowerCase(), ExternalTableName = externalTableName };
        }
        else if (this.ProjectionValue.Path.IsEmpty)
        {
            return new MappingAttribute { ColumnName = this.Environment.IdentityProperty.Name, ExternalTableName = externalTableName };
        }
        else if (!this.IsExpand && !this.ProjectionValue.Type.IsCollection)
        {
            var singlePathProp = this.ProjectionValue.Path.Single();

            var propMapping = singlePathProp.GetCustomAttribute<MappingAttribute>();

            if (propMapping != null)
            {
                if (propMapping.ExternalTableName == null)
                {
                    propMapping.ExternalTableName = externalTableName;
                }

                return propMapping;
            }
            else
            {
                var isPersistent = this.Environment.PersistentDomainObjectBaseType.IsAssignableFrom(singlePathProp.PropertyType);

                var namePostfix = isPersistent ? this.Environment.IdentityProperty.Name : string.Empty;

                return new MappingAttribute { ColumnName = $"{singlePathProp.Name.ToStartLowerCase()}{namePostfix}", ExternalTableName = externalTableName };
            }
        }

        if (externalTableName != null)
        {
            return new MappingAttribute { ExternalTableName = externalTableName };
        }

        return null;
    }


    private string TryGetExternalTableName()
    {
        var property = this.ProjectionValue.Path.FirstOrDefault();

        if (property != null)
        {
            var topProperty = property.GetTopProperty();

            var baseTypes = this.SourceType.GetAllElements(t => t.BaseType).TakeWhile(t => t != topProperty.ReflectedType);

            var nonAbstractDeclType = new[] { topProperty.ReflectedType }.Concat(baseTypes.Reverse()).First(type => !type.IsAbstract);

            if (nonAbstractDeclType != this.SourceType)
            {
                var existsColumnAttribute = property.GetCustomAttribute<MappingAttribute>();

                var existsTableAttribute = nonAbstractDeclType.GetTableAttribute();

                return existsColumnAttribute?.ExternalTableName ?? existsTableAttribute?.Name ?? nonAbstractDeclType.Name;
            }
        }

        return null;
    }
}
