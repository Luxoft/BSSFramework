using System.Linq.Expressions;

using Framework.Core;

namespace Framework.BLL.Domain.Models;

/// <summary>
/// Роль модели в системе
/// </summary>
public class ModelRole(string name, DirectMode.DirectMode directMode) : IEquatable<ModelRole>
{
    public ModelRole(Expression<Func<ModelRole>> getNameExpr, DirectMode.DirectMode directMode)
            : this(getNameExpr.GetStaticMemberName(), directMode)
    {
    }

    public string Name { get; } = name;

    public DirectMode.DirectMode DirectMode { get; } = directMode;

    public override string ToString() => this.Name;

    public bool Equals(ModelRole other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return string.Equals(this.Name, other.Name) && this.DirectMode == other.DirectMode;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;

        return this.Equals((ModelRole)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((this.Name != null ? this.Name.GetHashCode() : 0) * 397) ^ (int)this.DirectMode;
        }
    }




    public static readonly ModelRole Create = new(() => Create, Domain.DirectMode.DirectMode.In);

    public static readonly ModelRole Extended = new(() => Extended, Domain.DirectMode.DirectMode.Out | Domain.DirectMode.DirectMode.In);

    public static readonly ModelRole Change = new(() => Change, Domain.DirectMode.DirectMode.Out | Domain.DirectMode.DirectMode.In);

    public static readonly ModelRole MassChange = new(() => MassChange, Domain.DirectMode.DirectMode.Out | Domain.DirectMode.DirectMode.In);

    public static readonly ModelRole Format = new(() => Format, Domain.DirectMode.DirectMode.In);

    public static readonly ModelRole Filter = new(() => Filter, Domain.DirectMode.DirectMode.In);

    public static readonly ModelRole IntegrationSave = new(() => IntegrationSave, Domain.DirectMode.DirectMode.In);
}
