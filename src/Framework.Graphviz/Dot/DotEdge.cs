using Framework.Graphviz.Dot.Attributes;

namespace Framework.Graphviz.Dot
{
    public class DotEdge
    {
        public DotAttribute Attributes;

        public DotNode StartNode { get; set; }

        public DotNode EndNode { get; set; }

        public DotEdge(DotNode startNode, DotNode endNode)
        {
            this.StartNode = startNode;
            this.EndNode = endNode;
            this.Attributes = new DotAttribute();
        }

        public DotEdge(DotNode startNode, DotNode endNode, DotAttribute attributes)
            : this(startNode, endNode)
        {
            this.Attributes = attributes;
        }
    }
}
