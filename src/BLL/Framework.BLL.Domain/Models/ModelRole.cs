namespace Framework.BLL.Domain.Models;

/// <summary>
/// Роль модели в системе
/// </summary>
public record ModelRole(string Name, DirectMode.DirectMode DirectMode)
{
    public override string ToString() => this.Name;



    public static readonly ModelRole Create = new(nameof (Create), Domain.DirectMode.DirectMode.In);

    public static readonly ModelRole Extended = new(nameof(Extended), Domain.DirectMode.DirectMode.Out | Domain.DirectMode.DirectMode.In);

    public static readonly ModelRole Change = new(nameof(Change), Domain.DirectMode.DirectMode.Out | Domain.DirectMode.DirectMode.In);

    public static readonly ModelRole MassChange = new(nameof(MassChange), Domain.DirectMode.DirectMode.Out | Domain.DirectMode.DirectMode.In);

    public static readonly ModelRole Format = new(nameof(Format), Domain.DirectMode.DirectMode.In);

    public static readonly ModelRole Filter = new(nameof(Filter), Domain.DirectMode.DirectMode.In);

    public static readonly ModelRole IntegrationSave = new(nameof(IntegrationSave), Domain.DirectMode.DirectMode.In);
}
