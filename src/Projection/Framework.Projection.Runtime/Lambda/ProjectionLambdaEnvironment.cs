using System.Collections.Immutable;
using System.ComponentModel;

using CommonFramework;

using Framework.BLL.Services;
using Framework.Core;
using Framework.Core.TypeResolving;
using Framework.Projection._Extensions;
using Framework.ExtendedMetadata;
using Framework.Projection.Lambda._Extensions;
using Framework.Projection.Lambda.Exceptions;
using Framework.Projection.Lambda.ImplType;
using Framework.Projection.Lambda.ProjectionSource;
using Framework.Projection.Lambda.ProjectionSource._Base;
using Framework.Projection.Lambda.ProjectionSource.AttributeSource;

namespace Framework.Projection.Lambda;

/// <summary>
/// Окружение для проекций формируемых через лямбды
/// </summary>
public abstract class ProjectionLambdaEnvironment : ProjectionEnvironmentBase
{
    private ImmutableArray<IProjection> projections;

    protected ProjectionLambdaEnvironment(IProjectionSource projectionSource, IMetadataProxyProvider metadataProxyProvider, IPropertyPathService propertyPathService)
        : base(metadataProxyProvider)
    {
        this.PropertyPathService = propertyPathService;

        this.ProjectionTypeResolver = LazyInterfaceImplementHelper.CreateProxy(() => this.CreateProjectionTypeResolver(projectionSource));
    }

    public IPropertyPathService PropertyPathService { get; }

    public ITypeResolver<IProjection> ProjectionTypeResolver { get; private set; }

    private ITypeResolver<IProjection> CreateProjectionTypeResolver(IProjectionSource projectionSource)
    {
        var generateTypeResolver = new GenerateTypeResolver(this);

        this.ProjectionTypeResolver = generateTypeResolver;

        this.projections =
        [
            ..projectionSource.Pipe(v => new ExpandPathProjectionSource(this, v))
                              .Pipe(v => new LinkAllProjectionSource(v))
                              .Pipe(v => new VerifyUniqueProjectionSource(v))
                              .Pipe(v => this.UseDependencySecurity ? (IProjectionSource)v : new CreateSecurityNodesProjectionSource(this, v))
                              .Pipe(v => new CreateAutoNodesProjectionSource(this, v))
                              .Pipe(v => new InjectMissedParentsProjectionSource(this, v))
                              .Pipe(v => new InjectAttributesProjectionSource(this, v))
                              .GetProjections()
        ];

        return TypeResolverHelper.Create(this.projections.ToDictionary(projection => projection, this.ProjectionTypeResolver.Resolve));
    }

    public Type GetProjectionTypeByRole(Type sourceType, ProjectionRole role)
    {
        if (sourceType == null) throw new ArgumentNullException(nameof(sourceType));
        if (!Enum.IsDefined(typeof(ProjectionRole), role))
            throw new InvalidEnumArgumentException(nameof(role), (int)role, typeof(ProjectionRole));

        return this.ProjectionTypeResolver.Resolve(this.projections.GetProjectionByRole(sourceType, role));
    }

    public Type? GetSecurityProjectionType(Type sourceType)
    {
        if (sourceType == null) throw new ArgumentNullException(nameof(sourceType));

        return this.ProjectionTypeResolver.TryResolve(this.projections.GetSecurityProjection(sourceType));
    }

    /// <summary>
    /// Получение типа свойства из ссылки на тип
    /// </summary>
    /// <param name="typeReferenceBase">Ссылка на тип</param>
    /// <returns></returns>
    public Type BuildPropertyType(TypeReferenceBase typeReferenceBase)
    {
        if (typeReferenceBase == null)
        {
            throw new ArgumentNullException(nameof(typeReferenceBase));
        }

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

                var elementProjectionType =
                    (GeneratedType?)buildTypeRef.ElementProjection.Maybe(v => this.ProjectionTypeResolver.Resolve(v));

                var elementType = elementProjectionType ?? buildTypeRef.ElementType;

                return buildTypeRef.IsNullable
                           ? typeof(Nullable<>).CachedMakeGenericType(elementType)
                           : buildTypeRef.CollectionType.SafeMakeProjectionCollectionType(elementType);
            }

            default:
                throw new ArgumentOutOfRangeException(nameof(typeReferenceBase));
        }
    }

    internal Type BuildPropertyType(TypeReferenceBase typeReferenceBase, GeneratedType generatedProjection, string propertyName)
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
    protected internal virtual IAttributeSource GetProjectionTypeAttributeSource(IProjection projection)
    {
        if (projection == null)
        {
            throw new ArgumentNullException(nameof(projection));
        }

        return new ProjectionTypeAttributeSource(this, projection);
    }

    /// <summary>
    /// Получение провайдера, вычисляющего все атрибуты проекционного свойства
    /// </summary>
    /// <param name="projectionProperty">Свойство проекции</param>
    /// <returns></returns>
    protected internal virtual IAttributeSource GetProjectionPropertyAttributeSource(IProjectionProperty projectionProperty)
    {
        if (projectionProperty == null)
        {
            throw new ArgumentNullException(nameof(projectionProperty));
        }

        return new ProjectionPropertyAttributeSource(this, projectionProperty);
    }

    /// <summary>
    /// Получение провайдера, вычисляющего все атрибуты кастомного проекционного свойства
    /// </summary>
    /// <param name="projectionCustomProperty">Кастомное проекционное свойство</param>
    /// <returns></returns>
    protected internal virtual IAttributeSource GetProjectionCustomPropertyAttributeSource(
        IProjectionCustomProperty projectionCustomProperty)
    {
        if (projectionCustomProperty == null)
        {
            throw new ArgumentNullException(nameof(projectionCustomProperty));
        }

        return new ProjectionCustomPropertyAttributeSource(this, projectionCustomProperty);
    }
}
