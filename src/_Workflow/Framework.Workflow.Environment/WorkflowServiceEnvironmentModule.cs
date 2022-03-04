using System;
using System.Linq;
using System.Collections.Generic;

using Framework.Authorization.BLL;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.ExpressionParsers;
using Framework.Graphviz;
using Framework.Graphviz.Dot;
using Framework.Persistent;
using Framework.Validation;
using Framework.Workflow.BLL;

using JetBrains.Annotations;

namespace Framework.Workflow.Environment;

public class WorkflowServiceEnvironmentModule<TMainServiceEnvironment, TBLLContextContainer, TBLLContext, TPersistentDomainObjectBase> : IServiceEnvironmentModule<TBLLContextContainer>, IWorkflowServiceEnvironment
        where TMainServiceEnvironment : class, IRootServiceEnvironment<TBLLContext, TBLLContextContainer>
        where TBLLContextContainer : ServiceEnvironmentBase.ServiceEnvironmentBLLContextContainer, IBLLContextContainer<IWorkflowBLLContext>, IWorkflowBLLContextContainer
        where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
        where TBLLContext : IDefaultBLLContext<TPersistentDomainObjectBase, Guid>
{
    private readonly TMainServiceEnvironment mainServiceEnvironment;

    public WorkflowServiceEnvironmentModule([NotNull] TMainServiceEnvironment mainServiceEnvironment, [NotNull] IDotVisualizer<DotGraph> dotVisualizer)
    {
        this.mainServiceEnvironment = mainServiceEnvironment ?? throw new ArgumentNullException(nameof(mainServiceEnvironment));

        this.DotVisualizer = dotVisualizer ?? throw new ArgumentNullException(nameof(dotVisualizer));

        this.DefaultWorkflowValidatorCompileCache =

                this.SessionFactory
                    .AvailableValues
                    .ToValidation()
                    .ToBLLContextValidationExtendedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Domain.PersistentDomainObjectBase, Guid>()
                    .Pipe(extendedValidationData => new WorkflowValidationMap(extendedValidationData))
                    .ToCompileCache();
    }

    public ValidatorCompileCache DefaultWorkflowValidatorCompileCache { get; }

    public IValidator WorkflowAnonymousObjectValidator { get; } = new Validator(new ValidationMap(ValidationExtendedData.Infinity).ToCompileCache());

    public IAnonymousTypeBuilder<TypeMap<ParameterizedTypeMapMember>> WorkflowAnonymousTypeBuilder { get; } =

        new WorkflowAnonymousTypeBuilder(new AnonymousTypeBuilderStorageFactory().Create("Workflow_AnonymousType"))
                .WithCompressName()
                .WithCache()
                .WithLock();

    public TargetSystemServiceCompileCache<IAuthorizationBLLContext, Framework.Authorization.Domain.PersistentDomainObjectBase> WorkflowAuthorizationSystemCompileCache { get; } = new();

    public TargetSystemServiceCompileCache<TBLLContext, TPersistentDomainObjectBase> WorkflowMainSystemCompileCache { get; } = new();

    public Framework.Workflow.BLL.IExpressionParserFactory WorkflowLambdaProcessorFactory { get; } = new Framework.Workflow.BLL.ExpressionParserFactory(CSharpNativeExpressionParser.Composite);

    public IFetchService<Framework.Workflow.Domain.PersistentDomainObjectBase, FetchBuildRule> WorkflowFetchService { get; } =

        new WorkflowMainFetchService().WithCompress().WithCache().WithLock().Add(FetchService<Framework.Workflow.Domain.PersistentDomainObjectBase>.OData);



    public IDBSessionFactory SessionFactory => this.mainServiceEnvironment.SessionFactory;

    public IServiceProvider RootServiceProvider => this.mainServiceEnvironment.RootServiceProvider;

    public bool IsDebugMode => this.mainServiceEnvironment.IsDebugMode;

    public IDotVisualizer<DotGraph> DotVisualizer { get; }


    public virtual IEnumerable<IDALListener> GetDALFlushedListeners(TBLLContextContainer container)
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


    public IContextEvaluator<IWorkflowBLLContext> GetContextEvaluator(IServiceProvider currentScopedServiceProvider = null)
    {
        return currentScopedServiceProvider == null
                       ? new RootContextEvaluator<IWorkflowBLLContext>(this, this.RootServiceProvider)
                       : new ScopeContextEvaluator<IWorkflowBLLContext>(this, currentScopedServiceProvider);
    }

    public IBLLContextContainer<IWorkflowBLLContext> GetBLLContextContainer(
            IServiceProvider serviceProvider,
            IDBSession session,
            string currentPrincipalName = null)
    {
        return this.mainServiceEnvironment.GetBLLContextContainer(serviceProvider, session, currentPrincipalName);
    }
}
