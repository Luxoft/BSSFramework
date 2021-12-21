using JetBrains.Annotations;

namespace Framework.Workflow.BLL
{
    public partial interface ITransitionBLL
    {
        void Recalculate([NotNull]Domain.Definition.Transition transition);
    }
}
