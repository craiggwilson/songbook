using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Songbook.Structure
{
    public sealed class SectionNode : Node
    {
        public SectionNode(string name, IEnumerable<LineNode> lines)
        {
            Name = name;
            Lines = lines as ReadOnlyCollection<LineNode>;
            if (Lines == null)
            {
                Lines = (lines ?? new List<LineNode>()).ToList().AsReadOnly();
            }
        }

        public override NodeKind Kind => NodeKind.Section;

        public ReadOnlyCollection<LineNode> Lines { get; }

        public string Name { get; }

        public SectionNode Update(IEnumerable<LineNode> lines)
        {
            if (lines != Lines)
            {
                return new SectionNode(Name, lines);
            }

            return this;
        }
    }
}
