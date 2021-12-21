using System.Collections.Generic;
using System.Text;

namespace Framework.Graphviz.Dot.Attributes
{
    public class DotAttribute
    {
        public DotAttribute()
        {
            this.MultilineLabel = new List<string>();
        }

        public DotAttribute(DotAttribute attributeToCopy)
            : this()
        {
            this.HeadType = attributeToCopy.HeadType;
            this.Shape = attributeToCopy.Shape;
            this.Color = attributeToCopy.Color;
            this.FillColor = attributeToCopy.FillColor;
            this.Label = attributeToCopy.Label;
            this.MultilineLabel = attributeToCopy.MultilineLabel;
        }

        public DotHeadType HeadType { get; set; }

        public DotShape Shape { get; set; }

        public DotColor Color { get; set; }

        public DotColor FillColor { get; set; }

        public string Label { get; set; }

        public List<string> MultilineLabel { get; set; }

        public override string ToString()
        {
            var attributes = new StringBuilder("[");
            if (this.Label != "")
                attributes.AppendFormat(@"{0}=""{1}"" fontsize = 7 ", "label", this.Label);

            attributes.AppendFormat(@"{0}={1} ", "shape", this.Shape.ToString().ToLower());
            attributes.Append("]");

            return attributes.ToString();
        }

        public string ShapeToString()
        {
            var attributes = new StringBuilder("[");

            attributes.AppendFormat(@"{0} = {1} ", "shape", this.Shape.ToString().ToLower());
            attributes.Append("]");

            return attributes.ToString();
        }

        public string ColorToString()
        {
            var attributes = new StringBuilder("[");

            attributes.AppendFormat(@"{0} = {1} ", "color", this.Color.ToString().ToLower());
            attributes.Append("]");

            return attributes.ToString();
        }

        public string ColorLabelHeadTypeToString()
        {
            var attributes = new StringBuilder("[");

            if (this.Label != "")
                attributes.AppendFormat(@"{0}=""{1}"" fontsize = 7 ", "label", this.Label);

            attributes.AppendFormat(@"{0} = {1} ", "color", this.Color.ToString().ToLower());
            attributes.AppendFormat(@"{0} = {1} ", "arrowhead", this.HeadType.ToString().ToLower());
            attributes.Append("]");

            return attributes.ToString();
        }

        public string ShapeColorLabelToString()
        {
            var attributes = new StringBuilder("[");

            if (this.FillColor != DotColor.Black)
            {
                attributes.AppendFormat(@"{0} = {1} style=filled ", "fillcolor", this.FillColor.ToString().ToLower());
            }

            if (!string.IsNullOrWhiteSpace(this.Label) || this.MultilineLabel.Count != 0)
            {
                if (this.MultilineLabel.Count != 0)
                {
                    attributes.AppendFormat(@"{0} = ""{1}"" ", "label", this.MultilineLabelFullString());
                }
                else
                {
                    attributes.AppendFormat(@"{0} = ""{1}"" ", "label", this.Label);
                }
            }

            attributes.AppendFormat(@"{0} = {1} ", "color", this.Color.ToString().ToLower());
            attributes.AppendFormat(@"{0} = {1} ", "shape", this.Shape.ToString().ToLower());
            attributes.Append("]");

            return attributes.ToString();
        }

        public string MultilineLabelToString()
        {
            if (this.MultilineLabel.Count == 0)
                return "";

            var sb = new StringBuilder();

            sb.AppendLine(this.Label);

            if (this.MultilineLabel.Count == 1)
            {
                sb.AppendLine(this.MultilineLabel[0]);
            }
            else
            {
                if (this.MultilineLabel.Count > 2)
                    sb.AppendLine("...");

                sb.AppendLine("  " + this.MultilineLabel[this.MultilineLabel.Count - 2] + "  ");
                sb.AppendLine("  " + this.MultilineLabel[this.MultilineLabel.Count - 1] + "  ");
            }

            return sb.ToString();
        }

        public string MultilineLabelFullString()
        {
            if (this.MultilineLabel.Count == 0)
                return "";

            var sb = new StringBuilder();

            if (this.MultilineLabel.Count == 1)
            {
                sb.AppendLine(this.MultilineLabel[0]);
            }
            else
            {
                foreach (var label in this.MultilineLabel)
                {
                    sb.AppendLine("  " + label + "  ");
                }
            }

            return sb.ToString();
        }

    }
}
