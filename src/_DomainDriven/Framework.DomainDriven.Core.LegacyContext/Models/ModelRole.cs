using System.Linq.Expressions;

using Framework.Core;

namespace Framework.DomainDriven;

/// <summary>
/// Роль модели в системе
/// </summary>
public class ModelRole : IEquatable<ModelRole>
{
    public ModelRole(Expression<Func<ModelRole>> getNameExpr, DirectMode directMode)
            : this(getNameExpr.GetStaticMemberName(), directMode)
    {
    }

    public ModelRole(string name, DirectMode directMode)
    {
        this.Name = name;
        this.DirectMode = directMode;
    }

    public string Name { get; }

    public DirectMode DirectMode { get; }


    public override string ToString()
    {
        return this.Name;
    }

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




    public static readonly ModelRole Create = new ModelRole(() => Create, DirectMode.In);

    public static readonly ModelRole Extended = new ModelRole(() => Extended, DirectMode.Out | DirectMode.In);

    public static readonly ModelRole Change = new ModelRole(() => Change, DirectMode.Out | DirectMode.In);

    public static readonly ModelRole MassChange = new ModelRole(() => MassChange, DirectMode.Out | DirectMode.In);

    public static readonly ModelRole Format = new ModelRole(() => Format, DirectMode.In);

    public static readonly ModelRole Filter = new ModelRole(() => Filter, DirectMode.In);

    public static readonly ModelRole IntegrationSave = new ModelRole(() => IntegrationSave, DirectMode.In);
}
