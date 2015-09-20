using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Songbook.Structure
{
    public sealed class LineNode : Node
    {
        public LineNode(IEnumerable<LinePartNode> parts)
        {
            Parts = parts as ReadOnlyCollection<LinePartNode>;
            if (Parts == null)
            {
                Parts = (parts ?? new List<LinePartNode>()).ToList().AsReadOnly();
            }
        }

        public bool IsBlank =>
            Parts.Count == 0 || (Parts.Count == 1 && Parts[0].Kind == NodeKind.WhiteSpace);

        public override NodeKind Kind => NodeKind.Line;

        public ReadOnlyCollection<LinePartNode> Parts { get; }

        public LineNode Update(IEnumerable<LinePartNode> parts)
        {
            if (parts != Parts)
            {
                return new LineNode(parts);
            }

            return this;
        }
    }
}
