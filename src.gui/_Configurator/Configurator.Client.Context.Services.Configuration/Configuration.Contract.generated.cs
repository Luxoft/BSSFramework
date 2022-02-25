﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Configurator.Client.Context.ConfigurationService
{
    
    
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ConfigurationService.ConfigSLJsonController")]
    public partial interface ConfigSLJsonController
    {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/CreateSequence", ReplyAction="http://tempuri.org/ConfigSLJsonController/CreateSequenceResponse")]
        System.IAsyncResult BeginCreateSequence(Framework.Configuration.Generated.DTO.SequenceCreateModelStrictDTO sequenceCreateModel, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/CreateUserAction", ReplyAction="http://tempuri.org/ConfigSLJsonController/CreateUserActionResponse")]
        System.IAsyncResult BeginCreateUserAction(Framework.Configuration.Generated.DTO.UserActionCreateModelStrictDTO userActionCreateModel, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/ForceDomainTypeEvent", ReplyAction="http://tempuri.org/ConfigSLJsonController/ForceDomainTypeEventResponse")]
        System.IAsyncResult BeginForceDomainTypeEvent(Framework.Configuration.Generated.DTO.DomainTypeEventModelStrictDTO domainTypeEventModel, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/GetControlSettings", ReplyAction="http://tempuri.org/ConfigSLJsonController/GetControlSettingsResponse")]
        System.IAsyncResult BeginGetControlSettings(string name, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/GetEventQueueProcessingState", ReplyAction="http://tempuri.org/ConfigSLJsonController/GetEventQueueProcessingStateResponse")]
        System.IAsyncResult BeginGetEventQueueProcessingState(System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/GetFullCodeFirstSubscription", ReplyAction="http://tempuri.org/ConfigSLJsonController/GetFullCodeFirstSubscriptionResponse")]
        System.IAsyncResult BeginGetFullCodeFirstSubscription(Framework.Configuration.Generated.DTO.CodeFirstSubscriptionIdentityDTO codeFirstSubscriptionIdentity, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/GetFullCodeFirstSubscriptionsByIdents", ReplyAction="http://tempuri.org/ConfigSLJsonController/GetFullCodeFirstSubscriptionsByIdentsRe" +
            "sponse")]
        System.IAsyncResult BeginGetFullCodeFirstSubscriptionsByIdents(System.Collections.ObjectModel.ObservableCollection<Framework.Configuration.Generated.DTO.CodeFirstSubscriptionIdentityDTO> codeFirstSubscriptionIdents, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/GetFullCodeFirstSubscriptionsByRootFilt" +
            "er", ReplyAction="http://tempuri.org/ConfigSLJsonController/GetFullCodeFirstSubscriptionsByRootFilt" +
            "erResponse")]
        System.IAsyncResult BeginGetFullCodeFirstSubscriptionsByRootFilter(Framework.Configuration.Generated.DTO.CodeFirstSubscriptionRootFilterModelStrictDTO filter, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/GetFullDomainTypes", ReplyAction="http://tempuri.org/ConfigSLJsonController/GetFullDomainTypesResponse")]
        System.IAsyncResult BeginGetFullDomainTypes(System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/GetFullExceptionMessagesByIdents", ReplyAction="http://tempuri.org/ConfigSLJsonController/GetFullExceptionMessagesByIdentsRespons" +
            "e")]
        System.IAsyncResult BeginGetFullExceptionMessagesByIdents(System.Collections.ObjectModel.ObservableCollection<Framework.Configuration.Generated.DTO.ExceptionMessageIdentityDTO> exceptionMessageIdents, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/GetFullExceptionMessagesByRootFilter", ReplyAction="http://tempuri.org/ConfigSLJsonController/GetFullExceptionMessagesByRootFilterRes" +
            "ponse")]
        System.IAsyncResult BeginGetFullExceptionMessagesByRootFilter(Framework.Configuration.Generated.DTO.ExceptionMessageRootFilterModelStrictDTO filter, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/GetFullSequencesByIdents", ReplyAction="http://tempuri.org/ConfigSLJsonController/GetFullSequencesByIdentsResponse")]
        System.IAsyncResult BeginGetFullSequencesByIdents(System.Collections.ObjectModel.ObservableCollection<Framework.Configuration.Generated.DTO.SequenceIdentityDTO> sequenceIdents, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/GetFullSequencesByRootFilter", ReplyAction="http://tempuri.org/ConfigSLJsonController/GetFullSequencesByRootFilterResponse")]
        System.IAsyncResult BeginGetFullSequencesByRootFilter(Framework.Configuration.Generated.DTO.SequenceRootFilterModelStrictDTO filter, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/GetFullSystemConstant", ReplyAction="http://tempuri.org/ConfigSLJsonController/GetFullSystemConstantResponse")]
        System.IAsyncResult BeginGetFullSystemConstant(Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO systemConstantIdentity, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/GetFullSystemConstantsByIdents", ReplyAction="http://tempuri.org/ConfigSLJsonController/GetFullSystemConstantsByIdentsResponse")]
        System.IAsyncResult BeginGetFullSystemConstantsByIdents(System.Collections.ObjectModel.ObservableCollection<Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO> systemConstantIdents, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/GetFullSystemConstantsByRootFilter", ReplyAction="http://tempuri.org/ConfigSLJsonController/GetFullSystemConstantsByRootFilterRespo" +
            "nse")]
        System.IAsyncResult BeginGetFullSystemConstantsByRootFilter(Framework.Configuration.Generated.DTO.SystemConstantRootFilterModelStrictDTO filter, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/GetFullTargetSystemsByIdents", ReplyAction="http://tempuri.org/ConfigSLJsonController/GetFullTargetSystemsByIdentsResponse")]
        System.IAsyncResult BeginGetFullTargetSystemsByIdents(System.Collections.ObjectModel.ObservableCollection<Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO> targetSystemIdents, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/GetFullTargetSystemsByRootFilter", ReplyAction="http://tempuri.org/ConfigSLJsonController/GetFullTargetSystemsByRootFilterRespons" +
            "e")]
        System.IAsyncResult BeginGetFullTargetSystemsByRootFilter(Framework.Configuration.Generated.DTO.TargetSystemRootFilterModelStrictDTO filter, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/GetFullUserActionObjects", ReplyAction="http://tempuri.org/ConfigSLJsonController/GetFullUserActionObjectsResponse")]
        System.IAsyncResult BeginGetFullUserActionObjects(System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/GetFullUserActionObjectsByIdents", ReplyAction="http://tempuri.org/ConfigSLJsonController/GetFullUserActionObjectsByIdentsRespons" +
            "e")]
        System.IAsyncResult BeginGetFullUserActionObjectsByIdents(System.Collections.ObjectModel.ObservableCollection<Framework.Configuration.Generated.DTO.UserActionObjectIdentityDTO> userActionObjectIdents, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/GetFullUserActionObjectsByRootFilter", ReplyAction="http://tempuri.org/ConfigSLJsonController/GetFullUserActionObjectsByRootFilterRes" +
            "ponse")]
        System.IAsyncResult BeginGetFullUserActionObjectsByRootFilter(Framework.Configuration.Generated.DTO.UserActionObjectRootFilterModelStrictDTO filter, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/GetFullUserActionsByIdents", ReplyAction="http://tempuri.org/ConfigSLJsonController/GetFullUserActionsByIdentsResponse")]
        System.IAsyncResult BeginGetFullUserActionsByIdents(System.Collections.ObjectModel.ObservableCollection<Framework.Configuration.Generated.DTO.UserActionIdentityDTO> userActionIdents, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/GetModificationQueueProcessingState", ReplyAction="http://tempuri.org/ConfigSLJsonController/GetModificationQueueProcessingStateResp" +
            "onse")]
        System.IAsyncResult BeginGetModificationQueueProcessingState(System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/GetNotificationQueueProcessingState", ReplyAction="http://tempuri.org/ConfigSLJsonController/GetNotificationQueueProcessingStateResp" +
            "onse")]
        System.IAsyncResult BeginGetNotificationQueueProcessingState(System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/GetRichAttachmentTagsEx", ReplyAction="http://tempuri.org/ConfigSLJsonController/GetRichAttachmentTagsExResponse")]
        System.IAsyncResult BeginGetRichAttachmentTagsEx(Framework.Configuration.Generated.DTO.AttachmentIdentityDTO attachmentIdentity, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/GetRichCodeFirstSubscription", ReplyAction="http://tempuri.org/ConfigSLJsonController/GetRichCodeFirstSubscriptionResponse")]
        System.IAsyncResult BeginGetRichCodeFirstSubscription(Framework.Configuration.Generated.DTO.CodeFirstSubscriptionIdentityDTO codeFirstSubscriptionIdentity, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/GetRichDomainType", ReplyAction="http://tempuri.org/ConfigSLJsonController/GetRichDomainTypeResponse")]
        System.IAsyncResult BeginGetRichDomainType(Framework.Configuration.Generated.DTO.DomainTypeIdentityDTO domainTypeIdentity, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/GetRichExceptionMessage", ReplyAction="http://tempuri.org/ConfigSLJsonController/GetRichExceptionMessageResponse")]
        System.IAsyncResult BeginGetRichExceptionMessage(Framework.Configuration.Generated.DTO.ExceptionMessageIdentityDTO exceptionMessageIdentity, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/GetRichSequence", ReplyAction="http://tempuri.org/ConfigSLJsonController/GetRichSequenceResponse")]
        System.IAsyncResult BeginGetRichSequence(Framework.Configuration.Generated.DTO.SequenceIdentityDTO sequenceIdentity, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/GetRichSystemConstant", ReplyAction="http://tempuri.org/ConfigSLJsonController/GetRichSystemConstantResponse")]
        System.IAsyncResult BeginGetRichSystemConstant(Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO systemConstantIdentity, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/GetRichTargetSystem", ReplyAction="http://tempuri.org/ConfigSLJsonController/GetRichTargetSystemResponse")]
        System.IAsyncResult BeginGetRichTargetSystem(Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO targetSystemIdentity, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/GetSimpleAttachment", ReplyAction="http://tempuri.org/ConfigSLJsonController/GetSimpleAttachmentResponse")]
        System.IAsyncResult BeginGetSimpleAttachment(Framework.Configuration.Generated.DTO.AttachmentIdentityDTO attachmentIdentity, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/GetSimpleAttachmentsByContainerReferenc" +
            "e", ReplyAction="http://tempuri.org/ConfigSLJsonController/GetSimpleAttachmentsByContainerReferenc" +
            "eResponse")]
        System.IAsyncResult BeginGetSimpleAttachmentsByContainerReference(Framework.Configuration.Generated.DTO.AttachmentContainerReferenceStrictDTO attachmentContainerReference, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/GetSimpleCodeFirstSubscription", ReplyAction="http://tempuri.org/ConfigSLJsonController/GetSimpleCodeFirstSubscriptionResponse")]
        System.IAsyncResult BeginGetSimpleCodeFirstSubscription(Framework.Configuration.Generated.DTO.CodeFirstSubscriptionIdentityDTO codeFirstSubscriptionIdentity, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/GetSimpleCodeFirstSubscriptions", ReplyAction="http://tempuri.org/ConfigSLJsonController/GetSimpleCodeFirstSubscriptionsResponse" +
            "")]
        System.IAsyncResult BeginGetSimpleCodeFirstSubscriptions(System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/GetSimpleDomainTypeByPath", ReplyAction="http://tempuri.org/ConfigSLJsonController/GetSimpleDomainTypeByPathResponse")]
        System.IAsyncResult BeginGetSimpleDomainTypeByPath(string path, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/GetSimpleDomainTypes", ReplyAction="http://tempuri.org/ConfigSLJsonController/GetSimpleDomainTypesResponse")]
        System.IAsyncResult BeginGetSimpleDomainTypes(System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/GetSimpleDomainTypesByRootFilter", ReplyAction="http://tempuri.org/ConfigSLJsonController/GetSimpleDomainTypesByRootFilterRespons" +
            "e")]
        System.IAsyncResult BeginGetSimpleDomainTypesByRootFilter(Framework.Configuration.Generated.DTO.DomainTypeRootFilterModelStrictDTO filter, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/GetSimpleSystemConstant", ReplyAction="http://tempuri.org/ConfigSLJsonController/GetSimpleSystemConstantResponse")]
        System.IAsyncResult BeginGetSimpleSystemConstant(Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO systemConstantIdentity, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/GetSimpleSystemConstants", ReplyAction="http://tempuri.org/ConfigSLJsonController/GetSimpleSystemConstantsResponse")]
        System.IAsyncResult BeginGetSimpleSystemConstants(System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/GetSimpleTargetSystems", ReplyAction="http://tempuri.org/ConfigSLJsonController/GetSimpleTargetSystemsResponse")]
        System.IAsyncResult BeginGetSimpleTargetSystems(System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/GetSimpleTargetSystemsByRootFilter", ReplyAction="http://tempuri.org/ConfigSLJsonController/GetSimpleTargetSystemsByRootFilterRespo" +
            "nse")]
        System.IAsyncResult BeginGetSimpleTargetSystemsByRootFilter(Framework.Configuration.Generated.DTO.TargetSystemRootFilterModelStrictDTO filter, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/ProcessModifications", ReplyAction="http://tempuri.org/ConfigSLJsonController/ProcessModificationsResponse")]
        System.IAsyncResult BeginProcessModifications(int limit, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/RemoveAttachment", ReplyAction="http://tempuri.org/ConfigSLJsonController/RemoveAttachmentResponse")]
        System.IAsyncResult BeginRemoveAttachment(Framework.Configuration.Generated.DTO.AttachmentIdentityDTO attachmentIdent, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/RemoveControlSettingsCollection", ReplyAction="http://tempuri.org/ConfigSLJsonController/RemoveControlSettingsCollectionResponse" +
            "")]
        System.IAsyncResult BeginRemoveControlSettingsCollection(System.Collections.ObjectModel.ObservableCollection<Framework.Configuration.Generated.DTO.ControlSettingsIdentityDTO> controlSettingsIdCollection, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/RemoveSequence", ReplyAction="http://tempuri.org/ConfigSLJsonController/RemoveSequenceResponse")]
        System.IAsyncResult BeginRemoveSequence(Framework.Configuration.Generated.DTO.SequenceIdentityDTO sequenceIdent, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/SaveAttachment", ReplyAction="http://tempuri.org/ConfigSLJsonController/SaveAttachmentResponse")]
        System.IAsyncResult BeginSaveAttachment(Framework.Configuration.Generated.DTO.SaveAttachmentRequest request, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/SaveCodeFirstSubscription", ReplyAction="http://tempuri.org/ConfigSLJsonController/SaveCodeFirstSubscriptionResponse")]
        System.IAsyncResult BeginSaveCodeFirstSubscription(Framework.Configuration.Generated.DTO.CodeFirstSubscriptionStrictDTO codeFirstSubscriptionStrict, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/SaveControlSettingsList", ReplyAction="http://tempuri.org/ConfigSLJsonController/SaveControlSettingsListResponse")]
        System.IAsyncResult BeginSaveControlSettingsList(System.Collections.ObjectModel.ObservableCollection<Framework.Configuration.Generated.DTO.ControlSettingsStrictDTO> settings, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/SaveExceptionMessage", ReplyAction="http://tempuri.org/ConfigSLJsonController/SaveExceptionMessageResponse")]
        System.IAsyncResult BeginSaveExceptionMessage(Framework.Configuration.Generated.DTO.ExceptionMessageStrictDTO exceptionMessageStrict, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/SaveSequence", ReplyAction="http://tempuri.org/ConfigSLJsonController/SaveSequenceResponse")]
        System.IAsyncResult BeginSaveSequence(Framework.Configuration.Generated.DTO.SequenceStrictDTO sequenceStrict, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/SaveSystemConstant", ReplyAction="http://tempuri.org/ConfigSLJsonController/SaveSystemConstantResponse")]
        System.IAsyncResult BeginSaveSystemConstant(Framework.Configuration.Generated.DTO.SystemConstantStrictDTO systemConstantStrict, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/SaveTargetSystem", ReplyAction="http://tempuri.org/ConfigSLJsonController/SaveTargetSystemResponse")]
        System.IAsyncResult BeginSaveTargetSystem(Framework.Configuration.Generated.DTO.TargetSystemStrictDTO targetSystemStrict, System.AsyncCallback callback, object asyncState);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ConfigSLJsonController/SetAttachmentTags", ReplyAction="http://tempuri.org/ConfigSLJsonController/SetAttachmentTagsResponse")]
        System.IAsyncResult BeginSetAttachmentTags(Framework.Configuration.Generated.DTO.SetAttachmentTagsRequest request, System.AsyncCallback callback, object asyncState);
        
        Framework.Configuration.Generated.DTO.SequenceRichDTO EndCreateSequence(System.IAsyncResult result);
        
        Framework.Configuration.Generated.DTO.UserActionRichDTO EndCreateUserAction(System.IAsyncResult result);
        
        void EndForceDomainTypeEvent(System.IAsyncResult result);
        
        Framework.Configuration.Generated.DTO.ControlSettingsRichDTO EndGetControlSettings(System.IAsyncResult result);
        
        Framework.Configuration.Generated.DTO.QueueProcessingStateSimpleDTO EndGetEventQueueProcessingState(System.IAsyncResult result);
        
        Framework.Configuration.Generated.DTO.CodeFirstSubscriptionFullDTO EndGetFullCodeFirstSubscription(System.IAsyncResult result);
        
        System.Collections.ObjectModel.ObservableCollection<Framework.Configuration.Generated.DTO.CodeFirstSubscriptionFullDTO> EndGetFullCodeFirstSubscriptionsByIdents(System.IAsyncResult result);
        
        System.Collections.ObjectModel.ObservableCollection<Framework.Configuration.Generated.DTO.CodeFirstSubscriptionFullDTO> EndGetFullCodeFirstSubscriptionsByRootFilter(System.IAsyncResult result);
        
        System.Collections.ObjectModel.ObservableCollection<Framework.Configuration.Generated.DTO.DomainTypeFullDTO> EndGetFullDomainTypes(System.IAsyncResult result);
        
        System.Collections.ObjectModel.ObservableCollection<Framework.Configuration.Generated.DTO.ExceptionMessageFullDTO> EndGetFullExceptionMessagesByIdents(System.IAsyncResult result);
        
        System.Collections.ObjectModel.ObservableCollection<Framework.Configuration.Generated.DTO.ExceptionMessageFullDTO> EndGetFullExceptionMessagesByRootFilter(System.IAsyncResult result);
        
        System.Collections.ObjectModel.ObservableCollection<Framework.Configuration.Generated.DTO.SequenceFullDTO> EndGetFullSequencesByIdents(System.IAsyncResult result);
        
        System.Collections.ObjectModel.ObservableCollection<Framework.Configuration.Generated.DTO.SequenceFullDTO> EndGetFullSequencesByRootFilter(System.IAsyncResult result);
        
        Framework.Configuration.Generated.DTO.SystemConstantFullDTO EndGetFullSystemConstant(System.IAsyncResult result);
        
        System.Collections.ObjectModel.ObservableCollection<Framework.Configuration.Generated.DTO.SystemConstantFullDTO> EndGetFullSystemConstantsByIdents(System.IAsyncResult result);
        
        System.Collections.ObjectModel.ObservableCollection<Framework.Configuration.Generated.DTO.SystemConstantFullDTO> EndGetFullSystemConstantsByRootFilter(System.IAsyncResult result);
        
        System.Collections.ObjectModel.ObservableCollection<Framework.Configuration.Generated.DTO.TargetSystemFullDTO> EndGetFullTargetSystemsByIdents(System.IAsyncResult result);
        
        System.Collections.ObjectModel.ObservableCollection<Framework.Configuration.Generated.DTO.TargetSystemFullDTO> EndGetFullTargetSystemsByRootFilter(System.IAsyncResult result);
        
        System.Collections.ObjectModel.ObservableCollection<Framework.Configuration.Generated.DTO.UserActionObjectFullDTO> EndGetFullUserActionObjects(System.IAsyncResult result);
        
        System.Collections.ObjectModel.ObservableCollection<Framework.Configuration.Generated.DTO.UserActionObjectFullDTO> EndGetFullUserActionObjectsByIdents(System.IAsyncResult result);
        
        System.Collections.ObjectModel.ObservableCollection<Framework.Configuration.Generated.DTO.UserActionObjectFullDTO> EndGetFullUserActionObjectsByRootFilter(System.IAsyncResult result);
        
        System.Collections.ObjectModel.ObservableCollection<Framework.Configuration.Generated.DTO.UserActionFullDTO> EndGetFullUserActionsByIdents(System.IAsyncResult result);
        
        Framework.Configuration.Generated.DTO.QueueProcessingStateSimpleDTO EndGetModificationQueueProcessingState(System.IAsyncResult result);
        
        Framework.Configuration.Generated.DTO.QueueProcessingStateSimpleDTO EndGetNotificationQueueProcessingState(System.IAsyncResult result);
        
        System.Collections.ObjectModel.ObservableCollection<Framework.Configuration.Generated.DTO.AttachmentTagRichDTO> EndGetRichAttachmentTagsEx(System.IAsyncResult result);
        
        Framework.Configuration.Generated.DTO.CodeFirstSubscriptionRichDTO EndGetRichCodeFirstSubscription(System.IAsyncResult result);
        
        Framework.Configuration.Generated.DTO.DomainTypeRichDTO EndGetRichDomainType(System.IAsyncResult result);
        
        Framework.Configuration.Generated.DTO.ExceptionMessageRichDTO EndGetRichExceptionMessage(System.IAsyncResult result);
        
        Framework.Configuration.Generated.DTO.SequenceRichDTO EndGetRichSequence(System.IAsyncResult result);
        
        Framework.Configuration.Generated.DTO.SystemConstantRichDTO EndGetRichSystemConstant(System.IAsyncResult result);
        
        Framework.Configuration.Generated.DTO.TargetSystemRichDTO EndGetRichTargetSystem(System.IAsyncResult result);
        
        Framework.Configuration.Generated.DTO.AttachmentSimpleDTO EndGetSimpleAttachment(System.IAsyncResult result);
        
        System.Collections.ObjectModel.ObservableCollection<Framework.Configuration.Generated.DTO.AttachmentSimpleDTO> EndGetSimpleAttachmentsByContainerReference(System.IAsyncResult result);
        
        Framework.Configuration.Generated.DTO.CodeFirstSubscriptionSimpleDTO EndGetSimpleCodeFirstSubscription(System.IAsyncResult result);
        
        System.Collections.ObjectModel.ObservableCollection<Framework.Configuration.Generated.DTO.CodeFirstSubscriptionSimpleDTO> EndGetSimpleCodeFirstSubscriptions(System.IAsyncResult result);
        
        Framework.Configuration.Generated.DTO.DomainTypeSimpleDTO EndGetSimpleDomainTypeByPath(System.IAsyncResult result);
        
        System.Collections.ObjectModel.ObservableCollection<Framework.Configuration.Generated.DTO.DomainTypeSimpleDTO> EndGetSimpleDomainTypes(System.IAsyncResult result);
        
        System.Collections.ObjectModel.ObservableCollection<Framework.Configuration.Generated.DTO.DomainTypeSimpleDTO> EndGetSimpleDomainTypesByRootFilter(System.IAsyncResult result);
        
        Framework.Configuration.Generated.DTO.SystemConstantSimpleDTO EndGetSimpleSystemConstant(System.IAsyncResult result);
        
        System.Collections.ObjectModel.ObservableCollection<Framework.Configuration.Generated.DTO.SystemConstantSimpleDTO> EndGetSimpleSystemConstants(System.IAsyncResult result);
        
        System.Collections.ObjectModel.ObservableCollection<Framework.Configuration.Generated.DTO.TargetSystemSimpleDTO> EndGetSimpleTargetSystems(System.IAsyncResult result);
        
        System.Collections.ObjectModel.ObservableCollection<Framework.Configuration.Generated.DTO.TargetSystemSimpleDTO> EndGetSimpleTargetSystemsByRootFilter(System.IAsyncResult result);
        
        int EndProcessModifications(System.IAsyncResult result);
        
        void EndRemoveAttachment(System.IAsyncResult result);
        
        void EndRemoveControlSettingsCollection(System.IAsyncResult result);
        
        void EndRemoveSequence(System.IAsyncResult result);
        
        Framework.Configuration.Generated.DTO.AttachmentIdentityDTO EndSaveAttachment(System.IAsyncResult result);
        
        Framework.Configuration.Generated.DTO.CodeFirstSubscriptionIdentityDTO EndSaveCodeFirstSubscription(System.IAsyncResult result);
        
        void EndSaveControlSettingsList(System.IAsyncResult result);
        
        Framework.Configuration.Generated.DTO.ExceptionMessageIdentityDTO EndSaveExceptionMessage(System.IAsyncResult result);
        
        Framework.Configuration.Generated.DTO.SequenceIdentityDTO EndSaveSequence(System.IAsyncResult result);
        
        Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO EndSaveSystemConstant(System.IAsyncResult result);
        
        Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO EndSaveTargetSystem(System.IAsyncResult result);
        
        void EndSetAttachmentTags(System.IAsyncResult result);
    }
}
