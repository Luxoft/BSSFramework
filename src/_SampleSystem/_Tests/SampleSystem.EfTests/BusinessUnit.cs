using System.ComponentModel.DataAnnotations.Schema;

using Framework.SecuritySystem;

namespace SampleSystem.EfTests;

[Table(nameof(BusinessUnit), Schema = "app")]
public class BusinessUnit : ISecurityContext
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;
}
