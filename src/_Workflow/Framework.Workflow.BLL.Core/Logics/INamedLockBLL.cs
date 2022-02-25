using Framework.Workflow.Domain;

namespace Framework.Workflow.BLL
{
    public partial interface INamedLockBLL
    {
        void CheckInit();

        void Lock(NamedLockOperation lockOperation);
    }
}
