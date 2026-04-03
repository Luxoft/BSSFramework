using System.ComponentModel.DataAnnotations.Schema;

using Framework.Application.Domain;

namespace SampleSystem.WebApiCore.Domain;

[Table(nameof(Employee), Schema = "app")]
public class Employee : IIdentityObject<Guid>
{
    public Guid Id { get; set; }

    public string Login { get; set; } = null!;

    public bool Active { get; set; }

    public virtual BusinessUnit? CoreBusinessUnit { get; set; }
}
