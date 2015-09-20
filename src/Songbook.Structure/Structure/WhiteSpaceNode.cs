using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Songbook.Structure
{
    public sealed class WhiteSpaceNode : LinePartNode
    {
        public WhiteSpaceNode(string text)
        {
            Text = text;
        }

        public override NodeKind Kind => NodeKind.WhiteSpace;

        public string Text { get; }
    }
}
