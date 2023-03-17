using System.Collections.Generic;

using Framework.Authorization.Domain;

namespace Framework.Configurator.Models
{
    public class OperationDetailsDto
    {
        public List<string> BusinessRoles
        {
            get;
            set;
        }

        public List<string> Principals
        {
            get;
            set;
        }
    }
}
