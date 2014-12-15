using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Songbook.Structure
{
    public class WhiteSpaceNode : LinePartNode
    {
        public WhiteSpaceNode(LineInfo lineInfo, string text)
            : base(lineInfo)
        {
            Text = text;
        }

        public override NodeKind Kind
        {
            get { return NodeKind.WhiteSpace; }
        }

        public string Text { get; private set; }
    }
}
