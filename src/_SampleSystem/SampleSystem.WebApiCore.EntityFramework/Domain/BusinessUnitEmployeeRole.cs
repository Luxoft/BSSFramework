using System.ComponentModel.DataAnnotations.Schema;

using Framework.Application.Domain;

namespace SampleSystem.WebApiCore.Domain;

[Table(nameof(BusinessUnitEmployeeRole), Schema = "app")]
public class BusinessUnitEmployeeRole : IIdentityObject<Guid>
{
    public Guid Id { get; set; }

    public virtual BusinessUnit BusinessUnit { get; set; }

    public virtual Employee Employee { get; set; }

    public BusinessUnitEmployeeRoleType Role { get; set; }
}
