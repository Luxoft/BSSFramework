using System.Collections.Generic;

namespace Framework.Configurator.Models;

public class ContextDto : EntityDto
{
    public List<EntityDto> Entities
    {
        get;
        set;
    }
}