using System;
using Framework.Core;

namespace Framework.Workflow.BLL
{
    public partial class TransitionBLL
    {
        public void Recalculate(Domain.Definition.Transition transition)
        {
            if (transition == null) throw new ArgumentNullException(nameof(transition));

            transition.From = transition.TriggerEvent.Maybe(e =>

                this.Context.GetNestedEvent(e).SourceState);
        }


        public override void Save(Domain.Definition.Transition transition)
        {
            if (transition == null) throw new ArgumentNullException(nameof(transition));

            this.Recalculate(transition);

            base.Save(transition);
        }
    }
}