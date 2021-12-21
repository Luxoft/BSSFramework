using Framework.DomainDriven.BLL;

namespace Framework.DomainDriven.BLL
{
    public class EmptyDALListenter : IDALListener
    {
        public static readonly EmptyDALListenter Instance = new EmptyDALListenter();

        private EmptyDALListenter()
        {
        }

        public void Process(DALChangesEventArgs eventArgs)
        {
        }
    }
}