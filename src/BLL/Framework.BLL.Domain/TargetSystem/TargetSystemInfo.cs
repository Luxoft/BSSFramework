namespace Framework.BLL.Domain.TargetSystem;

public record TargetSystemInfo
{
    public required string Name { get; init; }

    public required Guid Id { get; init; }

    public required TargetSystemDomainInfo Domain { get; init; }

    public static TargetSystemInfo Base { get; } = new() { Name = nameof(Base), Id = new Guid("{E197EEA5-5750-4990-9A4B-6E9ACBC95FA0}"), Domain = TargetSystemDomainInfo.Base };
}
