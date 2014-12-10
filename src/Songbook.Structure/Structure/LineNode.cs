using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Songbook.Structure
{
    public class LineNode : Node
    {
        public LineNode(LineInfo lineInfo, IEnumerable<LinePartNode> parts)
            : base(lineInfo)
        {
            Parts = (parts ?? new List<LinePartNode>()).ToList().AsReadOnly();
        }

        public override NodeKind Kind
        {
            get { return NodeKind.Line; }
        }

        public ReadOnlyCollection<LinePartNode> Parts { get; private set; }
    }
}
