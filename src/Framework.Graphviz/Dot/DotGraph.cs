using System.Collections.Generic;
using System.Text;

using Framework.Graphviz.Dot.Attributes;

namespace Framework.Graphviz.Dot
{
    public enum DotGraphType { NonDirected = 0, Directed }

    public class DotGraph
    {
        public DotGraph(DotGraphType graphType = DotGraphType.Directed)
        {
            this.GraphType = graphType;
            this.Edges = new List<DotEdge>();
            this.Dpi = 300;
        }


        public DotGraphType GraphType { get; protected set; }

        public List<DotEdge> Edges;

        public int Dpi { get; set; }


        public override string ToString()
        {
            var sb = new StringBuilder();

            if (this.GraphType == DotGraphType.Directed)
                sb.AppendLine("digraph gr {");
            else
                sb.AppendLine("graph gr {");

            sb.AppendFormat("graph [dpi = {0}]", this.Dpi);
            sb.AppendLine("Node [shape = box]");

            foreach (var dotEdge in this.Edges)
            {
                sb.AppendLine("edge " + dotEdge.Attributes.ColorLabelHeadTypeToString());

                var tempNode = new DotNode
                    {
                        Attributes = new DotAttribute(dotEdge.EndNode.Attributes)
                    };

                var multiline = dotEdge.Attributes.MultilineLabelToString();
                if (multiline != "")
                    tempNode.Attributes.Label = multiline;
                else
                    tempNode.Attributes.Label = dotEdge.Attributes.Label;

                sb.AppendLine(dotEdge.StartNode.NameFormatted + " -> " + dotEdge.EndNode.NameFormatted + tempNode.Attributes);

                sb.AppendLine(dotEdge.StartNode.NameFormatted + " " + dotEdge.StartNode.Attributes.ShapeColorLabelToString());
                sb.AppendLine(dotEdge.EndNode.NameFormatted + " " + dotEdge.EndNode.Attributes.ShapeColorLabelToString());

            }

            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}
