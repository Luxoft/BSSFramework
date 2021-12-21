using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Framework.Core;
using Framework.Workflow.Domain.Definition;

using JetBrains.Annotations;

namespace Framework.Workflow.BLL
{
    public partial class WorkflowMachineBase<TBLLContext, TPersistentDomainObjectBase, TDomainObject, TWorkflow>
    {
        public IEnumerable<string> GetReversePath(WorkflowSource workflowSource)
        {
            if (workflowSource == null) throw new ArgumentNullException(nameof(workflowSource));

            return this.GetTaskInstanceGroupPathInternal(workflowSource).SelectMany(v => v);
        }

        private IEnumerable<string[]> GetTaskInstanceGroupPathInternal(WorkflowSource workflowSource)
        {
            if (workflowSource == null) throw new ArgumentNullException(nameof(workflowSource));

            if (this.WorkflowInstance.Definition == workflowSource.Workflow)
            {
                if (workflowSource.Path == null)
                {
                    yield return this.GetTaskInstanceGroupDefaultPathElement();
                }
                else
                {
                    yield return this.GetTaskInstanceGroupRootPathElement(workflowSource);
                }
            }
            else
            {
                yield return this.GetTaskInstanceGroupDefaultPathElement();

                if (this.WorkflowInstance.Owner == null)
                {
                    throw new ArgumentOutOfRangeException(nameof(workflowSource), "Invalid workflowSource");
                }
                else
                {
                    yield return this.Context.GetWorkflowMachine(this.WorkflowInstance.Owner).GetReversePath(workflowSource).ToArray();
                }
            }
        }

        private string[] GetTaskInstanceGroupRootPathElement([NotNull] WorkflowSource workflowSource)
        {
            if (workflowSource == null) throw new ArgumentNullException(nameof(workflowSource));

            return new Func<WorkflowSource, IEnumerable<string>>(this.GetTaskInstanceGroupRootPathElement<TPersistentDomainObjectBase>)
                  .CreateGenericMethod(this.TargetSystemService.TypeResolver.Resolve(workflowSource.Type, true))
                  .Invoke<string[]>(this, workflowSource);
        }

        private string[] GetTaskInstanceGroupRootPathElement<TSourceDomainObject>([NotNull] WorkflowSource workflowSource)
            where TSourceDomainObject : class, TPersistentDomainObjectBase
        {
            if (workflowSource == null) throw new ArgumentNullException(nameof(workflowSource));

            var pathDel = this.Context.ExpressionParsers.GetByWorkflowSourcePath<TBLLContext, TSourceDomainObject, TWorkflow>().GetDelegate(workflowSource);

            if (workflowSource.IsDefault)
            {
                return pathDel(this.TargetSystemService.TargetSystemContext, this.WorkflowObject.DomainObject as TSourceDomainObject, this.WorkflowObject).Reverse().ToArray();
            }
            else
            {
                var elementsDel = this.Context.ExpressionParsers.GetByWorkflowSourceElements<TBLLContext, TSourceDomainObject, TDomainObject>().GetDelegate(workflowSource);

                var elementsExpr = elementsDel(this.TargetSystemService.TargetSystemContext);

                var domainObjectId = this.WorkflowInstance.DomainObjectId;

                var filterExpr = elementsExpr.Select(elements => elements.Any(element => element.Id == domainObjectId));

                try
                {
                    var sourceDomainObject = this.TargetSystemService.TargetSystemContext.Logics.Default.Create<TSourceDomainObject>().GetObjectBy(filterExpr, true);

                    return pathDel(this.TargetSystemService.TargetSystemContext, sourceDomainObject, this.WorkflowObject).Reverse().ToArray();
                }
                catch (Exception)
                {
                    Trace.WriteLine("WorkflowInstanceId: " + this.WorkflowInstance.Id);
                    Trace.WriteLine("DomainObjectId: " + domainObjectId);

                    throw;
                }
            }
        }

        private string[] GetTaskInstanceGroupDefaultPathElement()
        {
            var request = from defaultSource in this.WorkflowInstance.Definition.DefaultSource.ToMaybe()

                          from path in defaultSource.Path.ToMaybe()

                          let pathDel = this.Context.ExpressionParsers.GetByWorkflowSourcePath<TBLLContext, TDomainObject, TWorkflow>().GetDelegate(defaultSource)

                          select pathDel(this.TargetSystemService.TargetSystemContext, this.WorkflowObject.DomainObject, this.WorkflowObject).Reverse().ToArray();

            return request.GetValueOrDefault(new[] { this.WorkflowInstance.Name });
        }
    }
}
