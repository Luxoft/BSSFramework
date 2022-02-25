using System;

using Framework.Configuration.WebApi;
using Framework.DomainDriven.DTOGenerator.TypeScript.Facade;

namespace Framework.Configuration.ClientGenerate
{
    public class MainFacadePolicyBuilder : TypeScriptMethodPolicyBuilder<ConfigSLJsonController>
    {
        public MainFacadePolicyBuilder()
        {
            this.AddSystemConstantMethods();
            this.AddCodeFirstSubscriptionMethods();
            this.AddExceptionMessageMethods();
            this.AddSequenceMethods();
            this.AddTargetSystemMethods();

            this.AddUserActionMethods();

            this.AddAttachmentMethods();
            this.AddUnsortedMethods();
        }

        private void AddSystemConstantMethods()
        {
            this.Add(facade => facade.GetSimpleSystemConstants());

            this.Add(facade => facade.GetSimpleSystemConstant(default));
            this.Add(facade => facade.GetFullSystemConstantsByIdents(default));
            this.Add(facade => facade.GetFullSystemConstantsByRootFilter(default));

            this.Add(facade => facade.GetFullSystemConstant(default));
            this.Add(facade => facade.GetRichSystemConstant(default));
            this.Add(facade => facade.SaveSystemConstant(default));
        }

        private void AddCodeFirstSubscriptionMethods()
        {
            this.Add(facade => facade.GetSimpleCodeFirstSubscriptions());

            this.Add(facade => facade.GetSimpleCodeFirstSubscription(default));
            this.Add(facade => facade.GetFullCodeFirstSubscriptionsByIdents(default));
            this.Add(facade => facade.GetFullCodeFirstSubscriptionsByRootFilter(default));

            this.Add(facade => facade.GetFullCodeFirstSubscription(default));
            this.Add(facade => facade.GetRichCodeFirstSubscription(default));
            this.Add(facade => facade.SaveCodeFirstSubscription(default));
        }

        private void AddExceptionMessageMethods()
        {
            this.Add(facade => facade.GetFullExceptionMessagesByIdents(default));
            this.Add(facade => facade.GetFullExceptionMessagesByRootFilter(default));

            this.Add(facade => facade.GetRichExceptionMessage(default));
            this.Add(facade => facade.SaveExceptionMessage(default));
        }
        
        private void AddSequenceMethods()
        {
            this.Add(facade => facade.GetFullSequencesByIdents(default));
            this.Add(facade => facade.GetFullSequencesByRootFilter(default));

            this.Add(facade => facade.GetRichSequence(default));

            this.Add(facade => facade.CreateSequence(default));
            this.Add(facade => facade.SaveSequence(default));
            this.Add(facade => facade.RemoveSequence(default));
        }

        private void AddTargetSystemMethods()
        {
            this.Add(facade => facade.GetSimpleTargetSystems());
            this.Add(facade => facade.GetSimpleTargetSystemsByRootFilter(default));

            this.Add(facade => facade.GetFullTargetSystemsByIdents(default));
            this.Add(facade => facade.GetFullTargetSystemsByRootFilter(default));

            this.Add(facade => facade.GetRichTargetSystem(default));

            this.Add(facade => facade.SaveTargetSystem(default));
        }

        private void AddUserActionMethods()
        {
            this.Add(facade => facade.GetFullUserActionsByIdents(default));

            this.Add(facade => facade.GetFullUserActionObjects());

            this.Add(facade => facade.GetFullUserActionObjectsByRootFilter(default));

            this.Add(facade => facade.CreateUserAction(default));
        }


        private void AddAttachmentMethods()
        {
            this.Add(facade => facade.GetSimpleDomainTypeByPath(default));
            this.Add(facade => facade.GetSimpleAttachmentsByContainerReference (default));

            this.Add(facade => facade.SaveAttachment(default));
            this.Add(facade => facade.RemoveAttachment(default));

            this.Add(facade => facade.GetSimpleAttachment(default));
            this.Add(facade => facade.GetRichAttachmentTagsEx(default));
            this.Add(facade => facade.SetAttachmentTags(default));
        }

        private void AddUnsortedMethods()
        {
            this.Add(facade => facade.GetSimpleDomainTypesByRootFilter(default));
            this.Add(facade => facade.ForceDomainTypeEvent(default));

            this.Add(facade => facade.GetEventQueueProcessingState());
            this.Add(facade => facade.GetModificationQueueProcessingState());
            this.Add(facade => facade.GetNotificationQueueProcessingState());

            this.Add(facade => facade.GetControlSettings(default));
            this.Add(facade => facade.SaveControlSettingsList(default));
            this.Add(facade => facade.RemoveControlSettingsCollection(default));

            this.Add(facade => facade.ProcessModifications(default));

            this.Add(facade => facade.GetFullDomainTypes());
            this.Add(facade => facade.GetRichDomainType(default));

            this.Add(facade => facade.GetFullUserActionObjectsByIdents(default));
            this.Add(facade => facade.GetSimpleDomainTypes());
        }
    }
}
