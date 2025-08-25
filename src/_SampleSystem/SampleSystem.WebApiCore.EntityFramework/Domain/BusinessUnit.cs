using System.ComponentModel.DataAnnotations.Schema;

using SecuritySystem;

namespace SampleSystem.Domain;

[Table(nameof(BusinessUnit), Schema = "app")]
public class BusinessUnit : ISecurityContext
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual BusinessUnit? Parent { get; set; }
}
