using AttachmentsSampleSystem.Domain;

namespace AttachmentsSampleSystem.BLL
{
    public partial class AttachmentsSampleSystemSecurityService
    {
        public override AttachmentsSampleSystemSecurityPath<BusinessUnit> GetBusinessUnitSecurityPath()
        {
            return AttachmentsSampleSystemSecurityPath<BusinessUnit>.Create(v => v);
        }

        public override AttachmentsSampleSystemSecurityPath<Employee> GetEmployeeSecurityPath()
        {
            return AttachmentsSampleSystemSecurityPath<Employee>.Create(v => v.CoreBusinessUnit);
        }
    }
}
