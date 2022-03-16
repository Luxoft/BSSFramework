using AttachmentsSampleSystem.Domain.Inline;
using AttachmentsSampleSystem.IntegrationTests.__Support.Utils;

namespace AttachmentsSampleSystem.IntegrationTests.__Support.TestData
{
    public class TestDataInitialize : TestBase
    {
        public void TestData()
        {
            this.DataHelper.Environment = this.Environment;
            this.AuthHelper.Environment = this.Environment;

            this.AuthHelper.AddCurrentUserToAdmin();

            this.AuthHelper.SetUserRole(DefaultConstants.NOTIFICATION_ADMIN, new AttachmentsSampleSystemPermission(BusinessRole.SystemIntegration));
            this.AuthHelper.SetUserRole(DefaultConstants.INTEGRATION_USER, new AttachmentsSampleSystemPermission(BusinessRole.SystemIntegration));

            this.DataHelper.SaveLocation(id: DefaultConstants.LOCATION_PARENT_ID, name: DefaultConstants.LOCATION_PARENT_NAME);

            this.DataHelper.SaveEmployee(
                id: DefaultConstants.EMPLOYEE_MY_ID,
                nameEng:
                    new Fio
                    {
                        FirstName = DefaultConstants.EMPLOYEE_MY_NAME,
                        LastName = DefaultConstants.EMPLOYEE_MY_NAME
                    },
                login: DefaultConstants.EMPLOYEE_MY_LOGIN,
                isObjectRequired: false);

            this.DataHelper.SaveHRDepartment(
                                             DefaultConstants.HRDEPARTMENT_PARENT_ID,
                                             DefaultConstants.HRDEPARTMENT_PARENT_NAME);
        }
    }
}
