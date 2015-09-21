using System.Linq;
using Songbook.Structure;

namespace Songbook.Formats.TwoLineTextFormat
{
    public class SectionNamingVisitor : NodeVisitor
    {
        protected override Node VisitSection(SectionNode node)
        {
            var sectionName = node.Name;
            if (node.Lines[0].Parts.Count > 0 && node.Lines[0].Parts[0].Kind == NodeKind.Text)
            {
                var textNode = (TextNode)node.Lines[0].Parts[0];
                if (textNode.Text.StartsWith("[") && textNode.Text.EndsWith("]"))
                {
                    sectionName = textNode.Text.Substring(1, textNode.Text.Length - 2);
                }
            }

            if (sectionName != node.Name)
            {
                return new SectionNode(sectionName, node.Lines);
            }

            return node;
        }
    }
}
