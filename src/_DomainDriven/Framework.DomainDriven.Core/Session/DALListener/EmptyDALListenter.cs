using Framework.DomainDriven.BLL;

namespace Framework.DomainDriven.BLL
{
    public class EmptyDALListener : IDALListener
    {
        public static readonly EmptyDALListener Instance = new EmptyDALListener();

        private EmptyDALListener()
        {
        }

        public void Process(DALChangesEventArgs eventArgs)
        {
        }
    }
}
