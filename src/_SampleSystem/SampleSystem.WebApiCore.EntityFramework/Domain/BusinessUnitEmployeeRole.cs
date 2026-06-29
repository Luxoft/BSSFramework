using System.ComponentModel.DataAnnotations.Schema;

using Framework.Application.Domain;

namespace SampleSystem.WebApiCore.Domain;

[Table(nameof(BusinessUnitEmployeeRole), Schema = "app")]
public class BusinessUnitEmployeeRole : IIdentityObject<Guid>
{
    public Guid Id { get; set; }

    public virtual BusinessUnit BusinessUnit { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;

    public BusinessUnitEmployeeRoleType Role { get; set; }
}

