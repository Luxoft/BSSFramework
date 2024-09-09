namespace Framework.Configurator.Models;

public class PermissionDto
{
    public Guid Id { get; set; }

    public string Role { get; set; }

    public Guid RoleId { get; set; }

    public string Comment { get; set; }

    public List<ContextDto> Contexts { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime StartDate { get; set; }

    public bool IsVirtual { get; set; }
}
