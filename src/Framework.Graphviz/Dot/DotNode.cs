using Framework.Graphviz.Dot.Attributes;

namespace Framework.Graphviz.Dot
{
    public class DotNode
    {
        public DotNode(string name) : this()
        {
            this.Name = name;
        }

        public DotNode()
        {
            this.Attributes = new DotAttribute();
        }

        public DotAttribute Attributes;

        public string Name { get; set; }

        public string NameFormatted
        {
            get
            {
                var name = this.Name.Replace(" ", "");
                return "\"" + name + "\"";
            }
        }

        public string Label { get; set; }
    }
}
