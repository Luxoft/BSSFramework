namespace Framework.Configurator.Models;

public class FullRoleDto : EntityDto
{
    public bool IsVirtual { get; set; }

    public List<RoleContextDto> Contexts { get; set; }
}
