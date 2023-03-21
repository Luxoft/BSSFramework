using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using Framework.Core;

using JetBrains.Annotations;

namespace Framework.Projection.Lambda;

/// <summary>
/// Окружение для проекций формируемых через лямбды
/// </summary>
public abstract class ProjectionLambdaEnvironment : ProjectionEnvironmentBase
{
    private IReadOnlyList<IProjection> projections;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="projectionSource">Источник проекций</param>
    protected ProjectionLambdaEnvironment([NotNull] IProjectionSource projectionSource)
    {
        if (projectionSource == null) throw new ArgumentNullException(nameof(projectionSource));

        this.ProjectionTypeResolver = LazyInterfaceImplementHelper.CreateProxy(() => this.CreateProjectionTypeResolver(projectionSource));
    }


    public ITypeResolver<IProjection> ProjectionTypeResolver { get; private set; }

    private ITypeResolver<IProjection> CreateProjectionTypeResolver(IProjectionSource projectionSource)
    {
        var generateTypeResolver = new GenerateTypeResolver(this);

        this.ProjectionTypeResolver = generateTypeResolver;

        this.projections = projectionSource.Pipe(v => new LinkAllProjectionSource(v))
                                           .Pipe(v => new VerifyUniqueProjectionSource(v))
                                           .Pipe(v => this.UseDependencySecurity ? (IProjectionSource)v : new CreateSecurityNodesProjectionSource(v, this))
                                           .Pipe(v => new CreateAutoNodesProjectionSource(v, this))
                                           .Pipe(v => new InjectMissedParentsProjectionSource(v))
                                           .Pipe(v => new InjectAttributesProjectionSource(v, this))
                                           .GetProjections()
                                           .ToList();

        return TypeResolverHelper.Create(this.projections.ToDictionary(projection => projection, generateTypeResolver.Resolve));
    }

    public Type GetProjectionTypeByRole(Type sourceType, ProjectionRole role)
    {
        if (sourceType == null) throw new ArgumentNullException(nameof(sourceType));
        if (!Enum.IsDefined(typeof(ProjectionRole), role)) throw new InvalidEnumArgumentException(nameof(role), (int)role, typeof(ProjectionRole));

        return this.ProjectionTypeResolver.Resolve(this.projections.GetProjectionByRole(sourceType, role));
    }

    public Type GetSecurityProjectionType(Type sourceType)
    {
        if (sourceType == null) throw new ArgumentNullException(nameof(sourceType));

        return this.ProjectionTypeResolver.Resolve(this.projections.GetSecurityProjection(sourceType));
    }

    /// <summary>
    /// Получение типа свойства из ссылки на тип
    /// </summary>
    /// <param name="typeReferenceBase">Ссылка на тип</param>
    /// <returns></returns>
    public Type BuildPropertyType([NotNull] TypeReferenceBase typeReferenceBase)
    {
        if (typeReferenceBase == null) { throw new ArgumentNullException(nameof(typeReferenceBase)); }

        switch (typeReferenceBase)
        {
            case TypeReferenceBase.FixedTypeReference fixRef:
            {
                if (this.IsPersistent(fixRef.PropertyType))
                {
                    throw new PropertyProjectionNotInitializedException(fixRef.PropertyType);
                }

                return fixRef.PropertyType;
            }

            case TypeReferenceBase.BuildTypeReference buildTypeRef:
            {
                if (buildTypeRef.ElementProjection == null && this.IsPersistent(buildTypeRef.ElementType))
                {
                    throw new PropertyProjectionNotInitializedException(buildTypeRef.ElementType);
                }

                var elementProjectionType = (GeneratedType)buildTypeRef.ElementProjection.Maybe(v => this.ProjectionTypeResolver.Resolve(v, true));

                var elementType = elementProjectionType ?? buildTypeRef.ElementType;

                return buildTypeRef.IsNullable ? typeof(Nullable<>).CachedMakeGenericType(elementType) : buildTypeRef.CollectionType.SafeMakeProjectionCollectionType(elementType);
            }

            default:
                throw new ArgumentOutOfRangeException(nameof(typeReferenceBase));
        }
    }

    internal Type BuildPropertyType([NotNull] TypeReferenceBase typeReferenceBase, GeneratedType generatedProjection, string propertyName)
    {
        if (typeReferenceBase == null) throw new ArgumentNullException(nameof(typeReferenceBase));
        if (generatedProjection == null) throw new ArgumentNullException(nameof(generatedProjection));
        if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

        try
        {
            return this.BuildPropertyType(typeReferenceBase);
        }
        catch (PropertyProjectionNotInitializedException ex)
        {
            throw new PropertyProjectionNotInitializedException(ex.ElementType, generatedProjection.Name, propertyName);
        }
    }

    /// <summary>
    /// Получение провайдера, вычисляющего все атрибуты проекции
    /// </summary>
    /// <param name="projection">Проекция</param>
    /// <returns></returns>
    protected internal virtual IAttributeSource GetProjectionTypeAttributeSource([NotNull] IProjection projection)
    {
        if (projection == null) { throw new ArgumentNullException(nameof(projection)); }

        return new ProjectionTypeAttributeSource(this, projection);
    }

    /// <summary>
    /// Получение провайдера, вычисляющего все атрибуты проекционного свойства
    /// </summary>
    /// <param name="projectionProperty">Свойство проекции</param>
    /// <returns></returns>
    protected internal virtual IAttributeSource GetProjectionPropertyAttributeSource([NotNull] IProjectionProperty projectionProperty)
    {
        if (projectionProperty == null) { throw new ArgumentNullException(nameof(projectionProperty)); }

        return new ProjectionPropertyAttributeSource(this, projectionProperty);
    }

    /// <summary>
    /// Получение провайдера, вычисляющего все атрибуты кастомного проекционного свойства
    /// </summary>
    /// <param name="projectionCustomProperty">Кастомное проекционное свойство</param>
    /// <returns></returns>
    protected internal virtual IAttributeSource GetProjectionCustomPropertyAttributeSource([NotNull] IProjectionCustomProperty projectionCustomProperty)
    {
        if (projectionCustomProperty == null) { throw new ArgumentNullException(nameof(projectionCustomProperty)); }

        return new ProjectionCustomPropertyAttributeSource(this, projectionCustomProperty);
    }

    public static ProjectionLambdaEnvironment Create(
            [NotNull] IProjectionSource projectionSource,
            [NotNull] string assemblyName,
            [NotNull] string assemblyFullName,
            [NotNull] Type domainObjectBaseType,
            [NotNull] Type persistentDomainObjectBaseType,
            [NotNull] string @namespace,
            bool useDependencySecurity = true)
    {
        return new DefaultProjectionLambdaEnvironment(
                                                      projectionSource,
                                                      assemblyName,
                                                      assemblyFullName,
                                                      domainObjectBaseType,
                                                      persistentDomainObjectBaseType,
                                                      @namespace,
                                                      useDependencySecurity);
    }
}
