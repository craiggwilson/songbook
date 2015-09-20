using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Songbook.Structure
{
    public sealed class TextNode : LinePartNode
    {
        public TextNode(string text)
        {
            Text = text;
        }

        public override NodeKind Kind => NodeKind.Text;

        public string Text { get; }
    }
}
