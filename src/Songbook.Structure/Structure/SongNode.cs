using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Songbook.Structure
{
    public class SongNode : Node
    {
        public SongNode(LineInfo lineInfo, IEnumerable<LineNode> lines)
            : base(lineInfo)
        {
            Lines = (lines ?? new List<LineNode>()).ToList().AsReadOnly();
        }

        public override NodeKind Kind
        {
            get { return NodeKind.Song; }
        }

        public ReadOnlyCollection<LineNode> Lines { get; private set; }
    }
}
