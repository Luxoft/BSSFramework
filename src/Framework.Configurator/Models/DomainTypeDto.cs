using System.Collections.Generic;

namespace Framework.Configurator.Models
{
    public class DomainTypeDto : EntityDto
    {
        public string Namespace
        {
            get;
            set;
        }

        public List<EntityDto> Operations
        {
            get;
            set;
        }
    }
}
