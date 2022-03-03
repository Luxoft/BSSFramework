using System;
using System.Collections.Generic;

using Framework.Authorization.BLL;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.ExpressionParsers;
using Framework.Validation;
using Framework.Workflow.BLL;

using JetBrains.Annotations;

namespace Framework.DomainDriven.ServiceModel.IAD
{
    public class WorkflowServiceEnvironmentModule
        //IWorkflowServiceEnvironment,
    {
        private readonly IAnonymousTypeBuilder<TypeMap<ParameterizedTypeMapMember>> workflowAnonymousTypeBuilder =
                new WorkflowAnonymousTypeBuilder(new AnonymousTypeBuilderStorageFactory().Create("Workflow_AnonymousType")).WithCompressName().WithCache().WithLock();

        private readonly TargetSystemServiceCompileCache<IAuthorizationBLLContext, Framework.Authorization.Domain.PersistentDomainObjectBase> workflowAuthorizationSystemCompileCache = new TargetSystemServiceCompileCache<IAuthorizationBLLContext, Framework.Authorization.Domain.PersistentDomainObjectBase>();

        private readonly Framework.Workflow.BLL.IExpressionParserFactory _workflowLambdaProcessorFactory =

                new Framework.Workflow.BLL.ExpressionParserFactory(CSharpNativeExpressionParser.Composite);

        private readonly IFetchService<Framework.Workflow.Domain.PersistentDomainObjectBase, FetchBuildRule> workflowFetchService;

        private readonly TargetSystemServiceCompileCache<TBLLContext, TPersistentDomainObjectBase> _workflowMainSystemCompileCache = new TargetSystemServiceCompileCache<TBLLContext, TPersistentDomainObjectBase>();

        public WorkflowServiceEnvironmentModule()
        {
            this.DefaultWorkflowValidatorCompileCache =

                    this.SessionFactory
                        .AvailableValues
                        .ToValidation()
                        .ToBLLContextValidationExtendedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Domain.PersistentDomainObjectBase, Guid>()
                        .Pipe(extendedValidationData => new WorkflowValidationMap(extendedValidationData))
                        .ToCompileCache();

            this.workflowFetchService = new WorkflowMainFetchService().WithCompress().WithCache().WithLock().Add(FetchService<Framework.Workflow.Domain.PersistentDomainObjectBase>.OData);
        }

        /// <summary>
        /// Кеш валидатора для утилит
        /// </summary>
        protected ValidatorCompileCache DefaultWorkflowValidatorCompileCache { get; }

        protected virtual IEnumerable<IDALListener> GetDALFlushedListeners(TBLLContextContainer container)
        {
            foreach (var listener in this.GetWorkflowDALListeners(container.Workflow))
            {
                yield return listener;
            }
        }

        private IEnumerable<IDALListener> GetWorkflowDALListeners(IWorkflowBLLContext context)
        {
            return from targetSystemService in context.GetTargetSystemServices()

                   select new WorkflowDALListener(targetSystemService);
        }

        #region IServiceEnvironment<WorkflowBLLContext> Members

        IBLLContextContainer<IWorkflowBLLContext> IServiceEnvironment<IWorkflowBLLContext>.GetBLLContextContainer(IServiceProvider serviceProvider, IDBSession session, string currentPrincipalName)
        {
            return this.GetBLLContextContainerBase(serviceProvider, session, currentPrincipalName);
        }

        #endregion
    }
}
