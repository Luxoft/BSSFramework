namespace Framework.Configurator.Models;

public class BusinessRoleDetailsDto
{
    public List<OperationDto> Operations { get; set; }

    public List<string> Principals { get; set; }
}
