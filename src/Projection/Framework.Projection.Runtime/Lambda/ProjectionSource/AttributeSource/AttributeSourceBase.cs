using Framework.Core;

namespace Framework.Projection.Lambda.ProjectionSource.AttributeSource;

/// <summary>
/// Attribute source
/// </summary>
/// <typeparam name="TProjectionValue">Projection type that contains attributes</typeparam>
public abstract class AttributeSourceBase<TProjectionValue> : IAttributeSource
    where TProjectionValue : class, IProjectionAttributeProvider
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="environment">Environment</param>
    /// <param name="projectionValue">Projection object</param>
    protected AttributeSourceBase(ProjectionLambdaEnvironment environment, TProjectionValue projectionValue)
    {
        this.Environment = environment ?? throw new ArgumentNullException(nameof(environment));
        this.ProjectionValue = projectionValue ?? throw new ArgumentNullException(nameof(projectionValue));
    }

    /// <summary>
    /// Environment
    /// </summary>
    public ProjectionLambdaEnvironment Environment { get; }


    /// <summary>
    /// Projection object
    /// </summary>
    public TProjectionValue ProjectionValue { get; }

    /// <inheritdoc />
    public virtual IEnumerable<Attribute> GetAttributes()
    {
        var singleAttrTypes = this.ProjectionValue.Attributes.Select(attr => attr.GetType()).Where(attrType => !attrType.HasAttribute<AttributeUsageAttribute>(usageAttr => usageAttr.AllowMultiple)).ToHashSet();

        return this.GetDefaultAttributes().Where(attr => !singleAttrTypes.Contains(attr.GetType())).Concat(this.ProjectionValue.Attributes);
    }

    /// <summary>
    /// Getting default object attributes
    /// </summary>
    /// <returns></returns>
    protected abstract IEnumerable<Attribute> GetDefaultAttributes();
}
