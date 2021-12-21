using System;
using System.Linq;

using Framework.Core;
using Framework.Graphviz.Dot;
using Framework.Graphviz.Dot.Attributes;
using Framework.Workflow.Domain.Definition;
using Framework.Workflow.Domain.Runtime;

using JetBrains.Annotations;

namespace Framework.Workflow.Graphviz
{
    public static class DotGraphExtensions
    {
        public static DotGraph GetDotGraph([NotNull] this Domain.Definition.Workflow workflow)
        {
            if (workflow == null) throw new ArgumentNullException(nameof(workflow));

            var dot = new DotGraph();

            foreach (var transit in workflow.Transitions)
            {
                var startNode = new DotNode(transit.From.Name);
                var endNode = new DotNode(transit.To.Name);

                if (transit.From.IsInitial)
                {
                    startNode.Attributes.Shape = DotShape.Ellipse;
                }

                switch (transit.From.Type)
                {
                    case StateType.Condition:
                        {
                            startNode.Attributes.Shape = DotShape.Diamond;
                            startNode.Attributes.Label = string.Format("{1}{0}{1}", startNode.Name, Environment.NewLine);
                            break;
                        }
                    case StateType.Parallel:
                        {
                            startNode.Attributes.Shape = DotShape.Trapezium;
                            break;
                        }

                }

                var lastState = workflow.States.FirstOrDefault(x => transit.To.Id == x.Id);
                if (lastState != null)
                {

                    if (lastState.IsFinal)
                    {
                        endNode.Attributes.Shape = DotShape.Ellipse;
                    }
                }

                switch (transit.To.Type)
                {
                    case StateType.Condition:
                        {
                            endNode.Attributes.Shape = DotShape.Diamond;
                            endNode.Attributes.Label = string.Format("{1}{0}{1}", endNode.Name, Environment.NewLine);
                            break;
                        }
                    case StateType.Parallel:
                        {
                            endNode.Attributes.Shape = DotShape.Trapezium;
                            break;
                        }

                }

                var edge = new DotEdge(startNode, endNode)
                {
                    Attributes =
                    {
                        Label = transit.Name,
                        Color = DotColor.Blue,
                        HeadType = DotHeadType.Vee
                    }
                };

                dot.Edges.Add(edge);
            }

            return dot;
        }

        public static DotGraph GetDotGraph([NotNull] this WorkflowInstance workflowInstance)
        {
            if (workflowInstance == null) throw new ArgumentNullException(nameof(workflowInstance));

            var dot = GetDotGraph(workflowInstance.Definition);

            var currentStateId = workflowInstance.CurrentState.Id;

            foreach (var transit in workflowInstance.Transitions)
            {
                var richFrom = transit.From;
                var richTo = transit.To;

                var edge = dot.Edges.First(x =>
                                            x.StartNode.Name == richFrom.Definition.Name &&
                                            x.EndNode.Name == richTo.Definition.Name);

                if (richFrom.Id == currentStateId)
                {
                    edge.StartNode.Attributes.FillColor = DotColor.Orange;
                }

                if (richTo.Id == currentStateId)
                {
                    edge.EndNode.Attributes.FillColor = DotColor.Orange;
                }

                if (richFrom.SubWorkflows != null && richFrom.SubWorkflows.Any())
                {
                    edge.StartNode.Attributes.Shape = DotShape.Trapezium;
                    edge.StartNode.Attributes.MultilineLabel.Add(edge.StartNode.Name);
                    edge.StartNode.FillNode(richFrom);
                }
                if (richTo.SubWorkflows != null && richTo.SubWorkflows.Any())
                {
                    edge.EndNode.Attributes.Shape = DotShape.Trapezium;
                    edge.EndNode.Attributes.MultilineLabel.Add(edge.EndNode.Name);
                    edge.EndNode.FillNode(richTo);
                }

                var modifiedByName = transit.ModifiedBy;
                if (modifiedByName.Contains('\\'))
                {
                    modifiedByName = modifiedByName.Remove(0, modifiedByName.IndexOf('\\'));
                }

                var dateFormatted = (transit.ModifyDate.HasValue ? transit.ModifyDate.Value : transit.CreateDate.Value).ToString("yyyy-MM-dd HH:mm");
                var newLabel = $"({modifiedByName}, {dateFormatted})";

                edge.Attributes.MultilineLabel.Add(newLabel);

                edge.Attributes.Color = DotColor.Orange;
            }

            return dot;
        }

        public static DotGraph GetDotGraph(this Domain.Definition.Workflow workflow, int? dpi)
        {
            var dot = workflow.GetDotGraph();

            dpi.MaybeNullable(outDpi => dot.Dpi = outDpi);

            return dot;
        }

        public static DotGraph GetDotGraph(this WorkflowInstance workflowInstance, int? dpi)
        {
            var dot = workflowInstance.GetDotGraph();

            dpi.MaybeNullable(outDpi => dot.Dpi = outDpi);

            return dot;
        }

        private static void FillNode(this DotNode node, StateInstance parallelState)
        {
            if (parallelState.SubWorkflows != null && parallelState.SubWorkflows.Any())
            {
                foreach (var wFlow in parallelState.SubWorkflows)
                {
                    if (wFlow.CurrentState != null)
                    {
                        var resultStr = $"{wFlow.Name} : {wFlow.CurrentState.Definition.Name}";
                        if (wFlow.IsAborted)
                        {
                            resultStr += " (Aborted)";
                        }
                        node.Attributes.MultilineLabel.Add(resultStr);
                    }
                }
            }
        }
    }
}
