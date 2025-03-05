using System.ComponentModel.DataAnnotations.Schema;

using Framework.Persistent;

namespace SampleSystem.EfTests;

[Table(nameof(Employee), Schema = "app")]
public class Employee : IIdentityObject<Guid>
{
    public Guid Id { get; set; }

    public string Login { get; set; } = null!;

    public bool Active { get; set; }

    public virtual BusinessUnit? CoreBusinessUnit { get; set; }
}
